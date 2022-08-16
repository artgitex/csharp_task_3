using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;

namespace ToDoApp.DbContexts;

public class ToDoDbContext : DbContext
{
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
    {

    }
    public DbSet<ToDoItem> ToDoItems { get; set; }
}
