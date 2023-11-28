using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_153504_Pryhozhy.API.Services.PizzaService;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Domain.Models;

namespace WEB_153504_Pryhozhy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        private readonly IPizzaService _pizzaService;

        public PizzasController(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        [HttpGet]
        [HttpGet("{category}")]
        [HttpGet("page{pageNo:int}")]
        [HttpGet("{category}/page{pageNo:int}")]
        [Authorize]
        public async Task<ActionResult<ResponseData<ListModel<Pizza>>>> GetPizzas(string? category, int pageNo = 1, int pageSize = 3)
        {
            var response = await _pizzaService.GetPizzaListAsync(category, pageNo, pageSize);

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Pizza>> GetPizza(int id)
        {
            var response = await _pizzaService.GetByIdAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPizza(int id, Pizza pizza)
        {
            await _pizzaService.UpdateAsync(id, pizza);

            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Pizza>> PostPizza(Pizza pizza)
        {
            var response = await _pizzaService.CreateAsync(pizza);

            return Created($"/api/pizzas/{response?.Data?.Id}", response);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePizza(int id)
        {
            await _pizzaService.DeleteAsync(id);

            return NoContent();
        }

        [HttpPost("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseData<string>>> PostImage(int id, IFormFile formFile)
        {
            var response = await _pizzaService.SaveImageAsync(id, formFile);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
