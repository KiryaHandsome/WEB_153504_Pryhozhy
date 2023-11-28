using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Domain.Models;

namespace WEB_153504_Pryhozhy.BlazorWasm.Services
{
    public class DataService : IDataService
    {
        private HttpClient _httpClient;
        private string _apiUri;
        private int _pageSize;
        private JsonSerializerOptions _serializerOptions;
        private IAccessTokenProvider _accessTokenProvider;


        public DataService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider accessTokenProvider)
        {
            _httpClient = httpClient;
            _apiUri = configuration.GetValue<string>("ApiUri")!;
            _pageSize = configuration.GetValue<int>("ItemsPerPage");
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _accessTokenProvider = accessTokenProvider;
        }

        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Pizza> ObjectsList { get; set; } = new List<Pizza>();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = "";
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public event Action DataLoaded;

        public async Task GetCategoryListAsync()
        {
            var tokenRequest = await _accessTokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                var urlString = new StringBuilder($"{_apiUri}categories/");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);
                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        Categories = (await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions)).Data;
                        DataLoaded?.Invoke();
                    }
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                    }
                }
                else
                {
                    Success = false;
                    ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}";
                }
            }
        }

        public async Task<Pizza?> GetProductByIdAsync(int id)
        {
            var tokenRequest = await _accessTokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                var urlString = new StringBuilder($"{_apiUri}pizzas/{id}");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);
                var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.Content.ReadFromJsonAsync<ResponseData<Pizza>>(_serializerOptions);
                        DataLoaded?.Invoke();
                        return result.Data;
                    }
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                        return null;
                    }
                }
                Success = false;
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}";
                return null;
            }
            return null;
        }

        public async Task GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var tokenRequest = await _accessTokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                var urlString = new StringBuilder($"{_apiUri}pizzas/");

                if (categoryNormalizedName != null)
                {
                    urlString.Append($"{categoryNormalizedName}/");
                };
                if (pageNo > 1)
                {
                    urlString.Append($"page{pageNo}/");
                };
                if (!_pageSize.Equals(3))
                {
                    urlString.Append($"size{_pageSize}");
                }
                var url = urlString.ToString();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);
                var response = await _httpClient.GetAsync(new Uri(url));

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = (await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Pizza>>>(_serializerOptions)).Data!;
                        ObjectsList = result.Items;
                        TotalPages = result.TotalPages;
                        CurrentPage = result.CurrentPage;
                        DataLoaded?.Invoke();
                    }
                    catch (JsonException ex)
                    {
                        Success = false;
                        ErrorMessage = $"Ошибка: {ex.Message}";
                    }
                }
                else
                {

                    Success = false;
                    ErrorMessage = $"Данные не получены от сервера.";
                }
            }

        }

    }
}
