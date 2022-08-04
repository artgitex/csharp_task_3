using AuthenticationServer.Models;

namespace AuthenticationServer.Services.UserRepository;

public interface IUserRepository
{
    Task<User> Create(User user);
    Task<User> GetByEmail(string email);
    Task<User> GetByUserName(string username);    
}
