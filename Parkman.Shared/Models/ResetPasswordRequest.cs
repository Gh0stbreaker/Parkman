using System.ComponentModel.DataAnnotations;

namespace Parkman.Shared.Models;

public class ResetPasswordRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Token { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required, Compare(nameof(Password)), StringLength(100, MinimumLength = 6)]
    public string ConfirmPassword { get; set; } = string.Empty;
}
