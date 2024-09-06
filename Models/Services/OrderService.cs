using Azure.Core;
using Bakery.Interfaces;
using Bakery.Models.DbModels;
using Bakery.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Bakery.Models.Services
{
    public class OrderService: IOrderService
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly BakeryContext _context;

        public OrderService(BakeryContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateOrder(CreateOrderRequest request)
        {
            // Створення нового замовлення
            var order = new Order
            {
                OrderDate = DateTime.Now,
                CustomerId = request.CustomerId,
                StatusId = 1, // Встановлюємо дефолтний статус
                TotalCost = 0
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // Зберігаємо, щоб отримати ID замовлення

            // Додавання позицій продукту в замовлення
            foreach (var item in request.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product is null)
                    throw new Exception($"Продукт з Id {item.ProductId} не знайдений");

                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    ItemCost = item.Quantity * product.Price
                };

                order.TotalCost += orderItem.ItemCost;
                _context.OrderItems.Add(orderItem);
            }

            // Оновлення загальної вартості та зберігання позицій
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<List<OrderResponse>> GetOrders(long? orderId = null, string? phone = null)
        {
            var query = from order in _context.Orders
                        join customer in _context.Customers
                        on order.CustomerId equals customer.Id
                        select new OrderResponse()
                        {
                            Id = order.Id,
                            OrderDate = order.OrderDate,
                            CustomerId = customer.Id,
                            StatusId = order.StatusId,
                            TotalCost = order.TotalCost,
                            CustomerName = customer.Name,
                            CustomerPhone = customer.Phone,
                            CustomerDeliveryAddress = customer.DeliveryAddress
                        };

            if (orderId.HasValue)
            {
                query = query.Where(o => o.Id == orderId);
            }

            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(p => p.CustomerPhone == phone);
            }

            var result = await query.ToListAsync();
            List<long> ids = result.Select(s => s.Id ?? -1).ToList();

            var orderItems = (from oi in _context.OrderItems
                              join product in _context.Products
                              on oi.ProductId equals product.Id
                              where ids.Contains(oi.OrderId)
                              select new OrderItemDto()
                              {
                                  Id = oi.Id,
                                  OrderId = oi.OrderId,
                                  ItemCost = oi.ItemCost,
                                  Quantity = oi.Quantity,
                                  ProductId = product.Id,
                                  ProductName = product.Name
                              }).ToList();

            if (orderItems.Any()) {
                foreach (var item in result) { 
                    item.OrderItems = orderItems.Where(w => w.OrderId == item.Id).ToList();
                }
            }

            return result;
        }

        public async Task<long> CreateCustomer(CustomerDto customer)
        {
            if (string.IsNullOrEmpty(customer.Name) || string.IsNullOrEmpty(customer.Phone) || string.IsNullOrEmpty(customer.DeliveryAddress))
                throw new Exception("Заповніть всі дані по клієнту");

            long? sameCustomer = GetSameCustomer(customer);
            if (sameCustomer is not null && sameCustomer > 0)
                return sameCustomer.Value;

            var customerDb = new Customer()
            {
                Name = customer.Name,
                Phone = customer.Phone,
                DeliveryAddress = customer.DeliveryAddress
            };

            _context.Customers.Add(customerDb);
            await _context.SaveChangesAsync();

            return customerDb.Id;
        }

        long? GetSameCustomer(CustomerDto customer)
        {
            return _context.Customers.Where(w => w.Name == customer.Name && w.Phone == customer.Phone && w.DeliveryAddress == customer.DeliveryAddress).Select(s => s.Id).FirstOrDefault();
        }

        public async Task<bool> EditOrderItems(ChangeOrderRequest request)
        {
            // Пошук замовлення
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId);

            // Перевірка існування замовлення
            if (order is null)
                throw new Exception("Замовлення не знайдене");

            // Перевірка наявності позицій для зміни
            if (request.OrderItems is null || request.OrderItems.Count == 0)
                throw new Exception("Відсутні позиції для зміни");

            if (order.OrderItems is null)
                order.OrderItems = new List<OrderItem>();

            // Видалення продуктів, які є в базі та немає в запиті
            List<long> ids = request.OrderItems.Select(s => s.Id).ToList();
            List<OrderItem> orderForDelete = order.OrderItems.Where(w => !ids.Contains(w.Id)).ToList();
            if (orderForDelete.Count > 0)
                _context.OrderItems.RemoveRange(orderForDelete);

            // Загальну вартість робимо нулем перед новим перерахунком
            order.TotalCost = 0;

            // Обробка нових та існуючих позицій замовлення
            foreach (var item in request.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Продукт з Id {item.ProductId} не знайдений");

                decimal itemCost = product.Price * item.Quantity;

                // Якщо ID негативний, то позиції в базі ще немає - робимо нову
                if (item.Id < 0)
                {
                    var newOrderItem = new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        ItemCost = itemCost,
                        OrderId = order.Id
                    };

                    _context.OrderItems.Add(newOrderItem);
                }
                else
                {
                    // Оновлення існуючої позиції
                    var orderForUpdate = order.OrderItems.FirstOrDefault(w => w.Id == item.Id);
                    if (orderForUpdate is not null)
                    {
                        orderForUpdate.ProductId = product.Id;
                        orderForUpdate.Quantity = item.Quantity;
                        orderForUpdate.ItemCost = itemCost;
                    }
                }

                // Перерахунок загальної вартості замовлення
                order.TotalCost += itemCost;
            }

            // Зберігаємо всі зміни
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> DeleteOrder(long orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order is null)
            {
                throw new Exception("Замовлення не знайдене");
            }

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeOrderStatus(ChangeOrderStatusRequest request)
        {
            var order = await _context.Orders.FindAsync(request.OrderId);
            if (order is null)
            {
                throw new Exception("Замовлення не знайдене");
            }

            order.StatusId = request.StatusId;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
