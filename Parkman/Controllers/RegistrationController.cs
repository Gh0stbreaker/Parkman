using Microsoft.AspNetCore.Mvc;
using Parkman.Infrastructure.Services;
using Parkman.Models;

namespace Parkman.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationController : ControllerBase
{
    private readonly IUserVehicleRegistrationService _vehicleRegistrationService;
    private readonly IUserCompanyRegistrationService _companyRegistrationService;

    public RegistrationController(
        IUserVehicleRegistrationService vehicleRegistrationService,
        IUserCompanyRegistrationService companyRegistrationService)
    {
        _vehicleRegistrationService = vehicleRegistrationService;
        _companyRegistrationService = companyRegistrationService;
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterWithVehicleRequest request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var result = await _vehicleRegistrationService.RegisterAsync(
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

    [HttpPost("company")]
    public async Task<IActionResult> RegisterCompany(RegisterCompanyRequest request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var result = await _companyRegistrationService.RegisterAsync(
            request.Email,
            request.Password,
            request.CompanyName,
            request.Ico,
            request.Dic,
            request.ContactPersonName,
            request.ContactEmail,
            request.PhoneNumber,
            request.BillingAddress,
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
