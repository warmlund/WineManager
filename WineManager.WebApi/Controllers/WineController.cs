using Microsoft.AspNetCore.Mvc;
using WineManager.EntityModels;
using WineManager.WebApi.Repositories;

namespace WineManager.WebApi.Controllers
{
    /// <summary>
    /// Controller for wine. Handles incoming requests and uses the repository for handling
    /// </summary>

    //base address: api/wines
    [Route("api/wines")]
    [ApiController]
    public class WineController : Controller
    {
        private readonly IWineRepository _repo;
        public WineController(IWineRepository repo)
        {
            _repo = repo;
        }

        //GET: api/wines
        //returns a list of all wines
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Wine>))]
        public async Task<IEnumerable<Wine>> GetWines()
        {
            return await _repo.RetrieveAllAsync();
        }

        //GET: api/wines/id
        [HttpGet("{id}", Name = nameof(GetWine))] //named route
        [ProducesResponseType(200, Type = typeof(Wine))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetWine(int id)
        {
            Wine? p = await _repo.RetrieveAsync(id);

            if (p == null)
            {
                return NotFound();
            }

            return Ok(p);
        }

        //POST: api/wines
        //BODY: Wine (JSON, XML)
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Wine))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateWine([FromBody] Wine wine)
        {
            if (wine == null)
            {
                return BadRequest(); //400
            }

            Wine? addedWine = await _repo.CreateAsync(wine);

            if (addedWine == null)
                return BadRequest("Repository failed to create wine"); //400 with message

            else
            {
                return CreatedAtRoute( // 201 Created
                    routeName: nameof(GetWine),
                    routeValues: new { id = addedWine.WineId },
                    value: addedWine);
            }
        }

        //PUT: api/wines/[id]
        //BODY: Wine (JSON, XML)
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] Wine wine)
        {
            if (wine == null || wine.WineId != id)
                return BadRequest(); //400

            Wine? existingWine = await _repo.RetrieveAsync(id);

            if (existingWine == null)
                return NotFound(); //404

            await _repo.UpdateAsync(wine);
            return new NoContentResult(); //204
        }

        //DELETE: api/wines/[id]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == -1)
            {
                ProblemDetails problemDetails = new()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://localhost:5151/wines/failed-to-delete",
                    Title = $"Wine Id {id} found but failed to delete",
                    Instance = HttpContext.Request.Path
                };

                return BadRequest(problemDetails); //400
            }

            Wine? existingWine = await _repo.RetrieveAsync(id);
            if (existingWine == null)
                return NotFound(); //404

            bool? deleted = await _repo.DeleteAsync(id);

            if (deleted.HasValue && deleted.Value)
                return new NoContentResult(); //204

            else
                return BadRequest($"Wine {id} failed to be deleted."); //400
        }
    }
}
