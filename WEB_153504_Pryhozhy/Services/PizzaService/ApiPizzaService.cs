using System.Net.Http;
using System.Text;
using System.Text.Json;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Domain.Models;

namespace WEB_153504_Pryhozhy.Services.PizzaService
{
    public class ApiPizzaService : IPizzaService
    {
        private HttpClient _httpClient;
        private string? _pageSize;
        private JsonSerializerOptions _serializerOptions;
        private ILogger<ApiPizzaService> _logger;

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
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Dishes");
            var response = await _httpClient.PostAsJsonAsync(uri, pizza, _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content
                    .ReadFromJsonAsync<ResponseData<Pizza>>(_serializerOptions);
                return data; // dish;
            }
            _logger.LogError($"-----> object not created. Error:{response.StatusCode.ToString()}");
            return new ResponseData<Pizza>
            {
                Success = false,
                ErrorMessage = $"Объект не добавлен. Error:{ response.StatusCode }"
            };
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Pizza>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
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

        public Task UpdateAsync(int id, Pizza pizza, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
