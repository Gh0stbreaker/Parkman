using System.ComponentModel.DataAnnotations;
using Parkman.Shared.Enums;

namespace Parkman.Shared.Models;

public class RegisterWithVehicleRequest
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

    [Required(ErrorMessage = "First name is required."), StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required."), StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required.")]
    public DateOnly? DateOfBirth { get; set; }

    [Required(ErrorMessage = "Phone number is required."), Phone(ErrorMessage = "Invalid phone number.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "License plate is required.")]
    public string LicensePlate { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vehicle brand is required.")]
    public VehicleBrand Brand { get; set; }

    [Required(ErrorMessage = "Vehicle type is required.")]
    public VehicleType Type { get; set; }

    [Required(ErrorMessage = "Propulsion type is required.")]
    public VehiclePropulsionType PropulsionType { get; set; }

    public bool Shareable { get; set; }

    public string? CompanyEmail { get; set; }

    public string? PairingPassword { get; set; }
}
