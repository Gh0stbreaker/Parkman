using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parkman.Domain.Entities;
using Parkman.Domain.Enums;
using Parkman.Infrastructure.Repositories.Entities;
using Parkman.Models;

namespace Parkman.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPersonProfileRepository _personProfileRepository;
    private readonly ICompanyProfileRepository _companyProfileRepository;
    private readonly IVehicleRepository _vehicleRepository;

    public UserController(
        UserManager<ApplicationUser> userManager,
        IPersonProfileRepository personProfileRepository,
        ICompanyProfileRepository companyProfileRepository,
        IVehicleRepository vehicleRepository)
    {
        _userManager = userManager;
        _personProfileRepository = personProfileRepository;
        _companyProfileRepository = companyProfileRepository;
        _vehicleRepository = vehicleRepository;
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
        string? licensePlate = null;
        VehicleBrand? brand = null;
        VehicleType? vehicleType = null;
        VehiclePropulsionType? propulsionType = null;

        if (user.PersonProfile != null)
        {
            var profiles = await _personProfileRepository.ListAsync(p => p.UserId == user.Id, includeProperties: "Vehicle");
            var profile = profiles.FirstOrDefault();
            if (profile != null)
            {
                firstName = profile.FirstName;
                lastName = profile.LastName;
                if (profile.Vehicle != null)
                {
                    licensePlate = profile.Vehicle.LicensePlate;
                    brand = profile.Vehicle.Brand;
                    vehicleType = profile.Vehicle.Type;
                    propulsionType = profile.Vehicle.PropulsionType;
                }
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

        return Ok(new { Email = email, FirstName = firstName, LastName = lastName, LicensePlate = licensePlate, Brand = brand, VehicleType = vehicleType, PropulsionType = propulsionType });
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        if (user.PersonProfile == null)
            return BadRequest();

        var profiles = await _personProfileRepository.ListAsync(p => p.UserId == user.Id, includeProperties: "Vehicle");
        var profile = profiles.FirstOrDefault();
        if (profile == null)
            return NotFound();

        profile.Update(request.FirstName, request.LastName, profile.DateOfBirth, profile.PhoneNumber, profile.Address);

        if (profile.Vehicle == null)
        {
            var vehicle = new Vehicle(request.LicensePlate, request.Brand, request.Type, request.PropulsionType);
            profile.SetVehicle(vehicle);
            await _vehicleRepository.AddAsync(vehicle);
        }
        else
        {
            profile.Vehicle.Update(request.LicensePlate, request.Brand, request.Type, request.PropulsionType, profile.Vehicle.IsShareable);
            await _vehicleRepository.UpdateAsync(profile.Vehicle);
        }

        await _personProfileRepository.UpdateAsync(profile);
        return NoContent();
    }
}
