using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Core.Entities;
using System.Text.RegularExpressions;
namespace EmployeeManagement.Core.DomainServices;

public class UserDomainService
{
    private readonly IUserRepository _userRepository;

    public UserDomainService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Authenticate(string email, string password)// Bmprao1234@
    {
        // Get user by email
        var user = await _userRepository.GetUserByEmail(email); // user from entity
        
        if (user == null)
        {
            throw new Exception("User not found");
        }

        // Check if the password is empty
        if (string.IsNullOrEmpty(password))
        {
            throw new Exception("Password cannot be empty");
        }
        // Check if the password is too short
        if (password.Length < 6)
        {
            throw new Exception("Password must be at least 6 characters long");
        }
        // Check if the password is too long
        if (password.Length > 20)
        {
            throw new Exception("Password must be less than 20 characters long");
        }
        // Check if the password contains at least one letter and one number
        if (!Regex.IsMatch(password, @"^(?=.*[a-zA-Z])(?=.*[0-9]).+$"))
        {
            throw new Exception("Password must contain at least one letter and one number");
        }
        // Check if the password contains at least one special character
        if (!Regex.IsMatch(password, @"^(?=.*[!@#$%^&*()_+={}\[\]:;""'<>?,./\\|`~]).+$"))
        {
            throw new Exception("Password must contain at least one special character");
        }
        // Check if the password contains at least one uppercase letter
        if (!Regex.IsMatch(password, @"^(?=.*[A-Z]).+$"))
        {
            throw new Exception("Password must contain at least one uppercase letter");
        }
        // Check if the password contains at least one lowercase letter
        if (!Regex.IsMatch(password, @"^(?=.*[a-z]).+$"))
        {
            throw new Exception("Password must contain at least one lowercase letter");
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

        if (!isPasswordValid)
        {
            throw new Exception("Invalid password");
        }
        else
        {
            return user;
        }     
    }
}

