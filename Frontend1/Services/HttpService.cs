using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Frontend1.Services
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"]);
        }

        private void AddAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content);
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            AddAuthorizationHeader();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent);
        }

        public async Task<T> PutAsync<T>(string endpoint, object data)
        {
            AddAuthorizationHeader();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent);
        }

        public async Task<T> DeleteAsync<T>(string endpoint)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent);
        }
    }
} 