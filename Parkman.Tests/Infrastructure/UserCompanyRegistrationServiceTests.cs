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

public class UserCompanyRegistrationServiceTests
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
    public async Task RegisterAsync_duplicate_license_plate_returns_error()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        using var context = new ApplicationDbContext(options);
        context.Vehicles.Add(new Vehicle("COMP1", VehicleBrand.Tesla, VehicleType.Car, VehiclePropulsionType.Electric));
        await context.SaveChangesAsync();

        var userManager = CreateUserManager(context);
        var companyRepo = new CompanyProfileRepository(context, NullLogger<GenericRepository<CompanyProfile>>.Instance);
        var vehicleRepo = new VehicleRepository(context, NullLogger<GenericRepository<Vehicle>>.Instance);
        var service = new UserCompanyRegistrationService(userManager, companyRepo, vehicleRepo);

        var result = await service.RegisterAsync(
            "company@example.com",
            "Password1!",
            "Comp",
            "12345678",
            "999",
            "Boss",
            "boss@example.com",
            "123456789",
            "Billing",
            "COMP1",
            VehicleBrand.Tesla,
            VehicleType.Car,
            VehiclePropulsionType.Electric);

        Assert.False(result.Succeeded);
        Assert.Contains(result.Errors, e => e.Code == "DuplicateLicensePlate");
    }
}
