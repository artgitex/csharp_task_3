using AuthenticationServer.Models;
using ToDoApp.Identity.Models;

namespace AuthenticationServer.Services.UserRepository;

public interface IUserRepository
{    
    public Task<UserProfile> GetByEmailAsync(string email); 
}
