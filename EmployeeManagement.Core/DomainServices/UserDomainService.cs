
using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Core.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
namespace EmployeeManagement.Core.DomainServices;

public class UserDomainService
{
    private readonly IUserRepository _userRepository;

    public UserDomainService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Authenticate(string email, string password)
    {
        // Get user by email
        User user = await _userRepository.GetUserByEmail(email);
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
        // Check if the password contains only letters and numbers
        if (!Regex.IsMatch(password, @"^[a-zA-Z0-9]+$"))
        {
            throw new Exception("Password can only contain letters and numbers");
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
        // Check if the password contains at least one digit
        if (!Regex.IsMatch(password, @"^(?=.*[0-9]).+$"))
        {
            throw new Exception("Password must contain at least one digit");
        }
        // Check if the password contains at least one space
        if (Regex.IsMatch(password, @"\s"))
        {
            throw new Exception("Password cannot contain spaces");
        }
        // Check if the password contains at least one tab
        if (Regex.IsMatch(password, @"\t"))
        {
            throw new Exception("Password cannot contain tabs");
        }
        // Check if the password contains at least one newline
        if (Regex.IsMatch(password, @"\n"))
        {
            throw new Exception("Password cannot contain newlines");
        }
        // Check if the password contains at least one carriage return
        if (Regex.IsMatch(password, @"\r"))
        {
            throw new Exception("Password cannot contain carriage returns");
        }
        // Check if the password contains at least one form feed
        if (Regex.IsMatch(password, @"\f"))
        {
            throw new Exception("Password cannot contain form feeds");
        }
        // Check if the password contains at least one vertical tab
        if (Regex.IsMatch(password, @"\v"))
        {
            throw new Exception("Password cannot contain vertical tabs");
        }
        // Check if the password contains at least one bell character
        if (Regex.IsMatch(password, @"\a"))
        {
            throw new Exception("Password cannot contain bell characters");
        }
        // Check if the password contains at least one backspace character
        if (Regex.IsMatch(password, @"\b"))
        {
            throw new Exception("Password cannot contain backspace characters");
        }
        // Check if the password contains at least one escape character
        if (Regex.IsMatch(password, @"\e"))
        {
            throw new Exception("Password cannot contain escape characters");
        }
        // Check if the password contains at least one delete character
        if (Regex.IsMatch(password, @"\x7F"))
        {
            throw new Exception("Password cannot contain delete characters");
        }
        // Check if the password contains at least one null character
        if (Regex.IsMatch(password, @"\0"))
        {
            throw new Exception("Password cannot contain null characters");
        }
        
        // Check password with saved PasswordHash 
        // Note: In a real application, you should not return the password hash
        // for security reasons. Instead, you should return a token or some other form of authentication.
        var passwordHash = ComputeHash(password);
        if (user.PasswordHash == passwordHash)  // If the password is correct, return the user
        {
            return user;
        }
        else
        {
            throw new Exception("Invalid password");
        }
    }
    
    private string ComputeHash(string input)
    {
        // Ví dụ sử dụng SHA256 (các hệ thống thực tế có thể dùng thuật toán phức tạp hơn hoặc kết hợp salt)
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}

