using Microsoft.AspNetCore.Mvc;
using Parkman.Infrastructure.Services;
using Parkman.Models;

namespace Parkman.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationController : ControllerBase
{
    private readonly IUserVehicleRegistrationService _service;

    public RegistrationController(IUserVehicleRegistrationService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterWithVehicleRequest request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var result = await _service.RegisterAsync(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.PhoneNumber,
            request.Address,
            request.LicensePlate,
            request.Brand,
            request.Type,
            request.PropulsionType,
            request.Shareable);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(ModelState);
        }

        return Ok();
    }
}
