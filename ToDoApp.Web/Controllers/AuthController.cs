using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ToDoApp.Web.Data;
using ToDoApp.Web.Models;
using ToDoApp.Web.Models.Requests;
using ToDoApp.Web.Service;

namespace ToDoApp.Web.Controllers;

public class AuthController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;    

    Uri baseAddress = new Uri("https://localhost:7275/login");
    HttpClient client;

    public AuthController(IUserRepository userRepository, PasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;

        client = new HttpClient();
        client.BaseAddress = baseAddress;
    }

    public IActionResult Index(string? msg)
    {
        LoginRequest loginRequest = new LoginRequest();
        if (msg != null)
            loginRequest.Message = msg;

        return View(loginRequest);
    }
    public IActionResult SendLoginData(string email, string password)
    {        
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

                return RedirectToAction("Index", "ToDo");
            }
        }
        catch (Exception ex)
        { 
        }
        
        return RedirectToAction("Index", new { msg = "Email/Password is incorrect!"});
    }

    public IActionResult UserProfile()
    {
        return View("Register");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(PhotoUpload fileObj)
    {
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
            return BadRequest();
        }

        UserProfile existingUserByEmail = await _userRepository.GetByEmailAsync(oRegisterRequest.Email);
        if (existingUserByEmail != null)
        {
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

        return Json(new { success = true });
    }

    public RedirectResult RedirectHome()
    {
        return Redirect("/Home/Index");
    }
}
