using AuthenticationServer.DbContexts;
using AuthenticationServer.Models;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Identity.Models;

namespace AuthenticationServer.Services.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;

        public UserRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile> GetByEmailAsync(string email)
        {
            return await _context.UserProfile.FirstOrDefaultAsync(u => u.Email == email);           
        }
         

        /*
        public async Task<User> Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChangesAsync();

            return user;
        }
       



        public async Task<User> GetByUserName(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);            
        }       
         */
    }
}