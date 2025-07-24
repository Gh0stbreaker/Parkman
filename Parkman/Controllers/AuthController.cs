using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Parkman.Shared.Entities;
using Parkman.Infrastructure.Services;
using Parkman.Shared.Models;

namespace Parkman.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserVehicleRegistrationService _vehicleRegistrationService;
    private readonly IUserCompanyRegistrationService _companyRegistrationService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUserVehicleRegistrationService vehicleRegistrationService,
        IUserCompanyRegistrationService companyRegistrationService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _vehicleRegistrationService = vehicleRegistrationService;
        _companyRegistrationService = companyRegistrationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterWithVehicleRequest request)
    {
        if(!ModelState.IsValid)
            return ValidationProblem(ModelState);

        if(request.Password != request.ConfirmPassword)
        {
            ModelState.AddModelError(nameof(request.ConfirmPassword), "Passwords do not match.");
            return ValidationProblem(ModelState);
        }

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
            request.Shareable,
            request.CompanyEmail,
            request.PairingPassword);

        if(!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(ModelState);
        }

        return Ok();
    }

    [HttpPost("register/company")]
    public async Task<IActionResult> RegisterCompany(RegisterCompanyRequest request)
    {
        if(!ModelState.IsValid)
            return ValidationProblem(ModelState);

        if(request.Password != request.ConfirmPassword)
        {
            ModelState.AddModelError(nameof(request.ConfirmPassword), "Passwords do not match.");
            return ValidationProblem(ModelState);
        }

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

        if(!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(ModelState);
        }

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if(!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            return Ok();
        }

        if (result.IsLockedOut)
        {
            return Forbid();
        }

        if (result.IsNotAllowed)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        return Unauthorized();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
}
