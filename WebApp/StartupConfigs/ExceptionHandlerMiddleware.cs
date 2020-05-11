using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace UniqueWords.WebApp.StartupConfigs
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        private HttpContext _httpContext;
        private IWebHostEnvironment _env;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext, IWebHostEnvironment env)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (httpContext.Response.HasStarted)
                {
                    throw;
                }

                _httpContext = httpContext;
                _env = env;

                await HandleExceptionAsync(ex);
            }
        }

        private async Task HandleExceptionAsync(Exception exception)
        {
            var problemDetails = CreateProblemDetails(exception);
            await SetRequestResponseAsync(problemDetails);
            await LogExceptionAsync(problemDetails);
        }

        private ProblemDetails CreateProblemDetails(Exception exception)
        {
            var statusCode = (int)MapExceptionToHttpStatusCode(exception);
            var detail = _env.IsDevelopment() ? exception.ToString() : exception.Message;
            var problem = new ProblemDetails
            {
                Title = "An unexpected error occurred!",
                Status = statusCode,
                Detail = detail,
                Type = exception.GetType().ToString(),
                Instance = Guid.NewGuid().ToString()
            };

            return problem;
        }

        private static HttpStatusCode MapExceptionToHttpStatusCode(Exception exception)
        {
            switch (exception)
            {
                case ArgumentException _:
                    return HttpStatusCode.BadRequest;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }

        private async Task SetRequestResponseAsync(ProblemDetails problem)
        {
            var json = JsonSerializer.Serialize(problem);

            _httpContext.Response.StatusCode = problem.Status ?? (int)HttpStatusCode.InternalServerError;
            _httpContext.Response.ContentType = MediaTypeNames.Application.Json;

            await _httpContext.Response.WriteAsync(json);
        }

        private async Task LogExceptionAsync(ProblemDetails problemDetails)
        {
            var request = _httpContext.Request;
            string body;

            using (var reader = new StreamReader(request.Body))
            {
                body = await reader.ReadToEndAsync();
            }

            _logger.LogError($"[instance]: {problemDetails.Instance}, [path]: {request.Path}, [method]: {request.Method}, [query]: {request.QueryString}, [body]: {body}");
        }
    }
}
