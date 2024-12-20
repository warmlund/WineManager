using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
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
            ViewData["Title"] = "Wines";

            HttpRequestMessage request = new(
              method: HttpMethod.Get, requestUri: apiUrl);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            IEnumerable<Wine>? model = await response.Content
              .ReadFromJsonAsync<IEnumerable<Wine>>();

            List<Wine> wine = model.OrderBy(w => w.WineName).ToList();

            return View(wine);
        }

        public async Task<IActionResult> Producers()
        {
            List<Producer> producer = await GetProducerList();

            return View(producer);
        }

        private async Task<List<Producer>> GetProducerList()
        {
            //API endpoint
            string apiUrl = "api/producers";
            ViewData["Title"] = "Producers";

            HttpRequestMessage request = new(
                method: HttpMethod.Get, requestUri: apiUrl);

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            IEnumerable<Producer> model = await response.Content
                .ReadFromJsonAsync<IEnumerable<Producer>>();

            List<Producer> producer = model.OrderBy(producer => producer.ProducerName).ToList();
            return producer;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //GET add producer
        public IActionResult AddProducer()
        {
            ViewData["Title"] = "Add Producer";
            return View();
        }

        //POST add producer
        [HttpPost]
        public async Task<IActionResult> AddProducer(Producer producer)
        {
            HttpClient client = _httpClientFactory.CreateClient(name: "WineManager.WebApi");
            HttpResponseMessage response = await client.PostAsJsonAsync(requestUri: "api/producers", value: producer);

            if (response.IsSuccessStatusCode)
                TempData["success"] = "Producer added";
            else
                TempData["error"] = "Producer not added";

            return RedirectToAction("Producers");
        }

        //GET delete producer
        public async Task<IActionResult> DeleteProducer(string producerName)
        {
            HttpClient client = _httpClientFactory.CreateClient(name: "WineManager.WebApi");

            Producer? producer = await client.GetFromJsonAsync<Producer>(
                requestUri: $"api/producers/{producerName}");

            ViewData["Title"] = "Delete Producer";

            return View(producer);
        }

        //POST deletion of producer
        public async Task<IActionResult> DeleteProducerPost(string producerName)
        {
            HttpClient client = _httpClientFactory.CreateClient(name: "WineManager.WebApi");
            HttpResponseMessage response = await client.DeleteAsync(requestUri: $"api/producers/{producerName}");

            if (response.IsSuccessStatusCode)
                TempData["success"] = "Producer deleted";

            else
                TempData["error"] = "Producer not deleted";

            return RedirectToAction("Producers");
        }

        //GET add wine
        public async Task<IActionResult> AddWineAsync()
        {
            var producers = GetProducerList().Result.Select(p => new SelectListItem
            {
                Value = p.ProducerName,
                Text = p.ProducerName
            }).ToList();

            ViewBag.Producers = producers;

            return View(new Wine());
        }

        //POST add wine
        [HttpPost]
        public async Task<IActionResult> AddWine(Wine wine)
        {
            HttpClient client = _httpClientFactory.CreateClient(name: "WineManager.WebApi");
            HttpResponseMessage response = await client.PostAsJsonAsync(requestUri: "api/wines", value: wine);

            if (response.IsSuccessStatusCode)
                TempData["success"] = $"Wine added";
            else
                TempData["error"] = $"Wine not added";

            return RedirectToAction("Index");
        }

        //GET delete wine
        public async Task<IActionResult> DeleteWine(int wineId)
        {
            HttpClient client = _httpClientFactory.CreateClient(name: "WineManager.WebApi");

            Wine? wine = await client.GetFromJsonAsync<Wine>(
                requestUri: $"api/wines/{wineId}");

            ViewData["Title"] = "Delete Wine";

            return View(wine);
        }

        //POST deletion of wine
        public async Task<IActionResult> DeleteWinePost(int wineId)
        {
            HttpClient client = _httpClientFactory.CreateClient(name: "WineManager.WebApi");
            HttpResponseMessage response = await client.DeleteAsync(requestUri: $"api/wines/{wineId}");

            if (response.IsSuccessStatusCode)
                TempData["success"] = "Wine deleted";

            else
                TempData["error"] = "Wine not deleted";

            return RedirectToAction("Index");
        }
    }
}
