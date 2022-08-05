using AuthenticationServer.Models;

namespace AuthenticationServer.Services.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public User Create(User user)
        {
            user.Id = Guid.NewGuid();
            _users.Add(user);

            return user;
        }

        public User GetByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }

        public User GetByUserName(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }
    }
}