using System.Net.Mime;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Playground.Server.Filters;

public class ExceptionFilter : ExceptionFilterAttribute
{
    private readonly bool _isDevelopment;
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(
        IHostEnvironment hostEnvironment,
        ProblemDetailsFactory problemDetailsFactory,
        ILogger<ExceptionFilter> logger)
    {
        _isDevelopment = hostEnvironment.IsDevelopment();
        _problemDetailsFactory = problemDetailsFactory;
        _logger = logger;
    }

    public async override Task OnExceptionAsync(
        ExceptionContext context)
    {
        await base.OnExceptionAsync(context);

        var httpContext = context.HttpContext;
        var error = context.Exception;
        using (var reader = new StreamReader(httpContext.Request.Body))
        {
            _logger.LogError(
                "Path: [{method}] {path}" +
                "\nBody: {body}" +
                "\nException: {error}", httpContext.Request.Method, httpContext.Request.GetDisplayUrl(), await reader.ReadToEndAsync(), error);
        }
        
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        if (!_isDevelopment)
        {
            await context.HttpContext.Response
                .WriteAsJsonAsync(_problemDetailsFactory.CreateProblemDetails(httpContext,
                    title: error.Message,
                    detail: error.StackTrace));
        }
        else
        {
            await context.HttpContext.Response
                .WriteAsJsonAsync(_problemDetailsFactory.CreateProblemDetails(httpContext));
        }

        context.ExceptionHandled = true;
    }
}
