using Microsoft.AspNetCore.Mvc;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Domain.Models;
using WEB_153504_Pryhozhy.Services.CategoryService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WEB_153504_Pryhozhy.Services.PizzaService
{
    public class MemoryPizzaService : IPizzaService
    {
        private List<Pizza> _pizzaList;
        private List<Category> _categories;
        private readonly int _pizzasPerPage;

        public MemoryPizzaService([FromServices] IConfiguration config,
                                   ICategoryService categoryService)
        {
            _categories = categoryService.GetCategoryListAsync()
                .Result
                .Data
                .Items;
            _pizzasPerPage = config.GetValue("PizzasPerPage", 3);
            SetUpData();
        }

        private void SetUpData()
        {
            _pizzaList = new List<Pizza>
            {
                new Pizza {
                    Id = 1,
                    Calories = 500,
                    Description = "Классная Пицца-дрицца",
                    Image = "Images/pizza_drizza.jpg",
                    Name = "пицца-дрицца",
                    CategoryId = 1
                },
                new Pizza {
                    Id = 2,
                    Calories = 231,
                    Description = "Низкокалорийная пицца для похудения",
                    Image = "Images/PPizza.jpg",
                    Name = "ППицца",
                    CategoryId = 2
                },
                new Pizza {
                    Id = 3,
                    Calories = 671,
                    Description = "Пицца с четырьмя видами сыра",
                    Image = "Images/four_cheese.jpg",
                    Name = "Четыре сыра",
                    CategoryId = 3
                },
                new Pizza {
                    Id = 4,
                    Calories = 671,
                    Description = "FFF",
                    Image = "Images/four_cheese.jpg",
                    Name = "FFFFF",
                    CategoryId = 3
                },
            };
        }

        public Task<ResponseData<Pizza>> CreateAsync(Pizza product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Pizza>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<ListModel<Pizza>>> GetPizzaListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var data = new ListModel<Pizza>();
            Category? category = _categories.FirstOrDefault(c => c.NormalizedName.Equals(categoryNormalizedName));
            var neededPizzas = _pizzaList
                 .Where(p => categoryNormalizedName == null || p.CategoryId == category?.Id)
                 .ToList();
            data.Items = neededPizzas
                 .Skip((pageNo - 1) * _pizzasPerPage)
                 .Take(_pizzasPerPage)
                 .ToList();
            data.TotalPages = ComputeTotalPages(neededPizzas.Count);
            data.CurrentPage = pageNo;

            var result = new ResponseData<ListModel<Pizza>> { Data = data };

            return Task.FromResult(result);
        }

        private int ComputeTotalPages(int all)
        {
            return (all + _pizzasPerPage - 1) / _pizzasPerPage;
        }

        public Task UpdateAsync(int id, Pizza product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
