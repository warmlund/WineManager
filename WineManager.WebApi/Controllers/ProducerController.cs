using Microsoft.AspNetCore.Mvc;
using WineManager.EntityModels;
using WineManager.WebApi.Repositories;

namespace WineManager.WebApi.Controllers
{
    //base address: api/producers
    [Route("api/producers")]
    [ApiController]
    public class ProducerController : Controller
    {
        private readonly IProducerRepository _repo;
        public ProducerController(IProducerRepository repo)
        {
            _repo = repo;
        }

        //GET: api/producers
        //returns a list of all producers
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(IEnumerable<Producer>))]
        public async Task<IEnumerable<Producer>> GetProducers()
        {
            return await _repo.RetrieveAllAsync();
        }

        //GET: api/producers/id
        [HttpGet("{name}", Name =nameof(GetProducer))] //named route
        [ProducesResponseType(200, Type=typeof(Producer))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProducer(string name)
        {
            Producer? p =await _repo.RetrieveAsync(name);

            if (p == null)
            {
                return NotFound();
            }

            return Ok(p);
        }

        //POST: api/producers
        //BODY: Producer (JSON, XML)
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Producer))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateProducer([FromBody] Producer producer)
        {
            if (producer == null)
            {
                return BadRequest(); //400
            }

            Producer? addedProducer = await _repo.CreateAsync(producer);

            if (addedProducer == null)
                return BadRequest("Repository failed to create producer"); //400 with message

            else
            {
                return CreatedAtRoute( // 201 Created
                    routeName: nameof(GetProducer),
                    routeValues: new { name = addedProducer.ProducerName },
                    value: addedProducer);
            }
        }

        //PUT: api/producers/[name]
        //BODY: Producer (JSON, XML)
        [HttpPut("{name}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(string name, [FromBody] Producer producer)
        {
            if (producer == null || producer.ProducerName != name)
                return BadRequest(); //400

            Producer? existingProducer = await _repo.RetrieveAsync(name);

            if(existingProducer == null)
                return NotFound(); //404

            await _repo.UpdateAsync(producer);
            return new NoContentResult(); //204
        }

        //DELETE: api/producers/[name]
        [HttpDelete("{name}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                ProblemDetails problemDetails = new()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://localhost:5151/producers/failed-to-delete",
                    Title = $"Producer {name} found but failed to delete",
                    Instance = HttpContext.Request.Path
                };

                return BadRequest(problemDetails); //400
            }

            Producer? existingProducer = await _repo.RetrieveAsync(name);
            if (existingProducer==null)
                return NotFound(); //404

            bool? deleted = await _repo.DeleteAsync(name);

            if(deleted.HasValue && deleted.Value)
                return new NoContentResult(); //204

            else
                return BadRequest($"Producer {name} failed to be deleted."); //400
        }
    } 
}
