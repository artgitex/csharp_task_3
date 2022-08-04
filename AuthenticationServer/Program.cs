using AuthenticationServer.Services;
using AuthenticationServer.Services.UserRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

var app = builder.Build();

app.MapControllers();
app.Run();
