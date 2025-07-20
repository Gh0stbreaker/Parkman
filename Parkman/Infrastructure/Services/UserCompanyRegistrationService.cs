using Microsoft.AspNetCore.Identity;
using Parkman.Domain.Entities;
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
        string billingAddress);
}

public class UserCompanyRegistrationService : IUserCompanyRegistrationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICompanyProfileRepository _companyRepo;

    public UserCompanyRegistrationService(
        UserManager<ApplicationUser> userManager,
        ICompanyProfileRepository companyRepo)
    {
        _userManager = userManager;
        _companyRepo = companyRepo;
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
        string billingAddress)
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

        return createResult;
    }
}
