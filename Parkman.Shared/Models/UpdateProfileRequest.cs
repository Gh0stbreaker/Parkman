using System.ComponentModel.DataAnnotations;
using Parkman.Shared.Enums;

namespace Parkman.Shared.Models;

public class UpdateProfileRequest
{
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public string LicensePlate { get; set; } = string.Empty;

    [Required]
    public VehicleBrand Brand { get; set; }

    [Required]
    public VehicleType Type { get; set; }

    [Required]
    public VehiclePropulsionType PropulsionType { get; set; }
}
