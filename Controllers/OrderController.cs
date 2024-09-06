using Azure.Core;
using Bakery.Interfaces;
using Bakery.Models.DbModels;
using Bakery.Models.DTO;
using Bakery.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Створення клієнта
        /// </summary>
        [HttpPost("create-customer")]
        public async Task<JsonResult> CreateCustomer([FromBody] CustomerDto customer)
        {
            try
            {
                var customerId = await _orderService.CreateCustomer(customer);
                return new JsonResult(new ApiResult(true, customerId));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return new JsonResult(new ApiResult(ex.Message));
            }
        }

        /// <summary>
        /// Створення замовлення
        /// </summary>
        [HttpPost("create-order")]
        public async Task<JsonResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                var result = await _orderService.CreateOrder(request);
                return new JsonResult(new ApiResult(result));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return new JsonResult(new ApiResult(ex.Message));
            }
        }

        /// <summary>
        /// Отримання замовлень
        /// </summary>
        [HttpGet]
        public async Task<JsonResult> GetOrders([FromQuery] long? orderId, [FromQuery] string? phone)
        {
            try
            {
                var orders = await _orderService.GetOrders(orderId, phone);
                return new JsonResult(new ApiResult(true, orders));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return new JsonResult(new ApiResult(ex.Message));
            }
        }

        /// <summary>
        /// Редагування позицій замовлення
        /// </summary>
        [HttpPut]
        public async Task<JsonResult> EditOrderItems([FromBody] ChangeOrderRequest request)
        {
            try
            {
                var result = await _orderService.EditOrderItems(request);
                return new JsonResult(new ApiResult(result));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return new JsonResult(new ApiResult(ex.Message));
            }
        }

        /// <summary>
        /// Видалення замовлення
        /// </summary>
        [HttpDelete]
        public async Task<JsonResult> DeleteOrder([FromQuery] long orderId)
        {
            try
            {
                var result = await _orderService.DeleteOrder(orderId);
                return new JsonResult(new ApiResult(result));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return new JsonResult(new ApiResult(ex.Message));
            }
        }

        /// <summary>
        /// Зміна статусу замовлення
        /// </summary>
        [HttpPatch]
        public async Task<JsonResult> ChangeOrderStatus([FromBody] ChangeOrderStatusRequest request)
        {
            try
            {
                var result = await _orderService.ChangeOrderStatus(request);
                return new JsonResult(new ApiResult(result));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return new JsonResult(new ApiResult(ex.Message));
            }
        }
    }
}
