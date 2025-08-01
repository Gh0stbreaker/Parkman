using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parkman.Shared.Enums;
using Parkman.Shared.Dtos;
using Parkman.Infrastructure.Repositories.Entities;
using Parkman.Shared.Models;
using Parkman.Domain.Entities;

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
        var userId = _userManager.GetUserId(User);
        var user = await _userManager.Users
            .Include(u => u.PersonProfile!)
                .ThenInclude(p => p.Vehicle)
            .Include(u => u.CompanyProfile)
            .FirstOrDefaultAsync(u => u.Id == userId);
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
        var roles = await _userManager.GetRolesAsync(user);

        if (user.PersonProfile != null)
        {
            var profile = user.PersonProfile;
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

        var dto = new UserProfileDto
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            LicensePlate = licensePlate,
            Brand = brand?.ToString(),
            VehicleType = vehicleType?.ToString(),
            PropulsionType = propulsionType?.ToString(),
            Roles = roles.ToList()
        };

        return Ok(dto);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var userId = _userManager.GetUserId(User);
        var user = await _userManager.Users
            .Include(u => u.PersonProfile!)
                .ThenInclude(p => p.Vehicle)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return Unauthorized();

        if (user.PersonProfile == null)
            return BadRequest();

        var profile = user.PersonProfile;

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
