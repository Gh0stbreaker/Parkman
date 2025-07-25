using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Parkman.Controllers;

[ApiController]
public class ErrorController : ControllerBase
{
    [Route("error")]
    public IActionResult HandleError()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        var problem = new ProblemDetails
        {
            Title = "An unexpected error occurred.",
            Detail = feature?.Error.Message,
            Status = StatusCodes.Status500InternalServerError
        };
        return StatusCode(problem.Status ?? 500, problem);
    }
}
