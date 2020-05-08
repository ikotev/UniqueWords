using Microsoft.AspNetCore.Mvc;

using System.Net.Mime;

namespace UniqueWords.WebApp.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    //[Consumes(MediaTypeNames.Application.Json)] // GETs issue with asp.net core 2.2
    [Route("api/[controller]")]
    public abstract class WebApiControllerBase : ControllerBase
    {

    }
}
