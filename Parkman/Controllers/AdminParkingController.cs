using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parkman.Domain.Entities;
using Parkman.Infrastructure.Services.Entities;
using Parkman.Shared.Models;
using System.Linq;

namespace Parkman.Controllers;

[ApiController]
[Route("api/admin/parking")]
[Authorize(Roles = "Administrator")]
public class AdminParkingController : ControllerBase
{
    private readonly IParkingLotService _lotService;
    private readonly IParkingSpotService _spotService;

    public AdminParkingController(IParkingLotService lotService, IParkingSpotService spotService)
    {
        _lotService = lotService;
        _spotService = spotService;
    }

    [HttpPost("lots")]
    public async Task<IActionResult> CreateLot(CreateParkingLotRequest request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var lot = new ParkingLot(request.Name, request.Address);
        foreach (var spotRequest in request.Spots)
        {
            var spot = new ParkingSpot(spotRequest.Label, spotRequest.Type, spotRequest.Accessibility, spotRequest.AllowedPropulsion);
            lot.AddSpot(spot);
        }

        await _lotService.AddAsync(lot);
        return CreatedAtAction(nameof(GetLot), new { id = lot.Id }, new { lot.Id });
    }

    [HttpGet("lots/{id}")]
    public async Task<IActionResult> GetLot(int id)
    {
        var lot = await _lotService.GetByIdAsync(id);
        if (lot == null) return NotFound();

        var result = new
        {
            lot.Id,
            lot.Name,
            lot.Address,
            Spots = lot.Spots.Select(s => new
            {
                s.Id,
                s.Label,
                s.Type,
                s.Accessibility,
                s.AllowedPropulsion
            })
        };
        return Ok(result);
    }

    [HttpPost("lots/{lotId}/spots")]
    public async Task<IActionResult> AddSpot(int lotId, CreateParkingSpotRequest request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var lot = await _lotService.GetByIdAsync(lotId);
        if (lot == null) return NotFound();

        var spot = new ParkingSpot(request.Label, request.Type, request.Accessibility, request.AllowedPropulsion);
        lot.AddSpot(spot);
        await _spotService.AddAsync(spot);
        return CreatedAtAction(nameof(GetLot), new { id = lotId }, new { spot.Id });
    }
}
