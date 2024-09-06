using Bakery.Interfaces;
using Bakery.Models.DbModels;
using Bakery.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Models.Services
{
    public class ProductService: IProductService
    {
        private readonly BakeryContext _context;

        public ProductService(BakeryContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> GetProducts(long? productId, long? categoryId, bool? isAvailable)
        {
            var query = from product in _context.Products
                        join category in _context.Categories
                        on product.CategoryId equals category.Id
                        select new ProductDto
                        {
                            Id = product.Id,
                            Name = product.Name,
                            CategoryId = category.Id,
                            CategoryName = category.Name,
                            Description = product.Description,
                            IsAvailable = product.IsAvailable,
                            Price = product.Price
                        };

            if (productId.HasValue)
            {
                query = query.Where(p => p.Id == productId);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            if (isAvailable.HasValue)
            {
                query = query.Where(p => p.IsAvailable == isAvailable);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> AddProduct(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                CategoryId = productDto.CategoryId,
                IsAvailable = productDto.IsAvailable,
                CreationDate = DateTime.Now
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateProduct(long id, ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
                throw new Exception("Продукт не знайдений");

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.CategoryId = productDto.CategoryId;
            product.IsAvailable = productDto.IsAvailable;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is not null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            var result = await _context.Categories.ToListAsync();
            return result;
        }

        public async Task<bool> AddCategory(ProductCategoryDto categoryDto)
        {
            var category = new Category { Name = categoryDto.Name };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCategory(long id, ProductCategoryDto categoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is null)
                throw new Exception("Категорія не знайдена");

            category.Name = categoryDto.Name;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCategory(long id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is not null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }

            return true;
        }
    }
}
