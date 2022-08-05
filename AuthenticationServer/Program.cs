using AuthenticationServer.Models;
using AuthenticationServer.Services;
using AuthenticationServer.Services.UserRepository;

var builder = WebApplication.CreateBuilder(args);

AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
ConfigurationManager configuration = builder.Configuration;
configuration.Bind("Authentication", authenticationConfiguration);
builder.Services.AddSingleton(authenticationConfiguration);

builder.Services.AddControllers();
builder.Services.AddSingleton<TokenGenerator>();
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

var app = builder.Build();

app.MapControllers();
app.Run();
