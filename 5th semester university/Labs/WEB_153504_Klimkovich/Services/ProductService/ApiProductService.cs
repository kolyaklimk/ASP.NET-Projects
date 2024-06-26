﻿using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WEB_153504_Klimkovich.Domain.Entities;
using WEB_153504_Klimkovich.Domain.Models;

namespace WEB_153504_Klimkovich.Services.ProductService
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly ILogger<ApiProductService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly HttpContext _httpContext;

        public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext;
        }

        private async Task SaveImageAsync(int id, IFormFile image)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Electronics/{id}")
            };

            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(image.OpenReadStream());
            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;

            await SetBearerTokenHeader(request);
            await _httpClient.SendAsync(request);
        }

        public async Task<ResponseData<ListModel<Electronics>>> GetProductListAsync(string? category, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Electronics/");

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

            await SetBearerTokenHeader();
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Electronics>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<ListModel<Electronics>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
            return new ResponseData<ListModel<Electronics>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
            };
        }

        public async Task<ResponseData<Electronics>> CreateProductAsync(Electronics product, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Electronics");
            await SetBearerTokenHeader();
            var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Electronics>>(_serializerOptions);
                if (formFile != null)
                {
                    await SaveImageAsync(data.Data.Id, formFile);
                }
                return data;
            }

            _logger.LogError($"-----> object not created. Error:{response.StatusCode}");
            return new ResponseData<Electronics>
            {
                Success = false,
                ErrorMessage = $"Объект не добавлен. Error:{response.StatusCode}"
            };
        }

        public async Task DeleteProductAsync(int id)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Electronics/{id}");
            await SetBearerTokenHeader();
            var response = await _httpClient.DeleteAsync(new Uri(urlString.ToString()));
        }

        public async Task<ResponseData<Electronics>> GetProductByIdAsync(int id)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Electronics/{id}");
            await SetBearerTokenHeader();
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Electronics>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<Electronics>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
            return new ResponseData<Electronics>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
            };
        }

        public async Task<ResponseData<Electronics>> UpdateProductAsync(int id, Electronics product, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"Electronics/{id}");
            await SetBearerTokenHeader();
            var response = await _httpClient.PutAsJsonAsync(uri, product, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Electronics>>(_serializerOptions);
                if (formFile != null)
                {
                    await SaveImageAsync(data.Data.Id, formFile);
                }
                return data;
            }

            _logger.LogError($"-----> object not updated. Error:{response.StatusCode}");
            return new ResponseData<Electronics>
            {
                Success = false,
                ErrorMessage = $"Объект не обновлен. Error:{response.StatusCode}"
            };
        }
        public async Task SetBearerTokenHeader(HttpRequestMessage request = null)
        {
            if (request != null)
            {
                var token = await _httpContext.GetTokenAsync("access_token");
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
            }
            else
            {
                var token = await _httpContext.GetTokenAsync("access_token");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }

        }
    }
}
