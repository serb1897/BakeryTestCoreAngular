using Bakery.Interfaces;
using Bakery.Models.DbModels;
using Bakery.Models.DTO;
using Bakery.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Отримання всіх продуктів
        /// </summary>
        [HttpGet]
        public async Task<JsonResult> GetProducts([FromQuery] long? productId, [FromQuery] long? categoryId, [FromQuery] bool? isAvailable)
        {
            try
            {
                var products = await _productService.GetProducts(productId, categoryId, isAvailable);
                return new JsonResult(new ApiResult(true, products));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return new JsonResult(new ApiResult(ex.Message));
            }
        }

        /// <summary>
        /// Додавання нового продукту
        /// </summary>
        [HttpPost]
        public async Task<JsonResult> AddProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var result = await _productService.AddProduct(productDto);
                return new JsonResult(new ApiResult(result));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return new JsonResult(new ApiResult(ex.Message));
            }
        }

        /// <summary>
        /// Видалення продукту
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<JsonResult> DeleteProduct([FromQuery] long id)
        {
            try
            {
                var result = await _productService.DeleteProduct(id);
                return new JsonResult(new ApiResult(result));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return new JsonResult(new ApiResult(ex.Message));
            }
        }

        /// <summary>
        /// Редагування продукту
        /// </summary>
        [HttpPut("{id}")]
        public async Task<JsonResult> UpdateProduct([FromQuery] long id, [FromBody] ProductDto productDto)
        {
            try
            {
                var result = await _productService.UpdateProduct(id, productDto);
                return new JsonResult(new ApiResult(result));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return new JsonResult(new ApiResult(ex.Message));
            }
        }

        /// <summary>
        /// Отримання всіх категорій
        /// </summary>
        [HttpGet("categories")]
        public async Task<JsonResult> GetAllCategories()
        {
            try
            {
                var categories = await _productService.GetAllCategories();
                return new JsonResult(new ApiResult(true, categories));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return new JsonResult(new ApiResult(ex.Message));
            }
        }

        /// <summary>
        /// Редагування категорії
        /// </summary>
        [HttpPut("categories/{id}")]
        public async Task<JsonResult> UpdateCategory([FromQuery] long id, [FromBody] ProductCategoryDto categoryDto)
        {
            try
            {
                var result = await _productService.UpdateCategory(id, categoryDto);
                return new JsonResult(new ApiResult(result));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return new JsonResult(new ApiResult(ex.Message));
            }
        }

        /// <summary>
        /// Додавання нової категорії
        /// </summary>
        [HttpPost("categories")]
        public async Task<JsonResult> AddCategory([FromBody] ProductCategoryDto categoryDto)
        {
            try
            {
                var result = await _productService.AddCategory(categoryDto);
                return new JsonResult(new ApiResult(result));
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                return new JsonResult(new ApiResult(ex.Message));
            }
        }

        /// <summary>
        /// Видалення категорії
        /// </summary>
        [HttpDelete("categories/{id}")]
        public async Task<JsonResult> DeleteCategory([FromQuery] long id)
        {
            try
            {
                var result = await _productService.DeleteCategory(id);
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
