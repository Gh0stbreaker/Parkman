using System.ComponentModel.DataAnnotations;

namespace Parkman.Shared.Models;

public class RegisterRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
