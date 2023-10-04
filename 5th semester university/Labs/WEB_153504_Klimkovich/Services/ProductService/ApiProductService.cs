using Azure;
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

        public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
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

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"good");
            }
            else
            {
                Console.WriteLine($"Ошибка при отправке запроса: {response.StatusCode}");
            }
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
            var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);
            var newItem = await response.Content.ReadFromJsonAsync<Electronics>();
            if (response.IsSuccessStatusCode)
            {
                if (formFile != null)
                {
                    SaveImageAsync(newItem.Id, formFile);
                }

                return await response.Content.ReadFromJsonAsync<ResponseData<Electronics>>(_serializerOptions);
            }

            _logger.LogError($"-----> object not created. Error:{response.StatusCode}");
            return new ResponseData<Electronics>
            {
                Success = false,
                ErrorMessage = $"Объект не добавлен. Error:{response.StatusCode}"
            };
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Electronics>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<Electronics>> UpdateProductAsync(int id, Electronics product, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"Electronics/{id}");
            var response = await _httpClient.PutAsJsonAsync(uri, product, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                if (formFile != null)
                {
                    await SaveImageAsync(id, formFile);
                }

                return await response.Content.ReadFromJsonAsync<ResponseData<Electronics>>(_serializerOptions);
            }

            _logger.LogError($"-----> object not updated. Error:{response.StatusCode}");
            return new ResponseData<Electronics>
            {
                Success = false,
                ErrorMessage = $"Объект не обновлен. Error:{response.StatusCode}"
            };
        }
    }
}
