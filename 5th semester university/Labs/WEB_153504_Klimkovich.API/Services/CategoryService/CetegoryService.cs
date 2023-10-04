using Microsoft.EntityFrameworkCore;
using WEB_153504_Klimkovich.API.Data;
using WEB_153504_Klimkovich.Domain.Entities;
using WEB_153504_Klimkovich.Domain.Models;

namespace WEB_153504_Klimkovich.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var list = await _context.Categories.ToListAsync();
            return new ResponseData<List<Category>>
            {
                Data = list,
                Success = true,
            };
        }
    }

}
