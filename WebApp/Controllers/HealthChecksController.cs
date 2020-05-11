using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UniqueWords.WebApp.Controllers
{
    [ApiVersion(WebApiDefaults.LatestVersion)]
    public class HealthChecksController : WebApiControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthChecksController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet]
        public async Task<ActionResult<HealthReport>> Get()
        {
            var report = await _healthCheckService.CheckHealthAsync();
            return report;
        }
    }
}