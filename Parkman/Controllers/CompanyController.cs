using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parkman.Domain.Entities;
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
        var user = await _userManager.GetUserAsync(User);
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
