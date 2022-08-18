using ToDoApp.Web.Models;

namespace ToDoApp.Web.Data;

public interface IUserRepository
{
    Task<UserProfile> CreateAsync(UserProfile userProfile);
    Task<UserProfile> GetByEmailAsync(string email);   
}
