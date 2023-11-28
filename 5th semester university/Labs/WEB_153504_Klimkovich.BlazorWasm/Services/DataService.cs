using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Http;
using WEB_153504_Klimkovich.Domain.Entities;
using WEB_153504_Klimkovich.Domain.Models;

namespace WEB_153504_Klimkovich.BlazorWasm.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly string _apiUri;
        private readonly ILogger<DataService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;
        private IAccessTokenProvider _accessTokenProvider;

        public DataService(HttpClient httpClient, IConfiguration configuration, ILogger<DataService> logger, IAccessTokenProvider accessTokenProvider)
        {
            _httpClient = httpClient;
            _apiUri = configuration.GetValue<String>("ApiUri")!;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _accessTokenProvider= accessTokenProvider;
        }
        public List<Category> Categories { get; set; }
        public List<Electronics> ObjectsList { get; set; }

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public async Task GetProductListAsync(string? category, int pageNo = 1)
        {
            if (!(await SetBearerTokenHeader())) return;
            var urlString = new StringBuilder($"{_apiUri}Electronics/");

            if (category != null)
            {
                urlString.Append($"{category}/");
            };

            if (pageNo > 1)
            {
                urlString.Append($"page{pageNo}");
            };

            if (!_pageSize.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", _pageSize));
            }
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var items = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Electronics>>>(_serializerOptions);
                    Success = true;
                    ObjectsList = items.Data.Items;
                    TotalPages = items.Data.TotalPages;
                    CurrentPage = items.Data.CurrentPage;
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    ErrorMessage = ex.Message;
                    Success = false;
                }
            }
            else
            {
                _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
                ErrorMessage = $"Error:{response.StatusCode}";
                Success = false;
            }
        }
        public async Task<Electronics> GetProductByIdAsync(int id)
        {
            if (!await SetBearerTokenHeader()) return null;
            var urlString = new StringBuilder($"{_apiUri}Electronics/{id}");
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var item = await response.Content.ReadFromJsonAsync<ResponseData<Electronics>>(_serializerOptions);
                    Success = true;
                    return item.Data;
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    ErrorMessage = ex.Message;
                    Success = false;
                }
            }
            else
            {
                _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
                ErrorMessage = $"Error:{response.StatusCode}";
                Success = false;
            }
            return null;
        }
        public async Task GetCategoryListAsync()
        {
            if (!await SetBearerTokenHeader()) return;
            var urlString = new StringBuilder($"{_apiUri}Categories/");

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var items = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions);
                    Success = true;
                    Categories = items.Data;
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    ErrorMessage = ex.Message;
                    Success = false;
                }
            }
            else
            {
                _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
                ErrorMessage = $"Error:{response.StatusCode}";
                Success = false;
            }
        }
        public async Task<bool> SetBearerTokenHeader()
        {
            var tokenRequest = await _accessTokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);
                return true;
            }
            else
            {
                _logger.LogError($"-----> Не получен токен");
                return false;
            }
        }
    }
}
