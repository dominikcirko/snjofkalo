using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OICAR.Service.Interface;

namespace OICAR.Controllers
{
    [ApiController]
    [Route("api/logs")]
    [Authorize]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet("{n?}")]
        public IActionResult GetLogs(int n = 10)
        {
            var logs = _logService.GetLatestLogs(n);
            return Ok(logs);
        }

        [HttpGet("count")]
        public IActionResult GetLogCount()
        {
            var count = _logService.GetLogCount();
            return Ok(new { Count = count });
        }
    }
}