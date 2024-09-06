namespace Bakery.Models.DTO
{
    public class ApiResult
    {
        /// <summary>
        /// Успішність операції
        /// </summary>
        public bool status { get; set; }
        /// <summary>
        /// Повідомлення з помилкою
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// Об'єкт з даними
        /// </summary>
        public object data { get; set; }

        public ApiResult() { }

        public ApiResult(bool status)
        {
            this.status = status;
        }

        public ApiResult(string message)
        {
            this.status = false;
            this.message = message;
        }

        public ApiResult(bool status, object data)
        {
            this.status = status;
            this.data = data;
        }
    }
}
