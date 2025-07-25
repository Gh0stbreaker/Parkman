using System;
using Microsoft.AspNetCore.Identity;

namespace Parkman.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public PersonProfile? PersonProfile { get; private set; }
    public CompanyProfile? CompanyProfile { get; private set; }

    public void SetPersonProfile(PersonProfile profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile));
        if (CompanyProfile != null)
        {
            throw new InvalidOperationException("Cannot set person profile when company profile exists.");
        }
        PersonProfile = profile;
        profile.SetUser(this);
    }

    public void SetCompanyProfile(CompanyProfile profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile));
        if (PersonProfile != null)
        {
            throw new InvalidOperationException("Cannot set company profile when person profile exists.");
        }
        CompanyProfile = profile;
        profile.SetUser(this);
    }
}

