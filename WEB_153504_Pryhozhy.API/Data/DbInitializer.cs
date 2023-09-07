using Microsoft.EntityFrameworkCore;
using WEB_153504_Pryhozhy.Domain.Entities;

namespace WEB_153504_Pryhozhy.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var imageUrl = app.Configuration.GetSection("ImageSettings:ImageUrl").Get<string>();
            // Ensure that the database is created and apply pending migrations
            await dbContext.Database.MigrateAsync(); 

            // Check if there are any records in the database
            if (!dbContext.Pizzas.Any())
            {
                // Sample data to be seeded
                var pizzas = new List<Pizza>()
                {
                    new Pizza {
                        Calories = 500,
                        Description = "Классная Пицца-дрицца",
                        Image = imageUrl + "pizza_drizza.jpg",
                        Name = "пицца-дрицца",
                        CategoryId = 1
                    },
                    new Pizza {
                        Calories = 231,
                        Description = "Низкокалорийная пицца для похудения",
                        Image = imageUrl + "PPizza.jpg",
                        Name = "ППицца",
                        CategoryId = 2
                    },
                    new Pizza {
                        Calories = 671,
                        Description = "Пицца с четырьмя видами сыра",
                        Image = imageUrl + "four_cheese.jpg",
                        Name = "Четыре сыра",
                        CategoryId = 3
                    },
                    new Pizza {
                        Calories = 671,
                        Description = "FFF",
                        Image = imageUrl + "four_cheese.jpg",
                        Name = "FFFFF",
                        CategoryId = 3
                    },
                };

                dbContext.Pizzas.AddRange(pizzas);

                await dbContext.SaveChangesAsync();
            }


            if (!dbContext.Categories.Any())
            {
                var categories = new Category[]
                {
                    new Category{Name = "Мясная", NormalizedName = "meat"},
                    new Category{Name = "Вегетарианская", NormalizedName = "vegan"},
                    new Category{Name = "Сырная", NormalizedName = "cheesy" }
                };

                dbContext.Categories.AddRange(categories);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
