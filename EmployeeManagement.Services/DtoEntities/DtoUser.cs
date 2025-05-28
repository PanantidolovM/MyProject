namespace EmployeeManagement.Services.DtoEntities;

public class DtoUser
{
    public int Id { get; private set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DtoUser(string email, string password, string role, DateTime createdAt, DateTime updatedAt)
    {
        Email = email;
        Password = password;
        Role = role;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    // Ví dụ: phương thức nghiệp vụ để thay đổi email (và có thể bổ sung thay đổi Role theo nghiệp vụ)
    // public void ChangeEmail(string newEmail)
    // {
    //     // Thêm các validate nếu cần
    //     Email = newEmail;
    // }
}