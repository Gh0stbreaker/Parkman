using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parkman.Shared.Entities;
using Parkman.Infrastructure.Repositories.Entities;

namespace Parkman.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompanyController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPersonProfileRepository _personRepository;

    public CompanyController(
        UserManager<ApplicationUser> userManager,
        IPersonProfileRepository personRepository)
    {
        _userManager = userManager;
        _personRepository = personRepository;
    }

    [HttpPost("approve/{personUserId}")]
    public async Task<IActionResult> ApproveMember(string personUserId)
    {
        var userId = _userManager.GetUserId(User);
        var user = await _userManager.Users
            .Include(u => u.CompanyProfile)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user?.CompanyProfile == null)
            return Forbid();

        var person = await _personRepository.GetByIdAsync(personUserId);
        if (person == null)
            return NotFound();

        if (person.CompanyProfileUserId != user.Id)
            return BadRequest();

        person.ApproveCompanyMembership();
        await _personRepository.UpdateAsync(person);

        return Ok();
    }
}
