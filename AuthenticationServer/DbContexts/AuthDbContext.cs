using AuthenticationServer.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationServer.DbContexts;

public class AuthDbContext : DbContext
{    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=peopledb;Trusted_Connection=True;");
    }

    public DbSet<User> Users { get; set; }
}
