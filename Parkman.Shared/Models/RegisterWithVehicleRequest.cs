using System.ComponentModel.DataAnnotations;
using Parkman.Domain.Enums;

namespace Parkman.Models;

public class RegisterWithVehicleRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required, Compare(nameof(Password)), StringLength(100, MinimumLength = 6)]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public DateOnly? DateOfBirth { get; set; }

    [Required, Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    public string LicensePlate { get; set; } = string.Empty;

    [Required]
    public VehicleBrand Brand { get; set; }

    [Required]
    public VehicleType Type { get; set; }

    [Required]
    public VehiclePropulsionType PropulsionType { get; set; }

    public bool Shareable { get; set; }

    public string? CompanyEmail { get; set; }

    public string? PairingPassword { get; set; }
}
