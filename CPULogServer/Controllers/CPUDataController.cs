using CPULogServer.Models;
using CPULogServer.Services.CPUDataService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CPULogServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CPUDataController : Controller
    {
        private ICPUDataService _cpuDataService;
        private ILogger<CPUDataController> _logger;
        public CPUDataController(ICPUDataService cpuDataService, ILogger<CPUDataController> logger)
        {
            _cpuDataService = cpuDataService;
            _logger = logger;
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            return Ok(_cpuDataService.Get().Result);
        }
        [HttpGet("get/{id}")]
        public IActionResult Get(int id) 
        {
            return Ok(_cpuDataService.Get(id).Result);
        }
        [HttpGet("get/client/{id}")]
        public IActionResult GetByClient(int id)
        {
            return Ok(_cpuDataService.GetByClient(id).Result);
        }
        [HttpPost]
        public async Task<ActionResult> Store(CPUDataModel cpuData)
        {
            return Ok(_cpuDataService.Store(cpuData));
        }
        [HttpPut]
        public async Task<ActionResult> Update(int id)
        {
            return Ok(_cpuDataService.Delete(id));
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(_cpuDataService.Delete(id));
        }
    }
}
