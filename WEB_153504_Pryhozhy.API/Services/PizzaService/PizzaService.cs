using System.Configuration;
using WEB_153504_Pryhozhy.API.Controllers;
using WEB_153504_Pryhozhy.API.Data;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Domain.Models;

namespace WEB_153504_Pryhozhy.API.Services.PizzaService
{
    public class PizzaService : IPizzaService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _imagesPath;

        public PizzaService(AppDbContext context,
                            IHttpContextAccessor httpContextAccessor,
                            IWebHostEnvironment env)
        {
            _imagesPath = Path.Combine(env.WebRootPath, "Images");
            _context = context;
            _httpContextAccessor = httpContextAccessor;
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
            var responseData = new ResponseData<string>();
            var product = await _context.Pizzas.FindAsync(id);
            if (product == null)
            {
                responseData.Success = false;
                responseData.ErrorMessage = "Нет такого товара";
                return responseData;
            }
            var host = "https://" + _httpContextAccessor.HttpContext.Request.Host;
            if (formFile != null)
            {
                // Удалить предыдущее изображение
                if (!string.IsNullOrEmpty(product.Image))
                {
                    var prevImage = Path.GetFileName(product.Image);
                    var prevImagePath = Path.Combine(_imagesPath, prevImage);

                    if (File.Exists(prevImagePath))
                    {
                        File.Delete(prevImagePath);
                    }
                }
                // Создать имя файла
                var ext = Path.GetExtension(formFile.FileName);
                var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
                // Сохранить файл
                using (var fileStream = new FileStream($"{_imagesPath}/{fName}", FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }
                // Указать имя файла в объекте
                product.Image = $"{host}/Images/{fName}";
                await _context.SaveChangesAsync();
            }
            responseData.Data = product.Image;
            return responseData;
        }

        public async Task UpdateAsync(int id, Pizza pizza)
        {
            Pizza? productToUpdate = await _context.Pizzas.FindAsync(id);
            if (productToUpdate != null)
            {
                productToUpdate.Name = pizza.Name;
                productToUpdate.Description = pizza.Description;
                productToUpdate.CategoryId = pizza.CategoryId;
                productToUpdate.Calories = pizza.Calories;
                if (pizza.Image != null)
                    productToUpdate.Image = pizza.Image;
                productToUpdate.Price = pizza.Price;
                _context.Update(productToUpdate);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ResponseData<Pizza>> CreateAsync(Pizza product)
        {
            _context.Pizzas.Add(product);
            await _context.SaveChangesAsync();
            return new ResponseData<Pizza>
            {
                Data = product,
                Success = true
            };
        }
    }
}
