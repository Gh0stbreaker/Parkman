using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Parkman.Infrastructure.Services;
using Parkman.Shared.Models;
using Parkman.Domain.Entities;

namespace Parkman.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUserVehicleRegistrationService _vehicleRegistrationService;
    private readonly IUserCompanyRegistrationService _companyRegistrationService;
    private readonly IEmailSender _emailSender;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUserVehicleRegistrationService vehicleRegistrationService,
        IUserCompanyRegistrationService companyRegistrationService,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _vehicleRegistrationService = vehicleRegistrationService;
        _companyRegistrationService = companyRegistrationService;
        _emailSender = emailSender;
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
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user != null)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Auth", new { userId = user.Id, token }, Request.Scheme);
            await _emailSender.SendEmailAsync(request.Email, "Confirm your email", $"Please confirm your account by <a href=\"{link}\">clicking here</a>.");
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
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user != null)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Auth", new { userId = user.Id, token }, Request.Scheme);
            await _emailSender.SendEmailAsync(request.Email, "Confirm your email", $"Please confirm your account by <a href=\"{link}\">clicking here</a>.");
        }

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if(!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Unauthorized(new ProblemDetails { Title = "Invalid email or password." });
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName!, request.Password, false, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            return Ok();
        }

        if (result.IsLockedOut)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ProblemDetails { Title = "Account is locked." });
        }

        if (result.IsNotAllowed)
        {
            var verified = await _userManager.IsEmailConfirmedAsync(user);
            var title = verified ? "Access denied." : "Account is not verified.";
            return StatusCode(StatusCodes.Status403Forbidden, new ProblemDetails { Title = title });
        }

        return Unauthorized(new ProblemDetails { Title = "Invalid email or password." });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet("confirm")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return BadRequest();

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded) return Ok();

        return BadRequest();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            return Ok();
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var link = Url.Action(nameof(ResetPassword), "Auth", new { email = request.Email, token }, Request.Scheme);
        await _emailSender.SendEmailAsync(request.Email, "Reset your password", $"Reset your password by <a href=\"{link}\">clicking here</a>.");
        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return BadRequest();

        if (request.Password != request.ConfirmPassword)
        {
            ModelState.AddModelError(nameof(request.ConfirmPassword), "Passwords do not match.");
            return ValidationProblem(ModelState);
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (result.Succeeded) return Ok();

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }
        return ValidationProblem(ModelState);
    }
}
