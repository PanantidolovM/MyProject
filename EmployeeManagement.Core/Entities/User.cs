namespace EmployeeManagement.Core.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; }  
    public string Role { get; set; } = "User";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public User(string email, string password, string role, DateTime createdAt,DateTime updatedAt )
    {
        Email = email;
        Password = password;
        Role = role;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}
