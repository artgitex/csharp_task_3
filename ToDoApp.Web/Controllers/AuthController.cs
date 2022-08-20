using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using ToDoApp.Web.Data;
using ToDoApp.Web.Models;
using ToDoApp.Web.Models.Requests;
using ToDoApp.Web.Service;
using ToDoApp.Web.Service.Logger;

namespace ToDoApp.Web.Controllers;

public class AuthController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ILoggerService _logger;
    private readonly PasswordHasher _passwordHasher;    

    Uri baseAddress = new Uri("https://localhost:7275/login");
    HttpClient client;

    public AuthController(IUserRepository userRepository, PasswordHasher passwordHasher, ILoggerService logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;

        client = new HttpClient();
        client.BaseAddress = baseAddress;
        _logger = logger;
    }

    public IActionResult Index(string? msg)
    {
        LoginRequest loginRequest = new LoginRequest();
        if (msg != null)
            loginRequest.Message = msg;

        return View(loginRequest);
    }
    public IActionResult Login(string email, string password)
    {
        _logger.LogInfo("Sending Authenticating data - start");

        LoginRequest login = new LoginRequest();
        login.Email = email;
        login.Password = password;

        string data = JsonConvert.SerializeObject(login);
        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = client.PostAsync(client.BaseAddress, content).Result;
            if (response.IsSuccessStatusCode)
            {
                var token = response.Content.ReadAsStringAsync().Result;
                HttpContext.Session.SetString("Token", token);

                _logger.LogInfo("Sending Authenticating data - token received and saved");
                _logger.LogInfo("Sending Authenticating data - end");
                return RedirectToAction("Index", "ToDo");
            }
        }
        catch (Exception ex)
        { 
        }

        _logger.LogError("Sending Authenticating data - token is not received");
        return RedirectToAction("Index", new { msg = "Email/Password is incorrect!"});
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("Token");
        return RedirectToAction("Index", "Auth");
    }

    public async Task<IActionResult> UserProfile(int id = 0)
    {
        if (id == 0)
        {
            return View("Register");            
        }
        else
        {
            string idUser = HttpContext.User.FindFirstValue("id");
            UserProfile userProfile = await _userRepository.GetByIdAsync(int.Parse(idUser));
            RegisterRequest registerRequest = new RegisterRequest() {
                Name = userProfile.Name,
                Email = userProfile.Email,
                Age = userProfile.Age,
                EmploymentDate = userProfile.EmploymentDate,
                Photo = userProfile.Photo
            };
            return View("Register", registerRequest);            
        }        
    }       

    [HttpPost("register")]
    public async Task<IActionResult> Register(PhotoUpload fileObj)
    {
        _logger.LogInfo("Register new user - start");

        RegisterRequest oRegisterRequest = JsonConvert.DeserializeObject<RegisterRequest>(fileObj.RegisterRequest);

        if (fileObj.file != null)
        {
            using (var ms = new MemoryStream())
            {
                fileObj.file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                oRegisterRequest.Photo = fileBytes;
            }
        }
        
        if (oRegisterRequest.Password != oRegisterRequest.ConfirmPassword)
        {
            _logger.LogError("Register new user - Password and Confirm Password is not equal");
            return BadRequest();
        }

        UserProfile existingUserByEmail = await _userRepository.GetByEmailAsync(oRegisterRequest.Email);
        if (existingUserByEmail != null)
        {
            _logger.LogError("Register new user - User already exist");
            return Conflict();
        }

        string passwordHash = _passwordHasher.HashPassword(oRegisterRequest.Password);

        UserProfile registrationUser = new UserProfile()
        {
            Name = oRegisterRequest.Name,
            Email = oRegisterRequest.Email,
            PasswordHash = passwordHash,
            Age = oRegisterRequest.Age,
            EmploymentDate = oRegisterRequest.EmploymentDate,
            Photo = oRegisterRequest.Photo
        };

        await _userRepository.CreateAsync(registrationUser);
        _logger.LogInfo("Register new user - end");
        return Json(new { success = true });
    }

    public RedirectResult RedirectHome()
    {
        return Redirect("/Home/Index");
    }
}
