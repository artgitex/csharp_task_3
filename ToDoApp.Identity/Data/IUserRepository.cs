using AuthenticationServer.Models;
using ToDoApp.Identity.Models;

namespace ToDoApp.Identity.Data;

public interface IUserRepository
{
    public Task<UserProfile> GetByEmailAsync(string email);
}
