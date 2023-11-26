using CPULogServer.Models;
using CPULogServer.Services.ClientService;
using CPULogServer.Services.CPUDataService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;


namespace CPULogServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private IClientService _clientService;
        private ILogger<ClientController> _logger;
        public ClientController(IClientService clientService, ILogger<ClientController> logger)
        {
            _clientService = clientService;
            _logger = logger;
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            return Ok(_clientService.Get().Result);
        }
        [HttpGet("get/{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_clientService.Get(id).Result);
        }
        [HttpPost]
        public async Task<ActionResult> Store(Client cpuData)
        {
            return Ok(_clientService.Store(cpuData));
        }
        
        [HttpPut]
        public async Task<ActionResult> Update(int id, Client client)
        {
            return Ok(_clientService.Update(id, client));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(_clientService.Delete(id));
        }
        [HttpPost("sensor")]
        public IActionResult SetSensorTimer([FromBody] SetSensor model)
        {
            _clientService.SetSensor(model.Id, model.Value);
            return Ok("Request sent!");
        }

    }
}
