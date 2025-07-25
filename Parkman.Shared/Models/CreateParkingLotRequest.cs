using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parkman.Shared.Models;

public class CreateParkingLotRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    public List<CreateParkingSpotRequest> Spots { get; set; } = new();
}
