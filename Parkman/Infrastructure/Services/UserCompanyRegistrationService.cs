using Microsoft.AspNetCore.Identity;
using Parkman.Shared.Entities;
using Parkman.Shared.Enums;
using Parkman.Infrastructure.Repositories.Entities;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Services;

public interface IUserCompanyRegistrationService
{
    Task<IdentityResult> RegisterAsync(
        string email,
        string password,
        string companyName,
        string ico,
        string dic,
        string contactPersonName,
        string contactEmail,
        string phoneNumber,
        string billingAddress,
        string licensePlate,
        VehicleBrand brand,
        VehicleType type,
        VehiclePropulsionType propulsionType,
        bool shareable = false,
        string? pairingPassword = null);
}

public class UserCompanyRegistrationService : IUserCompanyRegistrationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICompanyProfileRepository _companyRepo;
    private readonly IVehicleRepository _vehicleRepo;

    public UserCompanyRegistrationService(
        UserManager<ApplicationUser> userManager,
        ICompanyProfileRepository companyRepo,
        IVehicleRepository vehicleRepo)
    {
        _userManager = userManager;
        _companyRepo = companyRepo;
        _vehicleRepo = vehicleRepo;
    }

    public async Task<IdentityResult> RegisterAsync(
        string email,
        string password,
        string companyName,
        string ico,
        string dic,
        string contactPersonName,
        string contactEmail,
        string phoneNumber,
        string billingAddress,
        string licensePlate,
        VehicleBrand brand,
        VehicleType type,
        VehiclePropulsionType propulsionType,
        bool shareable = false,
        string? pairingPassword = null)
    {
        if (await _vehicleRepo.LicensePlateExistsAsync(licensePlate))
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "DuplicateLicensePlate",
                Description = "Vehicle with this license plate already exists."
            });
        }

        using var transaction = await _companyRepo.BeginTransactionAsync();

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            PhoneNumber = phoneNumber
        };
        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            await transaction.RollbackAsync();
            return createResult;
        }

        try
        {
            var profile = new CompanyProfile(companyName, ico, dic, contactPersonName, contactEmail, phoneNumber, billingAddress);
            user.SetCompanyProfile(profile);
            await _companyRepo.AddAsync(profile);

            var vehicle = new Vehicle(licensePlate, brand, type, propulsionType, shareable, pairingPassword);
            profile.AddVehicle(vehicle);
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
