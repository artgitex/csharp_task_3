using AuthenticationServer.Models;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Identity.Models;

namespace AuthenticationServer.DbContexts;

public class AuthDbContext : DbContext
{    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=mydb;Trusted_Connection=True;");
    }

    public DbSet<UserProfile> UserProfile { get; set; }
}
