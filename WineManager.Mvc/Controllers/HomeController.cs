using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WineManager.Mvc.Models;
using WineManager.EntityModels;

namespace WineManager.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory; //factory for creating a http client
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
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

        /// <summary>
        /// Calls the wine manager web api, retrieving all customers and passes them to the view
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Wines()
        {
            string uri;

            ViewData["Title"] = "Wines";
            uri = "api/Wines";

            HttpClient client = _httpClientFactory.CreateClient(name: "WineManager.WebApi");
            HttpRequestMessage request = new(method: HttpMethod.Get, requestUri: uri);
            HttpResponseMessage response = await client.SendAsync(request);
            IEnumerable<Wine>? model = await response.Content.ReadFromJsonAsync<IEnumerable<Wine>>();

            return View(model);
        }
    }
}
