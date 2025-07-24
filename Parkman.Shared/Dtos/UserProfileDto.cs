namespace Parkman.Dtos;

public class UserProfileDto
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? LicensePlate { get; set; }
    public string? Brand { get; set; }
    public string? VehicleType { get; set; }
    public string? PropulsionType { get; set; }
}
