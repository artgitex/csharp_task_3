using AuthenticationServer.DbContexts;
using AuthenticationServer.Models;
using AuthenticationServer.Services;
using AuthenticationServer.Services.UserRepository;

var builder = WebApplication.CreateBuilder(args);

AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
ConfigurationManager configuration = builder.Configuration;
configuration.Bind("Authentication", authenticationConfiguration);
builder.Services.AddSingleton(authenticationConfiguration);

builder.Services.AddDbContext<AuthDbContext>();

builder.Services.AddControllers();
builder.Services.AddSingleton<TokenGenerator>();
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    using (AuthDbContext context = scope.ServiceProvider.GetRequiredService<AuthDbContext>())
    {
        context.Database.EnsureCreated();
    }
}

app.MapControllers();
app.Run();
