﻿using Microsoft.AspNetCore.Mvc;
using WEB_153504_Pryhozhy.API.Data;
using WEB_153504_Pryhozhy.API.Services.PizzaService;
using WEB_153504_Pryhozhy.Domain.Entities;

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
        public async Task<ActionResult<IEnumerable<Pizza>>> GetPizzas(string? category, int pageNo = 1, int pageSize = 3)
        {
            var response = await _pizzaService.GetPizzaListAsync(category, pageNo, pageSize);

            return Ok(response);
        }

        [HttpGet("{id:int}")]
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
        public async Task<IActionResult> PutPizza(int id, Pizza pizza)
        {
            await _pizzaService.UpdateAsync(id, pizza);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Pizza>> PostPizza(Pizza pizza)
        {
            var response = await _pizzaService.CreateAsync(pizza);

            return Created($"/api/pizzas/{response?.Data?.Id}", response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePizza(int id)
        {
            await _pizzaService.DeleteAsync(id);

            return NoContent();
        }
    }
}