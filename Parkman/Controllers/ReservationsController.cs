using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Parkman.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(Array.Empty<object>());

    [HttpPost]
    public IActionResult Create() => Ok();
}
