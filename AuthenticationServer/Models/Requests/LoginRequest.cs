using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.Models.Requests;

public class LoginRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
