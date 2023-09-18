using Microsoft.AspNetCore.Mvc;
using WEB_153504_Klimkovich.Domain.Entities;
using WEB_153504_Klimkovich.Domain.Models;
using WEB_153504_Klimkovich.Services.CategoryService;

namespace WEB_153504_Klimkovich.Services.ProductService
{
    public class MemoryProductService : IProductService
    {
        List<Electronics> _electronics;
        List<Category> _categories;

        public MemoryProductService([FromServices] IConfiguration config, ICategoryService categoryService, int pageNo)
        {
            _categories = categoryService.GetCategoryListAsync().Result.Data;
            SetupData();
        }

        private void SetupData()
        {
            _electronics = new List<Electronics>
            {
                new Electronics { Id = 1, Name="Redmi Note 5",
                    Description="Snapdragon 636",
                    Price = 199, Image="Images/redmiNote5.jpg",
                    Category= _categories.Find(c=>c.NormalizedName.Equals("smartphones"))},

                new Electronics { Id = 2, Name="Huawei P Smart 2019",
                    Description="Kirin 710",
                    Price = 149, Image="Images/huaweiPSmart.jpg",
                    Category= _categories.Find(c=>c.NormalizedName.Equals("smartphones"))},

                new Electronics { Id = 2, Name="Huawei 2020",
                    Description="Kirin 810",
                    Price = 230, Image="Images/huawei2020.jpg",
                    Category= _categories.Find(c=>c.NormalizedName.Equals("smartphones"))},

                new Electronics { Id = 2, Name="Huawei 2017",
                    Description="Kirin 510",
                    Price = 82, Image="Images/huawei2017.jpg",
                    Category= _categories.Find(c=>c.NormalizedName.Equals("smartphones"))},

                new Electronics { Id = 2, Name="Redmi Note 8 Pro",
                    Description="Snapdragon 712",
                    Price = 222, Image="Images/redmiNote8pro.jpg",
                    Category= _categories.Find(c=>c.NormalizedName.Equals("smartphones"))},

                new Electronics { Id = 2, Name="Beats",
                    Description="White, big",
                    Price = 123, Image="Images/beats.jpg",
                    Category= _categories.Find(c=>c.NormalizedName.Equals("headphones"))},

                new Electronics { Id = 2, Name="Sony xk840",
                    Description="Blue",
                    Price = 235, Image="Images/sonyxk840.jpg",
                    Category= _categories.Find(c=>c.NormalizedName.Equals("headphones"))},

                new Electronics { Id = 2, Name="AquaKey",
                    Description="Red",
                    Price = 53, Image="Images/aquakey.jpg",
                    Category= _categories.Find(c=>c.NormalizedName.Equals("keyboards"))},

                new Electronics { Id = 2, Name="RGB Serial 2820",
                    Description="RGB",
                    Price = 75, Image="Images/rgbserial2820.jpg",
                    Category= _categories.Find(c=>c.NormalizedName.Equals("keyboards"))},

                new Electronics { Id = 2, Name="GrassKey",
                    Description="Green",
                    Price = 111, Image="Images/grasskey.jpg",
                    Category= _categories.Find(c=>c.NormalizedName.Equals("keyboards"))},
            };
        }

        public Task<ResponseData<ListModel<Electronics>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var result = new ResponseData<ListModel<Electronics>>()
            {
                Data = new()
                {
                    Items = _electronics.Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName)).ToList(),
                    CurrentPage = pageNo,
                    TotalPages = pageNo,
                }
            };
            return Task.FromResult(result);
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
    }
}
