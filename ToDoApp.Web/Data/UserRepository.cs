using Microsoft.EntityFrameworkCore;
using ToDoApp.Web.DbContexts;
using ToDoApp.Web.Models;

namespace ToDoApp.Web.Data;

public class UserRepository : IUserRepository
{
    private readonly ToDoDbContext _context;

    public UserRepository(ToDoDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfile> CreateAsync(UserProfile userProfile)
    {
        _context.UserProfile.Add(userProfile);
        await _context.SaveChangesAsync();

        return userProfile;
    }

    public async Task<UserProfile> GetByEmailAsync(string email)
    {
        return await _context.UserProfile.FirstOrDefaultAsync(u => u.Email == email);
    }
}
