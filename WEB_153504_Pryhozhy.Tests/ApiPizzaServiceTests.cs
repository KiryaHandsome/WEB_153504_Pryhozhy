using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153504_Pryhozhy.API.Data;
using WEB_153504_Pryhozhy.API.Services.PizzaService;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Domain.Models;

namespace WEB_153504_Pryhozhy.Tests
{
    public class ApiPizzaServiceTests
    {

        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _contextOptions;

        public ApiPizzaServiceTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema and seed some data
            using var context = new AppDbContext(_contextOptions);

            if (context.Database.EnsureCreated())
            {
                using var viewCommand = context.Database.GetDbConnection().CreateCommand();
                viewCommand.CommandText = @"
                CREATE VIEW AllResources AS
                SELECT Url
                FROM Blogs;";
                viewCommand.ExecuteNonQuery();
            }
            context.Categories.AddRange(
                new Category { Name = "Category 1", NormalizedName = "category1" },
                new Category { Name = "Category 2", NormalizedName = "category2" },
                new Category { Name = "Category 3", NormalizedName = "category3" }
            );

            context.Pizzas.AddRange(
                new Pizza { Name = "Pizza 1", Description = "Description 1", CategoryId = 1, Price = 10 },
                new Pizza { Name = "Pizza 2", Description = "Description 2", CategoryId = 1, Price = 20 },
                new Pizza { Name = "Pizza 3", Description = "Description 3", CategoryId = 2, Price = 30 },
                new Pizza { Name = "Pizza 4", Description = "Description 4", CategoryId = 2, Price = 40 },
                new Pizza { Name = "Pizza 5", Description = "Description 5", CategoryId = 3, Price = 50 },
                new Pizza { Name = "Pizza 6", Description = "Description 6", CategoryId = 3, Price = 60 },
                new Pizza { Name = "Pizza 6", Description = "Description 7", CategoryId = 3, Price = 60 },
                new Pizza { Name = "Pizza 6", Description = "Description 8", CategoryId = 3, Price = 60 },
                new Pizza { Name = "Pizza 6", Description = "Description 9", CategoryId = 3, Price = 60 },
                new Pizza { Name = "Pizza 6", Description = "Description 10", CategoryId = 3, Price = 60 },
                new Pizza { Name = "Pizza 6", Description = "Description 11", CategoryId = 3, Price = 60 },
                new Pizza { Name = "Pizza 6", Description = "Description 12", CategoryId = 3, Price = 60 },
                new Pizza { Name = "Pizza 6", Description = "Description 13", CategoryId = 3, Price = 60 },
                new Pizza { Name = "Pizza 6", Description = "Description 14", CategoryId = 3, Price = 60 },
                new Pizza { Name = "Pizza 6", Description = "Description 15", CategoryId = 3, Price = 60 },
                new Pizza { Name = "Pizza 6", Description = "Description 16", CategoryId = 3, Price = 60 },
                new Pizza { Name = "Pizza 6", Description = "Description 17", CategoryId = 3, Price = 60 },
                new Pizza { Name = "Pizza 6", Description = "Description 18", CategoryId = 3, Price = 60 },
                new Pizza { Name = "Pizza 6", Description = "Description 19", CategoryId = 3, Price = 60 },
                new Pizza { Name = "Pizza 6", Description = "Description 20", CategoryId = 3, Price = 60 },
                new Pizza { Name = "Pizza 6", Description = "Description 21", CategoryId = 3, Price = 60 }
            );
            context.SaveChanges();
        }

        AppDbContext CreateContext() => new AppDbContext(_contextOptions);

        public void Dispose() => _connection.Dispose();

        [Fact]
        public void ServiceReturnsFirstPageOfThreeItems()
        {
            using var context = CreateContext();
            var service = new PizzaService(context, null, null);
            var result = service.GetPizzaListAsync(null).Result;
            Assert.IsType<ResponseData<ListModel<Pizza>>>(result);
            Assert.True(result.Success);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(7, result.Data.TotalPages);
            Assert.Equal(context.Pizzas.AsEnumerable().ElementAt(0), result.Data.Items[0]);
            Assert.Equal(context.Pizzas.AsEnumerable().ElementAt(1), result.Data.Items[1]);
            Assert.Equal(context.Pizzas.AsEnumerable().ElementAt(2), result.Data.Items[2]);
        }

        [Fact]
        public void ServiceReturnsRightPageOfThreeItems()
        {
            using var context = CreateContext();
            var service = new PizzaService(context, null, null);
            var result = service.GetPizzaListAsync(null, 2).Result;
            Assert.IsType<ResponseData<ListModel<Pizza>>>(result);
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(7, result.Data.TotalPages);
            Assert.Equal(context.Pizzas.AsEnumerable().ElementAt(3), result.Data.Items[0]);
            Assert.Equal(context.Pizzas.AsEnumerable().ElementAt(4), result.Data.Items[1]);
            Assert.Equal(context.Pizzas.AsEnumerable().ElementAt(5), result.Data.Items[2]);
        }

        [Fact]
        public void ServiceFiltersByCategory()
        {
            using var context = CreateContext();
            var service = new PizzaService(context, null, null);
            var result = service.GetPizzaListAsync("category1").Result;
            Assert.IsType<ResponseData<ListModel<Pizza>>>(result);
            Assert.True(result.Success);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(2, result.Data.Items.Count);
            Assert.Equal(1, result.Data.TotalPages);
            Assert.Equal(context.Pizzas.AsEnumerable().ElementAt(0), result.Data.Items[0]);
            Assert.Equal(context.Pizzas.AsEnumerable().ElementAt(1), result.Data.Items[1]);
        }

        [Fact]
        public void ServiceSetsMaxPageSize()
        {
            using var context = CreateContext();
            var service = new PizzaService(context, null, null);
            var result = service.GetPizzaListAsync(null, 1, 23).Result;
            Assert.IsType<ResponseData<ListModel<Pizza>>>(result);
            Assert.True(result.Success);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(20, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages);
        }

        [Fact]
        public void ServiceReturnsErrorIfPageNoIsIncorrect()
        {
            using var context = CreateContext();
            var service = new PizzaService(context, null, null);
            var result = service.GetPizzaListAsync(null, 4, 23).Result;
            Assert.IsType<ResponseData<ListModel<Pizza>>>(result);
            Assert.False(result.Success);
        }
    }
}
