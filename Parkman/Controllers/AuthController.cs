using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parkman.Domain.Entities;
using Parkman.Models;

namespace Parkman.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if(!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);
        if(!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(ModelState);
        }
        await _signInManager.SignInAsync(user, isPersistent: false);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if(!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
        if(result.Succeeded)
        {
            return Ok();
        }
        return Unauthorized();
    }
}
