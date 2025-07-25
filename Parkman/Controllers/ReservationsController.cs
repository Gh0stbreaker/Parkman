using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Parkman.Infrastructure.Services.Entities;

namespace Parkman.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservationsController : ControllerBase
{
    private readonly IParkingSpotService _spotService;

    public ReservationsController(IParkingSpotService spotService)
    {
        _spotService = spotService;
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable(DateTime startTime, DateTime endTime)
    {
        if (endTime <= startTime)
            return BadRequest();

        var spots = await _spotService.ListAvailableAsync(startTime, endTime);
        var result = spots.Select(s => new { s.Id, s.Label, Lot = s.ParkingLot.Name });
        return Ok(result);
    }

    [HttpGet]
    public IActionResult Get() => Ok(Array.Empty<object>());

    [HttpPost]
    public IActionResult Create() => Ok();
}
