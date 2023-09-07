using WEB_153504_Pryhozhy.API.Data;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Domain.Models;

namespace WEB_153504_Pryhozhy.API.Services.PizzaService
{
    public class PizzaService : IPizzaService
    {
        private readonly AppDbContext _context;

        public PizzaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(int id)
        {
            Pizza? pizza = await _context.Pizzas.FindAsync(id);
            if (pizza != null)
            {
                _context.Pizzas.Remove(pizza);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ResponseData<Pizza>> GetByIdAsync(int id)
        {
            Pizza? pizza = await _context.Pizzas.FindAsync(id);
            if (pizza == null)
            {
                return new ResponseData<Pizza>()
                {
                    Success = false,
                    ErrorMessage = $"Pizza with id={id} not found"
                };
            }
            return new ResponseData<Pizza>()
            {
                Data = pizza
            };
        }

        public Task<ResponseData<ListModel<Pizza>>> GetPizzaListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            var data = new ListModel<Pizza>();
            Category? category = _context.Categories.FirstOrDefault(c => c.NormalizedName.Equals(categoryNormalizedName));
            var neededPizzas = _context.Pizzas
                 .Where(p => categoryNormalizedName == null || p.CategoryId == category.Id)
                 .ToList();
            data.Items = neededPizzas
                 .Skip((pageNo - 1) * pageSize)
                 .Take(pageSize)
                 .ToList();
            data.TotalPages = ComputeTotalPages(neededPizzas.Count, pageSize);
            data.CurrentPage = pageNo;

            return Task.FromResult(
                new ResponseData<ListModel<Pizza>>()
                {
                    Data = data
                }
            );
        }

        private static int ComputeTotalPages(int all, int pageSize)
        {
            return (all + pageSize - 1) / pageSize;
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(int id, Pizza pizza)
        {
            Pizza? productToUpdate = await _context.Pizzas.FindAsync(id);
            if (productToUpdate != null)
            {
                productToUpdate.Description = pizza.Description;
                productToUpdate.CategoryId = pizza.CategoryId;
                productToUpdate.Calories = pizza.Calories;
                if (pizza.Image != null)
                    productToUpdate.Image = pizza.Image;
                _context.Update(productToUpdate);
                await _context.SaveChangesAsync();
            }
        }

        public Task<ResponseData<Pizza>> CreateAsync(Pizza pizza)
        {
            throw new NotImplementedException();
        }
    }
}
