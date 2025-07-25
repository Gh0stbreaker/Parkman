using System.ComponentModel.DataAnnotations;
using Parkman.Shared.Enums;

namespace Parkman.Shared.Models;

public class CreateParkingSpotRequest
{
    [Required]
    public string Label { get; set; } = string.Empty;

    [Required]
    public ParkingSpotType Type { get; set; }

    [Required]
    public ParkingSpotAccessibility Accessibility { get; set; }

    [Required]
    public ParkingSpotAllowedPropulsionType AllowedPropulsion { get; set; }
}
