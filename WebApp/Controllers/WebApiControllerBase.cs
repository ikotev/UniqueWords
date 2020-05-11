using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;

namespace UniqueWords.WebApp.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route(WebApiDefaults.DefaultRoute)]
    public abstract class WebApiControllerBase : ControllerBase
    {       
        protected List<string> GetRequestAcceptLanguages()
        {
            var acceptLanguages = Request.GetTypedHeaders().AcceptLanguage
                                     .OrderByDescending(al => al.Quality ?? 1)
                                     .Select(al => al.Value.ToString())
                                     .ToList();

            return acceptLanguages;
        }
        protected string GetFirstRequestAcceptLanguage()
        {
            var acceptLanguage = GetRequestAcceptLanguages()
                                     .FirstOrDefault() ?? nameof(WebApiDefaults.DefaultAcceptLanguage);
            return acceptLanguage;
        }
    }
}
