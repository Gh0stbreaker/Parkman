using System;

namespace Parkman.Domain;

public class CompanyProfile
{
    public string UserId { get; private set; } = null!;
    public ApplicationUser User { get; private set; } = null!;

    public string CompanyName { get; private set; } = string.Empty;
    public string Ico { get; private set; } = string.Empty;
    public string Dic { get; private set; } = string.Empty;

    private CompanyProfile() { }

    public CompanyProfile(string companyName, string ico, string dic)
    {
        Update(companyName, ico, dic);
    }

    public void Update(string companyName, string ico, string dic)
    {
        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name is required", nameof(companyName));
        if (string.IsNullOrWhiteSpace(ico))
            throw new ArgumentException("ICO is required", nameof(ico));
        if (string.IsNullOrWhiteSpace(dic))
            throw new ArgumentException("DIC is required", nameof(dic));

        CompanyName = companyName;
        Ico = ico;
        Dic = dic;
    }

    internal void SetUser(ApplicationUser user)
    {
        User = user;
        UserId = user.Id;
    }
}
