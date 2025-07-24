using Microsoft.AspNetCore.Identity;
using Parkman.Domain.Entities;
using Parkman.Domain.Enums;
using Parkman.Infrastructure.Repositories.Entities;

namespace Parkman.Infrastructure.Services;

public interface IUserVehicleRegistrationService
{
    Task<IdentityResult> RegisterAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        DateOnly? dateOfBirth,
        string phoneNumber,
        string address,
        string licensePlate,
        VehicleBrand brand,
        VehicleType type,
        VehiclePropulsionType propulsionType,
        bool shareable = false);
}

public class UserVehicleRegistrationService : IUserVehicleRegistrationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPersonProfileRepository _personRepo;
    private readonly IVehicleRepository _vehicleRepo;

    public UserVehicleRegistrationService(
        UserManager<ApplicationUser> userManager,
        IPersonProfileRepository personRepo,
        IVehicleRepository vehicleRepo)
    {
        _userManager = userManager;
        _personRepo = personRepo;
        _vehicleRepo = vehicleRepo;
    }

    public async Task<IdentityResult> RegisterAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        DateOnly? dateOfBirth,
        string phoneNumber,
        string address,
        string licensePlate,
        VehicleBrand brand,
        VehicleType type,
        VehiclePropulsionType propulsionType,
        bool shareable = false)
    {
        using var transaction = await _personRepo.BeginTransactionAsync();

        var user = new ApplicationUser { UserName = email, Email = email };
        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            await transaction.RollbackAsync();
            return createResult;
        }

        try
        {
            var profile = new PersonProfile(firstName, lastName, dateOfBirth, phoneNumber, address);
            user.SetPersonProfile(profile);
            await _personRepo.AddAsync(profile);

            var vehicle = new Vehicle(licensePlate, brand, type, propulsionType, shareable);
            profile.SetVehicle(vehicle);
            await _vehicleRepo.AddAsync(vehicle);

            await transaction.CommitAsync();
            return createResult;
        }
        catch
        {
            await transaction.RollbackAsync();
            await _userManager.DeleteAsync(user);
            throw;
        }
    }
}
