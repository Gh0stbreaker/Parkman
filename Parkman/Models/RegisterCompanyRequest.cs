namespace Parkman.Models;

public class RegisterCompanyRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;
    public string Ico { get; set; } = string.Empty;
    public string Dic { get; set; } = string.Empty;
    public string ContactPersonName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string BillingAddress { get; set; } = string.Empty;
}
