using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Parkman.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        // Return minimal profile info from authenticated user
        var email = User.Identity?.Name ?? "unknown";
        return Ok(new { Email = email, FirstName = "", LastName = "" });
    }
}
