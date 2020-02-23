namespace UniqueWords.WebApp.Controllers
{
    using System.Net.Mime;
    using Microsoft.AspNetCore.Mvc;
    using StartupConfigs;

    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    //[Consumes(MediaTypeNames.Application.Json)] // GETs issue with asp.net core 2.2
    [Route("api/[controller]")]
    public abstract class WebApiControllerBase : ControllerBase
    {
        
    }
}
