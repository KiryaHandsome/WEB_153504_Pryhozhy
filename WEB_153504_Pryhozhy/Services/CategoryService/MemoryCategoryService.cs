using System.Diagnostics;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Domain.Models;

namespace WEB_153504_Pryhozhy.Services.CategoryService
{
    public class MemoryCategoryService : ICategoryService
    {
        private List<Category> _categories = new List<Category> {
                new Category{Id = 1, Name = "Мясная", NormalizedName = "meat"},
                new Category{Id = 2, Name = "Вегетарианская", NormalizedName = "vegan"},
                new Category{Id = 3, Name = "Сырная", NormalizedName = "cheesy" },
            };

        public Task<ResponseData<ListModel<Category>>> GetCategoryListAsync()
        {
            var result = new ResponseData<ListModel<Category>>();
            result.Data = new ListModel<Category> { Items = _categories };

            return Task.FromResult(result);
        }
    }
}
