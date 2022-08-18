using Microsoft.EntityFrameworkCore;
using ToDoApp.Web.Models;

namespace ToDoApp.Web.DbContexts;

public class ToDoDbContext : DbContext
{
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
    {
            
    }

    public DbSet<UserProfile> UserProfile { get; set; }    
    public DbSet<ToDoItem> ToDoItems { get; set; }    
}
