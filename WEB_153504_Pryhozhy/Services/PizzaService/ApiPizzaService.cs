using System.Data.SqlTypes;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Domain.Models;

namespace WEB_153504_Pryhozhy.Services.PizzaService
{
    public class ApiPizzaService : IPizzaService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiPizzaService> _logger;

        public ApiPizzaService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiPizzaService> logger)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public async Task<ResponseData<Pizza>> CreateAsync(Pizza pizza, IFormFile? formFile)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}pizzas/");
            var response = await _httpClient.PostAsJsonAsync(urlString.ToString(), pizza, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                var pizzaResponse = await response.Content.ReadFromJsonAsync<ResponseData<Pizza>>(_serializerOptions);
                if (formFile != null)
                {
                    await SaveImageAsync(pizzaResponse.Data.Id, formFile);
                }

                return new ResponseData<Pizza>
                {
                    Data = pizzaResponse.Data,
                    Success = true
                };
            }
            return new ResponseData<Pizza>
            {
                Success = false,
                ErrorMessage = "Ошибка при создании продукта"
            };
        }
        public async Task DeleteAsync(int id)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}pizzas/{id}");
            await _httpClient.DeleteAsync(urlString.ToString());
        }

        public async Task<ResponseData<Pizza>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(new Uri($"{_httpClient.BaseAddress.AbsoluteUri}pizzas/{id}"));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<Pizza>>();
            }

            return new ResponseData<Pizza>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
            };
        }

        public async Task<ResponseData<ListModel<Pizza>>> GetPizzaListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}pizzas/");

            if (categoryNormalizedName != null)
            {
                urlString.Append($"{categoryNormalizedName}/");
            }
            if (pageNo > 1)
            {
                urlString.Append($"page{pageNo}");
            }
            if (!_pageSize.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", _pageSize));
            }
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content
                        .ReadFromJsonAsync<ResponseData<ListModel<Pizza>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<ListModel<Pizza>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
            return new ResponseData<ListModel<Pizza>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
            };
        }

        public async Task UpdateAsync(int id, Pizza pizza, IFormFile? formFile)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}pizzas/{id}");
            await _httpClient.PutAsJsonAsync(urlString.ToString(), pizza, _serializerOptions);

            if (formFile != null)
            {
                await SaveImageAsync(id, formFile);
            }
        }

        private async Task SaveImageAsync(int id, IFormFile image)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}pizzas/{id}")
            };
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(image.OpenReadStream());
            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;
            await _httpClient.SendAsync(request);
        }
    }
}
