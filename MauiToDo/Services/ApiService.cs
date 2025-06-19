////using System.Net.Http.Headers;
////using System.Text;
////using System.Text.Json;
////using MauiToDo.Models; // MAUI tarafýnda ToDoItem modelini burada tanýmlayacaðýz

////namespace MauiToDo.Services
////{
////	public class ApiService
////	{
////		private readonly HttpClient _http;
////		private readonly string _baseUrl = "https://10.0.2.2:7160/api"; // Android için localhost yerine 10.0.2.2

////		public string Token { get; set; }

////		public ApiService()
////		{
////			_http = new HttpClient();
////		}

////		private void SetAuthHeader()
////		{
////			if (!string.IsNullOrEmpty(Token))
////				_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
////		}

////		public async Task<List<ToDoItem>> GetToDosAsync()
////		{
////			SetAuthHeader();

////			var response = await _http.GetAsync($"{_baseUrl}/todo");

////			if (!response.IsSuccessStatusCode)
////				return new List<ToDoItem>();

////			var json = await response.Content.ReadAsStringAsync();
////			return JsonSerializer.Deserialize<List<ToDoItem>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
////		}
////		public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content)
////		{
////			SetAuthHeader();
////			return await _http.PostAsync($"{_baseUrl}/{endpoint}", content);
////		}

////	}
////}
///////////////////////////////////////////////////////////////////////////////////



//using System.Net.Http.Headers;
//using System.Text;
//using System.Text.Json;
//using MauiToDo.Models;

//namespace MauiToDo.Services
//{
//	public class ApiService
//	{
//		private readonly HttpClient _http;
//		private readonly string _baseUrl = "http://localhost:5130/api";


//		public string Token { get; set; }

//		public ApiService()
//		{
//			_http = new HttpClient();
//		}

//		private void SetAuthHeader()
//		{
//			if (!string.IsNullOrEmpty(Token))
//				_http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
//		}

//		public async Task<List<ToDoItem>> GetToDosAsync()
//		{
//			SetAuthHeader();

//			var response = await _http.GetAsync($"{_baseUrl}/todo");

//			if (!response.IsSuccessStatusCode)
//				return new List<ToDoItem>();

//			var json = await response.Content.ReadAsStringAsync();
//			return JsonSerializer.Deserialize<List<ToDoItem>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
//		}

//		public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content)
//		{
//			SetAuthHeader();
//			return await _http.PostAsync($"{_baseUrl}/{endpoint}", content);
//		}

//		public async Task<HttpResponseMessage> PutAsync(string endpoint, HttpContent content)
//		{
//			SetAuthHeader();
//			return await _http.PutAsync($"{_baseUrl}/{endpoint}", content);
//		}

//		public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
//		{
//			SetAuthHeader();
//			return await _http.DeleteAsync($"{_baseUrl}/{endpoint}");
//		}
//	}
//}

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MauiToDo.Models;
using Microsoft.Extensions.Logging;

namespace MauiToDo.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl = "http://localhost:5130/api";
        private readonly ILogger<ApiService> _logger;

        public string Token { get; set; }

        public ApiService(ILogger<ApiService> logger = null)
        {
            _http = new HttpClient();
            _logger = logger;
        }

        private void SetAuthHeader()
        {
            if (!string.IsNullOrEmpty(Token))
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        }

        public async Task<List<ToDoItem>> GetToDosAsync()
        {
            try
            {
                SetAuthHeader();
                var response = await _http.GetAsync($"{_baseUrl}/todo");

                if (!response.IsSuccessStatusCode)
                {
                    _logger?.LogWarning("GetToDos baþarýsýz. Status Code: {StatusCode}, Reason: {ReasonPhrase}",
                        response.StatusCode, response.ReasonPhrase);
                    return new List<ToDoItem>();
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ToDoItem>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "GetToDos iþlemi sýrasýnda hata oluþtu");
                return new List<ToDoItem>();
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content)
        {
            try
            {
                SetAuthHeader();
                var response = await _http.PostAsync($"{_baseUrl}/{endpoint}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger?.LogError("POST iþlemi baþarýsýz. Endpoint: {Endpoint}, Status: {StatusCode}, Error: {Error}",
                        endpoint, response.StatusCode, errorContent);
                }
                else
                {
                    _logger?.LogInformation("POST iþlemi baþarýlý. Endpoint: {Endpoint}, Status: {StatusCode}",
                        endpoint, response.StatusCode);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "POST iþlemi sýrasýnda exception oluþtu. Endpoint: {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<HttpResponseMessage> PutAsync(string endpoint, HttpContent content)
        {
            try
            {
                SetAuthHeader();
                var response = await _http.PutAsync($"{_baseUrl}/{endpoint}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger?.LogError("PUT iþlemi baþarýsýz. Endpoint: {Endpoint}, Status: {StatusCode}, Error: {Error}",
                        endpoint, response.StatusCode, errorContent);
                }
                else
                {
                    _logger?.LogInformation("PUT iþlemi baþarýlý. Endpoint: {Endpoint}, Status: {StatusCode}",
                        endpoint, response.StatusCode);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "PUT iþlemi sýrasýnda exception oluþtu. Endpoint: {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            try
            {
                SetAuthHeader();
                var response = await _http.DeleteAsync($"{_baseUrl}/{endpoint}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger?.LogError("DELETE iþlemi baþarýsýz. Endpoint: {Endpoint}, Status: {StatusCode}, Error: {Error}",
                        endpoint, response.StatusCode, errorContent);
                }
                else
                {
                    _logger?.LogInformation("DELETE iþlemi baþarýlý. Endpoint: {Endpoint}, Status: {StatusCode}",
                        endpoint, response.StatusCode);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "DELETE iþlemi sýrasýnda exception oluþtu. Endpoint: {Endpoint}", endpoint);
                throw;
            }
        }

        // Yardýmcý method - ToDoItem kaydetme için özel method
        public async Task<bool> SaveToDoAsync(ToDoItem item)
        {
            try
            {
                var json = JsonSerializer.Serialize(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await PostAsync("todo", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger?.LogInformation("ToDoItem baþarýyla kaydedildi. Id: {Id}, Title: {Title}", item.Id, item.Title);
                    return true;
                }
                else
                {
                    _logger?.LogError("ToDoItem kaydetme baþarýsýz. Id: {Id}, Title: {Title}", item.Id, item.Title);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "ToDoItem kaydetme sýrasýnda hata. Id: {Id}, Title: {Title}", item.Id, item.Title);
                return false;
            }
        }

        // Yardýmcý method - ToDoItem güncelleme için özel method
        public async Task<bool> UpdateToDoAsync(int id, ToDoItem item)
        {
            try
            {
                var json = JsonSerializer.Serialize(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await PutAsync($"todo/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger?.LogInformation("ToDoItem baþarýyla güncellendi. Id: {Id}, Title: {Title}", id, item.Title);
                    return true;
                }
                else
                {
                    _logger?.LogError("ToDoItem güncelleme baþarýsýz. Id: {Id}, Title: {Title}", id, item.Title);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "ToDoItem güncelleme sýrasýnda hata. Id: {Id}, Title: {Title}", id, item.Title);
                return false;
            }
        }

        // Yardýmcý method - ToDoItem silme için özel method
        public async Task<bool> DeleteToDoAsync(int id)
        {
            try
            {
                var response = await DeleteAsync($"todo/{id}");

                if (response.IsSuccessStatusCode)
                {
                    _logger?.LogInformation("ToDoItem baþarýyla silindi. Id: {Id}", id);
                    return true;
                }
                else
                {
                    _logger?.LogError("ToDoItem silme baþarýsýz. Id: {Id}", id);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "ToDoItem silme sýrasýnda hata. Id: {Id}", id);
                return false;
            }
        }
    }
}