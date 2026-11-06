using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDirWeb.Exceptions;

public class GlobalExceptionHandler() : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var statusCode = StatusCodes.Status500InternalServerError;
        var errMsg = "Something went wrong. Try again later.";

        if (exception is IEmployeeException emEx)
        {
            statusCode = emEx.StatusCode;
            errMsg = exception.Message;
        }

        ProblemDetails problemDetails = new() { Status = statusCode, Detail = errMsg };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
