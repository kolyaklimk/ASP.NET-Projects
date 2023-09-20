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

        public Task<ResponseData<ListModel<Electronics>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _context.Electronics.AsQueryable();
            var dataList = new ListModel<Electronics>();
            query = query.Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName));
            // количество элементов в списке
            var count = query.Count();
            if (count == 0)
            {
                return Task.FromResult(new ResponseData<ListModel<Electronics>>
                {
                    Data = dataList
                });
            }
            // количество страниц
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
                return Task.FromResult(new ResponseData<ListModel<Electronics>>
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "No such page"
                });
            dataList.Items = query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;
            var response = new ResponseData<ListModel<Electronics>>
            {
                Data = dataList
            };
            return Task.FromResult(response);
        }

        public Task<ResponseData<Electronics>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Electronics>> CreateProductAsync(Electronics product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(int id, Electronics product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }
    }
}
