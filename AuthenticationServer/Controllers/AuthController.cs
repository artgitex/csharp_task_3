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
    private readonly TokenGenerator _tokenGenerator;

    public AuthController(IUserRepository userRepository, PasswordHasher passwordHasher, TokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest registerRequest)
    {

        if (registerRequest.Password != registerRequest.ConfirmPassword)
        {
            return BadRequest();
        }

        User existingUserByEmail = _userRepository.GetByEmail(registerRequest.Email);
        if (existingUserByEmail != null)
        {
            return Conflict();
        }

        User existingUserByUsername = _userRepository.GetByUserName(registerRequest.Username);
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

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {       

        User user = _userRepository.GetByUserName(loginRequest.Username);
        if (user == null)
        {
            return Unauthorized();
        }

        bool isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);
        if (!isCorrectPassword)
        {
            return Unauthorized();
        }

        string accessToken = _tokenGenerator.GenerateToken(user);

        return Ok(accessToken);
    }
}
