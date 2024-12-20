using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using WineManager.EntityModels;
using WineManager.Mvc.Models;

namespace WineManager.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("WineManager.WebApi"); //creates a htppclient and uses the factory to consume the web api
        }

        public async Task<IActionResult> Index()
        {
            string apiUrl = "api/wines"; //string of the url to the api endpoint
            ViewData["Title"] = "Wines";

            HttpRequestMessage request = new(
              method: HttpMethod.Get, requestUri: apiUrl); //Create get request to the web api

            HttpResponseMessage response = await _httpClient.SendAsync(request); //awaits response of the get request

            IEnumerable<Wine>? model = await response.Content
              .ReadFromJsonAsync<IEnumerable<Wine>>(); //Serialize the content of the response 

            List<Wine> wine = model.OrderBy(w => w.WineName).ToList(); //Sort the retrieved wines as a sorted list

            return View(wine); //Pass the list to the view
        }

        public async Task<IActionResult> Producers()
        {
            List<Producer> producer = await GetProducerList(); //Retrieves a list of producers

            return View(producer); //Pass the list to the view
        }

        private async Task<List<Producer>> GetProducerList()
        {
            //API endpoint
            string apiUrl = "api/producers";
            ViewData["Title"] = "Producers"; //Sets the title of the view

            HttpRequestMessage request = new(
                method: HttpMethod.Get, requestUri: apiUrl); //Make get request

            HttpResponseMessage response = await _httpClient.SendAsync(request); //await response

            IEnumerable<Producer> model = await response.Content
                .ReadFromJsonAsync<IEnumerable<Producer>>(); //Serialize the response result of producers

            List<Producer> producer = model.OrderBy(producer => producer.ProducerName).ToList(); //Create list of producers and sort by name
            return producer; //return list
        }

        //GET add producer
        public IActionResult AddProducer()
        {
            //Shows the add producer form
            ViewData["Title"] = "Add Producer"; //Sets the title
            return View();
        }

        //POST add producer
        [HttpPost]
        public async Task<IActionResult> AddProducer(Producer producer)
        {
            HttpClient client = _httpClientFactory.CreateClient(name: "WineManager.WebApi"); //Creates client to consume web api
            HttpResponseMessage response = await client.PostAsJsonAsync(requestUri: "api/producers", value: producer); //Requests to add an new producers

            if (response.IsSuccessStatusCode) //if the producer is successfully added
                TempData["success"] = "Producer added"; //Set temp success information
            else
                TempData["error"] = "Producer not added"; //if producer is not added, sets the temp error information

            return RedirectToAction("Producers"); //Redirect to the producer view after the operation
        }

        //GET delete producer
        public async Task<IActionResult> DeleteProducer(string producerName)
        {
            HttpClient client = _httpClientFactory.CreateClient(name: "WineManager.WebApi"); //Creates client to consume web api

            Producer? producer = await client.GetFromJsonAsync<Producer>(
                requestUri: $"api/producers/{producerName}"); //Retrieves the producer to be deleted

            ViewData["Title"] = "Delete Producer"; //Sets title for the delete page

            return View(producer); //Passes producer as model to the view
        }

        //POST deletion of producer
        public async Task<IActionResult> DeleteProducerPost(string producerName)
        {

            HttpClient client = _httpClientFactory.CreateClient(name: "WineManager.WebApi"); //Creates client to consume web api
            HttpResponseMessage response = await client.DeleteAsync(requestUri: $"api/producers/{producerName}"); //requests to delete producer by name

            if (response.IsSuccessStatusCode) //if deletion is successful
                TempData["success"] = "Producer deleted"; //Sets temp data for siccess

            else
                TempData["error"] = "Producer not deleted"; //Sets temp data for error if not successful

            return RedirectToAction("Producers"); //Redirect to the producer view
        }

        //GET add wine
        public async Task<IActionResult> AddWineAsync()
        {
            //Creates a selectlistitem of producer for the populating a dropdown list in the form
            var producers = GetProducerList().Result.Select(p => new SelectListItem
            {
                Value = p.ProducerName,
                Text = p.ProducerName
            }).ToList();

            ViewBag.Producers = producers; //sets a viewbag with the selectlistitem

            return View(new Wine()); //Populates the view with a new wine instance
        }

        //POST add wine
        [HttpPost]
        public async Task<IActionResult> AddWine(Wine wine)
        {
            HttpClient client = _httpClientFactory.CreateClient(name: "WineManager.WebApi"); //Creates client to consume web api
            HttpResponseMessage response = await client.PostAsJsonAsync(requestUri: "api/wines", value: wine); //requests to add a new wine

            if (response.IsSuccessStatusCode) //If successful
                TempData["success"] = $"Wine added"; //Sets temp data for success if successful
            else
                TempData["error"] = $"Wine not added"; //Sets temp data for error if not successful

            return RedirectToAction("Index"); //Redirect the wine page
        }

        //GET delete wine
        public async Task<IActionResult> DeleteWine(int wineId)
        {
            HttpClient client = _httpClientFactory.CreateClient(name: "WineManager.WebApi"); //Creates client to consume web api

            Wine? wine = await client.GetFromJsonAsync<Wine>(requestUri: $"api/wines/{wineId}"); //request the api to get the wine by wine id

            ViewData["Title"] = "Delete Wine"; //Sets the view title

            return View(wine); //Passes wine as model to the view
        }

        //POST deletion of wine
        public async Task<IActionResult> DeleteWinePost(int wineId)
        {
            HttpClient client = _httpClientFactory.CreateClient(name: "WineManager.WebApi"); //Creates client to consume web api
            HttpResponseMessage response = await client.DeleteAsync(requestUri: $"api/wines/{wineId}"); //requests to delete wine

            if (response.IsSuccessStatusCode) //if deletion is successful
                TempData["success"] = "Wine deleted"; //Set temp success information

            else
                TempData["error"] = "Wine not deleted"; //Set temp error information

            return RedirectToAction("Index"); //Redirects to the wine page
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
