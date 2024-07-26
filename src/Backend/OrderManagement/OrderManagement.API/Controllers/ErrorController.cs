using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Domain.Exceptions;

namespace OrderManagement.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError()
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionHandlerFeature == null)
                return Problem();

            var statusCode = exceptionHandlerFeature.Error switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var errorMessage = exceptionHandlerFeature.Error switch
            {
                NotFoundException => $"{((NotFoundException)exceptionHandlerFeature.Error).Message}",
                    ValidationException => $"{((ValidationException)exceptionHandlerFeature.Error).Message}",
                    _ => null
            };

            return Problem(detail: errorMessage, statusCode: statusCode);
        }
    }
}