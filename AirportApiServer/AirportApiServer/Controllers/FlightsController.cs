using AirportApiServer.Models.Entities;
using AirportApiServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace AirportApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IDataService _dataService;
        public FlightsController(IDataService dataService) => _dataService = dataService;


        [HttpGet]
        public async Task<IEnumerable<Log>> Get()
        {
            return await _dataService.GetLogs();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Flight flight)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _dataService.AddFlight(flight);
                }
                catch (Exception)
                {
                    return StatusCode(500);
                }
            }

            return StatusCode(201);
        }
    }
}
