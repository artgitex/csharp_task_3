using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.DbContexts;
using ToDoApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ToDoDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<IToDoRepository, ToDoRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<RazorRenderService>();

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    using (ToDoDbContext context = scope.ServiceProvider.GetRequiredService<ToDoDbContext>())
    {
        context.Database.EnsureCreated();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
