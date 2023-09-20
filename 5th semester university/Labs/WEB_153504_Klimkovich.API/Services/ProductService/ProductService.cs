using Microsoft.EntityFrameworkCore;
using WEB_153504_Klimkovich.API.Data;
using WEB_153504_Klimkovich.Domain.Entities;
using WEB_153504_Klimkovich.Domain.Models;

namespace WEB_153504_Klimkovich.API.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly int _maxPageSize = 20;
        private ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseData<ListModel<Electronics>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _context.Electronics
                .AsQueryable()
                .Where(d => categoryNormalizedName == null || d.Category.NormalizedName
                .Equals(categoryNormalizedName));

            int totalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);
            if (pageNo > totalPages)
                return new ResponseData<ListModel<Electronics>>
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "No such page"
                };

            var result = new ResponseData<ListModel<Electronics>>()
            {
                Data = new()
                {
                    Items = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync(),
                    CurrentPage = pageNo,
                    TotalPages = totalPages,
                }
            };
            return result;
        }

        public async Task<ResponseData<Electronics>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<Electronics>> CreateProductAsync(Electronics product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProductAsync(int id, Electronics product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }
    }
}
