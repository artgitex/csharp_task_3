namespace AuthenticationServer.Services;

public class PasswordHasher
{    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
