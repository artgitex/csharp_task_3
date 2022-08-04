using AuthenticationServer.Models;
using AuthenticationServer.Models.Requests;
using AuthenticationServer.Services;
using AuthenticationServer.Services.UserRepository;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationServer.Controllers;

public class AuthController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;

    public AuthController(IUserRepository userRepository, PasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        
        if (registerRequest.Password != registerRequest.ConfirmPassword)
        {
            return BadRequest();            
        }

        User existingUserByEmail = await _userRepository.GetByEmail(registerRequest.Email);
        if (existingUserByEmail != null)
        {
            return Conflict();            
        }

        User existingUserByUsername = await _userRepository.GetByUserName(registerRequest.Username);
        if (existingUserByUsername != null)
        {
            return Conflict();            
        }

        string passwordHash = _passwordHasher.HashPassword(registerRequest.Password);

        User registrationUser = new User()
        {
            Email = registerRequest.Email,
            Username = registerRequest.Username,
            PasswordHash = passwordHash
        };

        _userRepository.Create(registrationUser);

        return Ok();
    }
}
