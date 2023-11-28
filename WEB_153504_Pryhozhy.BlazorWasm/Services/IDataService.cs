using WEB_153504_Pryhozhy.Domain.Entities;

namespace WEB_153504_Pryhozhy.BlazorWasm.Services
{
    public interface IDataService
    {
        event Action DataLoaded;

        List<Category> Categories { get; set; }

        List<Pizza> ObjectsList { get; set; }

        bool Success { get; set; }

        string ErrorMessage { get; set; }

        int TotalPages { get; set; }

        int CurrentPage { get; set; }

        public Task GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);

        public Task<Pizza> GetProductByIdAsync(int id);

        public Task GetCategoryListAsync();
    }
}
