using CPULogServer.Models;
using CPULogServer.Services.ServerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CPULogServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServerController : Controller
    {

        private IServerService _serverService;
        private ILogger<ServerController> _logger;
        public ServerController(IServerService service, ILogger<ServerController> logger)
        {
            _serverService = service;
            _logger = logger;
        }

        [HttpPost("start")]
        public IActionResult StartServer()
        {
            _serverService.StartServer();
            return Ok("Server started successfully!");
        }

        [HttpGet("getData")]
        public IActionResult GetData()
        {
            ClientModel client = new ClientModel();
            client.Ip = "127.0.0.1";
            _serverService.GetCPUData(client);
            return Ok("Request sent!");
        }

        [HttpPost("setTimer")]
        public IActionResult SetSensorTimer(double value)
        {
            ClientModel client = new ClientModel();
            client.Ip = "127.0.0.1";
            _serverService.SetSensorTimer(client, value);
            return Ok("Request sent!");
        }

    }
}
