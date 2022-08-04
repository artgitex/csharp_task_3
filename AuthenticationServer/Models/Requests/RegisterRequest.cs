using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.Models.Requests;

public class RegisterRequest
{
    [Required]    
    public string Email { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
}
