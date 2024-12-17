using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using WineManager.EntityModels;
using WineManager.Mvc.Models;

namespace WineManager.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("WineManager.WebApi");
        }

        public async Task<IActionResult> Index()
        {
            // API endpoint
            string apiUrl = "api/wines";
            List<Wine> wines = new();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    wines = JsonSerializer.Deserialize<List<Wine>>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = false
                    });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error fetching data from API");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Request exception: {ex.Message}");
            }

            return View(wines);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
