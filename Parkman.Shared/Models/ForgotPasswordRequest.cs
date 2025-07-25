using System.ComponentModel.DataAnnotations;

namespace Parkman.Shared.Models;

public class ForgotPasswordRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
}
