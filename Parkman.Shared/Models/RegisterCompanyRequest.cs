using System.ComponentModel.DataAnnotations;
using Parkman.Domain.Enums;

namespace Parkman.Models;

public class RegisterCompanyRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required, Compare(nameof(Password)), StringLength(100, MinimumLength = 6)]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string CompanyName { get; set; } = string.Empty;

    [Required, RegularExpression(@"^\d{8}$", ErrorMessage = "IČO must have 8 digits.")]
    public string Ico { get; set; } = string.Empty;

    public string Dic { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string ContactPersonName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string ContactEmail { get; set; } = string.Empty;

    [Required, Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string BillingAddress { get; set; } = string.Empty;

    [Required]
    public string LicensePlate { get; set; } = string.Empty;

    [Required]
    public VehicleBrand Brand { get; set; }

    [Required]
    public VehicleType Type { get; set; }

    [Required]
    public VehiclePropulsionType PropulsionType { get; set; }

    public bool Shareable { get; set; }
}
