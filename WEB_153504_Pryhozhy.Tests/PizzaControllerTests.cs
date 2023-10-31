using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WEB_153504_Pryhozhy.Controllers;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Domain.Models;
using WEB_153504_Pryhozhy.Services.CategoryService;
using WEB_153504_Pryhozhy.Services.PizzaService;

namespace WEB_153504_Pryhozhy.Tests
{
    public class PizzaControllerTests
    {

        [Fact]
        public async Task Index_ReturnsNotFound_WhenCategoriesNotReceived()
        {
            var categoryServiceMock = new Mock<ICategoryService>();

            categoryServiceMock
                .Setup(s => s.GetCategoryListAsync())
                .ReturnsAsync(new ResponseData<ListModel<Category>> { Success = false, ErrorMessage = "Ошибка при получении списка категорий" });

            var productServiceMock = new Mock<IPizzaService>();
            productServiceMock
                .Setup(s => s.GetPizzaListAsync(null, 1))
                .ReturnsAsync(new ResponseData<ListModel<Pizza>> { Success = true });

            var controller = new PizzaController(categoryServiceMock.Object, productServiceMock.Object);

            var result = await controller.Index(null, 1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Index_ReturnsNotFound_WhenProductsNotReceived()
        {
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock
               .Setup(s => s.GetCategoryListAsync())
               .ReturnsAsync(new ResponseData<ListModel<Category>> { Success = true, Data = new ListModel<Category>() });

            var productServiceMock = new Mock<IPizzaService>();
            productServiceMock
                .Setup(s => s.GetPizzaListAsync(null, 1))
                .ReturnsAsync(new ResponseData<ListModel<Pizza>> { Success = false, ErrorMessage = "Ошибка при получении списка продуктов" });


            var controller = new PizzaController(categoryServiceMock.Object, productServiceMock.Object);

            var result = await controller.Index(null, 1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Index_CorrectAction()
        {

            var expectedCategories = new List<Category>()
            {
                new Category { Id = 1, Name = "Name", NormalizedName = "name" }
            };

            var expectedCurrentCategory = "Все";

            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock
               .Setup(s => s.GetCategoryListAsync())
               .ReturnsAsync(new ResponseData<ListModel<Category>> { Data = new ListModel<Category> { Items = expectedCategories } });

            var productServiceMock = new Mock<IPizzaService>();
            productServiceMock.Setup(s => s.GetPizzaListAsync(null, 1))
                .ReturnsAsync(new ResponseData<ListModel<Pizza>> { 
                    Data = new ListModel<Pizza>() {
                        Items = new List<Pizza>() {
                            new Pizza { Id = 1, Name = "Product", Description = "Description", Price = 10 } 
                        } 
                    } 
                });

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.Request.Headers)
                .Returns(new HeaderDictionary { { "X-Requested-With", "XMLHttpRequest" } });

            var controllerContext = new ControllerContext
            {
                HttpContext = httpContextMock.Object
            };


            var controller = new PizzaController(categoryServiceMock.Object, productServiceMock.Object)
            {
                ControllerContext = controllerContext
            };

            var result = await controller.Index(null, 1);

            Assert.True(
                expectedCategories.SequenceEqual(controller.ViewData["categories"] as List<Category>)
            );
            Assert.Equal(expectedCurrentCategory, controller.ViewData["currentCategory"]);
            Assert.IsType<ListModel<Pizza>>(controller.ViewData.Model);
        }

    }
}
