////using System.Net.Http.Headers;
////using System.Text;
////using System.Text.Json;
////using MauiToDo.Models; // MAUI taraf�nda ToDoItem modelini burada tan�mlayaca��z

////namespace MauiToDo.Services
////{
////	public class ApiService
////	{
////		private readonly HttpClient _http;
////		private readonly string _baseUrl = "https://10.0.2.2:7160/api"; // Android i�in localhost yerine 10.0.2.2

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
                    _logger?.LogWarning("GetToDos ba�ar�s�z. Status Code: {StatusCode}, Reason: {ReasonPhrase}",
                        response.StatusCode, response.ReasonPhrase);
                    return new List<ToDoItem>();
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ToDoItem>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "GetToDos i�lemi s�ras�nda hata olu�tu");
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
                    _logger?.LogError("POST i�lemi ba�ar�s�z. Endpoint: {Endpoint}, Status: {StatusCode}, Error: {Error}",
                        endpoint, response.StatusCode, errorContent);
                }
                else
                {
                    _logger?.LogInformation("POST i�lemi ba�ar�l�. Endpoint: {Endpoint}, Status: {StatusCode}",
                        endpoint, response.StatusCode);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "POST i�lemi s�ras�nda exception olu�tu. Endpoint: {Endpoint}", endpoint);
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
                    _logger?.LogError("PUT i�lemi ba�ar�s�z. Endpoint: {Endpoint}, Status: {StatusCode}, Error: {Error}",
                        endpoint, response.StatusCode, errorContent);
                }
                else
                {
                    _logger?.LogInformation("PUT i�lemi ba�ar�l�. Endpoint: {Endpoint}, Status: {StatusCode}",
                        endpoint, response.StatusCode);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "PUT i�lemi s�ras�nda exception olu�tu. Endpoint: {Endpoint}", endpoint);
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
                    _logger?.LogError("DELETE i�lemi ba�ar�s�z. Endpoint: {Endpoint}, Status: {StatusCode}, Error: {Error}",
                        endpoint, response.StatusCode, errorContent);
                }
                else
                {
                    _logger?.LogInformation("DELETE i�lemi ba�ar�l�. Endpoint: {Endpoint}, Status: {StatusCode}",
                        endpoint, response.StatusCode);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "DELETE i�lemi s�ras�nda exception olu�tu. Endpoint: {Endpoint}", endpoint);
                throw;
            }
        }

        // Yard�mc� method - ToDoItem kaydetme i�in �zel method
        public async Task<bool> SaveToDoAsync(ToDoItem item)
        {
            try
            {
                var json = JsonSerializer.Serialize(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await PostAsync("todo", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger?.LogInformation("ToDoItem ba�ar�yla kaydedildi. Id: {Id}, Title: {Title}", item.Id, item.Title);
                    return true;
                }
                else
                {
                    _logger?.LogError("ToDoItem kaydetme ba�ar�s�z. Id: {Id}, Title: {Title}", item.Id, item.Title);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "ToDoItem kaydetme s�ras�nda hata. Id: {Id}, Title: {Title}", item.Id, item.Title);
                return false;
            }
        }

        // Yard�mc� method - ToDoItem g�ncelleme i�in �zel method
        public async Task<bool> UpdateToDoAsync(int id, ToDoItem item)
        {
            try
            {
                var json = JsonSerializer.Serialize(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await PutAsync($"todo/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger?.LogInformation("ToDoItem ba�ar�yla g�ncellendi. Id: {Id}, Title: {Title}", id, item.Title);
                    return true;
                }
                else
                {
                    _logger?.LogError("ToDoItem g�ncelleme ba�ar�s�z. Id: {Id}, Title: {Title}", id, item.Title);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "ToDoItem g�ncelleme s�ras�nda hata. Id: {Id}, Title: {Title}", id, item.Title);
                return false;
            }
        }

        // Yard�mc� method - ToDoItem silme i�in �zel method
        public async Task<bool> DeleteToDoAsync(int id)
        {
            try
            {
                var response = await DeleteAsync($"todo/{id}");

                if (response.IsSuccessStatusCode)
                {
                    _logger?.LogInformation("ToDoItem ba�ar�yla silindi. Id: {Id}", id);
                    return true;
                }
                else
                {
                    _logger?.LogError("ToDoItem silme ba�ar�s�z. Id: {Id}", id);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "ToDoItem silme s�ras�nda hata. Id: {Id}", id);
                return false;
            }
        }
    }
}