using System.ComponentModel.DataAnnotations;
using Parkman.Shared.Enums;

namespace Parkman.Shared.Models;

public class RegisterCompanyRequest
{
    [Required(ErrorMessage = "Email is required."), EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required."),
     StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long."),
     RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
        ErrorMessage = "Password must contain uppercase, lowercase, number and special character.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please confirm password."),
     Compare(nameof(Password), ErrorMessage = "Passwords do not match."),
     StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Company name is required."), StringLength(100)]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "IČO is required."), RegularExpression(@"^\d{8}$", ErrorMessage = "IČO must have 8 digits.")]
    public string Ico { get; set; } = string.Empty;

    public string Dic { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contact person name is required."), StringLength(100)]
    public string ContactPersonName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contact email is required."), EmailAddress(ErrorMessage = "Invalid email address.")]
    public string ContactEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required."), Phone(ErrorMessage = "Invalid phone number.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Billing address is required."), StringLength(200)]
    public string BillingAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "License plate is required.")]
    public string LicensePlate { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vehicle brand is required.")]
    public VehicleBrand Brand { get; set; }

    [Required(ErrorMessage = "Vehicle type is required.")]
    public VehicleType Type { get; set; }

    [Required(ErrorMessage = "Propulsion type is required.")]
    public VehiclePropulsionType PropulsionType { get; set; }

    public bool Shareable { get; set; }
}
