using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parkman.Domain.Entities;
using Parkman.Infrastructure.Repositories.Entities;

namespace Parkman.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPersonProfileRepository _personProfileRepository;
    private readonly ICompanyProfileRepository _companyProfileRepository;

    public UserController(
        UserManager<ApplicationUser> userManager,
        IPersonProfileRepository personProfileRepository,
        ICompanyProfileRepository companyProfileRepository)
    {
        _userManager = userManager;
        _personProfileRepository = personProfileRepository;
        _companyProfileRepository = companyProfileRepository;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var email = user.Email ?? user.UserName ?? "unknown";

        string firstName = string.Empty;
        string lastName = string.Empty;

        if (user.PersonProfile != null)
        {
            var profile = await _personProfileRepository.GetByIdAsync(user.Id);
            if (profile != null)
            {
                firstName = profile.FirstName;
                lastName = profile.LastName;
            }
        }
        else if (user.CompanyProfile != null)
        {
            var profile = await _companyProfileRepository.GetByIdAsync(user.Id);
            if (profile != null)
            {
                var parts = profile.ContactPersonName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                firstName = parts.Length > 0 ? parts[0] : profile.ContactPersonName;
                lastName = parts.Length > 1 ? parts[1] : string.Empty;
            }
        }

        return Ok(new { Email = email, FirstName = firstName, LastName = lastName });
    }
}
