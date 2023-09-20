using WEB_153504_Klimkovich.Domain.Entities;
using WEB_153504_Klimkovich.Domain.Models;

namespace WEB_153504_Klimkovich.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
            {
                new Category {Id=1, Name="Смартфоны",
                    NormalizedName="smartphones"},
                 new Category {Id=2, Name="Наушники",
                     NormalizedName="headphones"},
                 new Category {Id=3, Name="Клавиатуры",
                     NormalizedName="keyboards"},
                 new Category {Id=3, Name="USB",
                     NormalizedName="usb"},
            };

            var result = new ResponseData<List<Category>>();
            result.Data = categories;
            return Task.FromResult(result);
        }
    }

}
