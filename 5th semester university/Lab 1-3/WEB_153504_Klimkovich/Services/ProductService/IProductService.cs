using WEB_153504_Klimkovich.Domain.Entities;
using WEB_153504_Klimkovich.Domain.Models;

namespace WEB_153504_Klimkovich.Services.ProductService
{
    public interface IProductService
    {

        public Task<ResponseData<ListModel<Electronics>>> GetProductListAsync(string?
       categoryNormalizedName, int pageNo = 1);

        public Task<ResponseData<Electronics>> GetProductByIdAsync(int id);

        public Task UpdateProductAsync(int id, Electronics product, IFormFile? formFile);

        public Task DeleteProductAsync(int id);

        public Task<ResponseData<Electronics>> CreateProductAsync(Electronics product, IFormFile?
       formFile);
    }
}
