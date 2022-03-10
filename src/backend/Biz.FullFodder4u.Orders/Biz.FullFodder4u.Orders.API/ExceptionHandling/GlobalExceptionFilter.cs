using Biz.FullFodder4u.Orders.API.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Biz.FullFodder4u.Orders.API.ExceptionHandling;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

        var httpStatusCode = (int)HttpStatusCode.InternalServerError;

        switch (context.Exception)
        {
            case ValidationException:
                httpStatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case NotFoundException:
                httpStatusCode = (int)HttpStatusCode.NotFound;
                break;
        }

        context.HttpContext.Response.StatusCode = httpStatusCode;
        context.ExceptionHandled = true;
    }
}

