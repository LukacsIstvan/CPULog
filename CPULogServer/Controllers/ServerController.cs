using CPULogServer.Services.ServerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CPULogServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServerController : Controller
    {

        private IServerService _serverService;
        private ILogger _logger;

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

    }
}
