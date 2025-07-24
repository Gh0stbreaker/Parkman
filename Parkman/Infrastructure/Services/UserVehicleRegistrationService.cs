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
        bool shareable = false,
        string? companyEmail = null,
        string? pairingPassword = null);
}

public class UserVehicleRegistrationService : IUserVehicleRegistrationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPersonProfileRepository _personRepo;
    private readonly IVehicleRepository _vehicleRepo;
    private readonly ICompanyProfileRepository _companyRepo;

    public UserVehicleRegistrationService(
        UserManager<ApplicationUser> userManager,
        IPersonProfileRepository personRepo,
        IVehicleRepository vehicleRepo,
        ICompanyProfileRepository companyRepo)
    {
        _userManager = userManager;
        _personRepo = personRepo;
        _vehicleRepo = vehicleRepo;
        _companyRepo = companyRepo;
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
        bool shareable = false,
        string? companyEmail = null,
        string? pairingPassword = null)
    {
        var existingVehicle = await _vehicleRepo.GetByLicensePlateAsync(licensePlate);
        if (existingVehicle != null)
        {
            if (existingVehicle.CompanyProfileUserId == null || !existingVehicle.IsShareable)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateLicensePlate",
                    Description = "Vehicle with this license plate already exists."
                });
            }

            if (string.IsNullOrWhiteSpace(companyEmail))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "CompanyRequired",
                    Description = "Company email required for existing company vehicle."
                });
            }

            var companyUser = await _userManager.FindByEmailAsync(companyEmail);
            if (companyUser == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "CompanyNotFound",
                    Description = "Specified company not found."
                });
            }

            var companyProfile = await _companyRepo.GetByIdAsync(companyUser.Id);
            if (companyProfile == null || existingVehicle.CompanyProfileUserId != companyProfile.UserId)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "VehicleCompanyMismatch",
                    Description = "Vehicle does not belong to specified company."
                });
            }
        }

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

            Vehicle vehicle;
            if (existingVehicle == null)
            {
                vehicle = new Vehicle(licensePlate, brand, type, propulsionType, shareable);
                profile.SetVehicle(vehicle);
                await _vehicleRepo.AddAsync(vehicle);
            }
            else
            {
            var companyUser = await _userManager.FindByEmailAsync(companyEmail!);
            var companyProfile = await _companyRepo.GetByIdAsync(companyUser!.Id);

            if (existingVehicle.PairingPassword != pairingPassword)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidPairingPassword",
                    Description = "Invalid pairing password."
                });
            }

            companyProfile!.AddMember(profile);
            profile.SetVehicle(existingVehicle);
            await _vehicleRepo.UpdateAsync(existingVehicle);
            await _personRepo.UpdateAsync(profile);
            await _companyRepo.UpdateAsync(companyProfile);
            }

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
