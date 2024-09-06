using Bakery.Models.DbModels;
using Bakery.Models.DTO;

namespace Bakery.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProducts(long? productId, long? categoryId, bool? isAvailable);
        Task<bool> AddProduct(ProductDto productDto);
        Task<bool> UpdateProduct(long id, ProductDto productDto);
        Task<bool> DeleteProduct(long id);
        Task<List<Category>> GetAllCategories();
        Task<bool> AddCategory(ProductCategoryDto categoryDto);
        Task<bool> UpdateCategory(long id, ProductCategoryDto categoryDto);
        Task<bool> DeleteCategory(long id);
    }
}
