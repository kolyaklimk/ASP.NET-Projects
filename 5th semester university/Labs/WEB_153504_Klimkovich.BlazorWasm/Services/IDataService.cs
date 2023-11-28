using WEB_153504_Klimkovich.Domain.Entities;

namespace WEB_153504_Klimkovich.BlazorWasm.Services
{
    public interface IDataService
    {
        List<Category> Categories { get; set; }
        List<Electronics> ObjectsList { get; set; }

        bool Success { get; set; }
        string ErrorMessage { get; set; }
        int TotalPages { get; set; }
        int CurrentPage { get; set; }

        public Task GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);
        public Task<Electronics> GetProductByIdAsync(int id);
        public Task GetCategoryListAsync();
    }
}
