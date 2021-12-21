using Donation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Donation.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHttpContextAccessor accessor;
        private readonly ILogger<MainController> logger;

        public ExceptionMiddleware(RequestDelegate next, IHttpContextAccessor accessor, ILogger<MainController> logger)
        {
            this.next = next;
            this.logger = logger;
            this.accessor = accessor;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (TimeoutException ex)
            {
                logger.LogError(ex, $"A service is unavailable: {accessor.HttpContext.TraceIdentifier}.");

                await HandleExceptionMessageAsync(accessor.HttpContext, HttpStatusCode.ServiceUnavailable);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error was ocurred during the process. Trace Identifier: {accessor.HttpContext.TraceIdentifier}.");

                await HandleExceptionMessageAsync(accessor.HttpContext, HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, HttpStatusCode status)
        {
            string response = JsonSerializer.Serialize(new ValidationProblemDetails()
            {
                Title = "An error was occurred.",
                Status = (int)status,
                Instance = context.Request.Path,
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(response);
        }
    }
}