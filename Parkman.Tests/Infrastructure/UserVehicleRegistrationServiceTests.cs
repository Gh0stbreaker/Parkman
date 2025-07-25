using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Parkman.Domain.Entities;
using Parkman.Infrastructure;
using Parkman.Infrastructure.Repositories;
using Parkman.Infrastructure.Repositories.Entities;
using Parkman.Infrastructure.Services;
using Parkman.Shared.Enums;
using Xunit;

namespace Parkman.Tests.Infrastructure;

public class UserVehicleRegistrationServiceTests
{
    private static UserManager<ApplicationUser> CreateUserManager(ApplicationDbContext context)
    {
        var store = new UserStore<ApplicationUser>(context);
        var options = Options.Create(new IdentityOptions());
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        var userValidators = new[] { new UserValidator<ApplicationUser>() };
        var passwordValidators = new[] { new PasswordValidator<ApplicationUser>() };
        var normalizer = new UpperInvariantLookupNormalizer();
        var describer = new IdentityErrorDescriber();
        var services = new ServiceCollection().BuildServiceProvider();
        var logger = NullLogger<UserManager<ApplicationUser>>.Instance;
        return new UserManager<ApplicationUser>(store, options, passwordHasher, userValidators, passwordValidators, normalizer, describer, services, logger);
    }

    [Fact]
    public async Task RegisterAsync_creates_user_profile_and_vehicle()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        using var context = new ApplicationDbContext(options);
        var userManager = CreateUserManager(context);
        var personRepo = new PersonProfileRepository(context, NullLogger<GenericRepository<PersonProfile>>.Instance);
        var vehicleRepo = new VehicleRepository(context, NullLogger<GenericRepository<Vehicle>>.Instance);
        var companyRepo = new CompanyProfileRepository(context, NullLogger<GenericRepository<CompanyProfile>>.Instance);
        var service = new UserVehicleRegistrationService(userManager, personRepo, vehicleRepo, companyRepo);

        var result = await service.RegisterAsync(
            "user@example.com",
            "Password1!",
            "John",
            "Doe",
            DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
            "123456789",
            "Some Street",
            "ABC123",
            VehicleBrand.Tesla,
            VehicleType.Car,
            VehiclePropulsionType.Electric);

        Assert.True(result.Succeeded);
        Assert.Equal(1, context.Users.Count());
        Assert.Equal(1, context.PersonProfiles.Count());
        Assert.Equal(1, context.Vehicles.Count());
    }

    [Fact]
    public async Task RegisterAsync_duplicate_license_plate_returns_error()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        using var context = new ApplicationDbContext(options);
        context.Vehicles.Add(new Vehicle("ABC123", VehicleBrand.Tesla, VehicleType.Car, VehiclePropulsionType.Electric));
        await context.SaveChangesAsync();

        var userManager = CreateUserManager(context);
        var personRepo = new PersonProfileRepository(context, NullLogger<GenericRepository<PersonProfile>>.Instance);
        var vehicleRepo = new VehicleRepository(context, NullLogger<GenericRepository<Vehicle>>.Instance);
        var companyRepo = new CompanyProfileRepository(context, NullLogger<GenericRepository<CompanyProfile>>.Instance);
        var service = new UserVehicleRegistrationService(userManager, personRepo, vehicleRepo, companyRepo);

        var result = await service.RegisterAsync(
            "user@example.com",
            "Password1!",
            "John",
            "Doe",
            DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
            "123456789",
            "Some Street",
            "ABC123",
            VehicleBrand.Tesla,
            VehicleType.Car,
            VehiclePropulsionType.Electric);

        Assert.False(result.Succeeded);
        Assert.Contains(result.Errors, e => e.Code == "DuplicateLicensePlate");
    }
}
