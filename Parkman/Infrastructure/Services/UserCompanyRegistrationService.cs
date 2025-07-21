using Microsoft.AspNetCore.Identity;
using Parkman.Domain.Entities;
using Parkman.Domain.Enums;
using Parkman.Infrastructure.Repositories.Entities;

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
        bool shareable = false);
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
        bool shareable = false)
    {
        var user = new ApplicationUser { UserName = email, Email = email };
        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            return createResult;
        }

        var profile = new CompanyProfile(companyName, ico, dic, contactPersonName, contactEmail, phoneNumber, billingAddress);
        user.SetCompanyProfile(profile);
        await _companyRepo.AddAsync(profile);

        var vehicle = new Vehicle(licensePlate, brand, type, propulsionType, shareable);
        profile.AddVehicle(vehicle);
        await _vehicleRepo.AddAsync(vehicle);

        return createResult;
    }
}
