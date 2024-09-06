namespace Bakery.Models.DTO
{
    public class OrderResponse : CreateOrderRequest
    {
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerDeliveryAddress { get; set; }

        public string StatusName
        {
            get
            {
                return GetStatusName(this.StatusId);
            }
        }

        private string GetStatusName(long statusId) => statusId switch
        {
            1 => "Створений",
            2 => "Прийнятий",
            3 => "Готується",
            4 => "Пакується",
            5 => "Доставляється",
            6 => "Виконаний",
            _ => "Невідомий"
        };
        
    }
}