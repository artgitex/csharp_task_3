using AuthenticationServer.Models;

namespace AuthenticationServer.Services.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public Task<User> Create(User user)
        {
            user.Id = Guid.NewGuid();
            _users.Add(user);

            return Task.FromResult(user);
        }

        public Task<User> GetByEmail(string email)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Email == email));
        }

        public Task<User> GetByUserName(string username)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Username == username));
        }
    }
}
