using AuthenticationServer.Models;

namespace AuthenticationServer.Services.UserRepository;

public interface IUserRepository
{
    User Create(User user);
    User GetByEmail(string email);
    User GetByUserName(string username);
}
