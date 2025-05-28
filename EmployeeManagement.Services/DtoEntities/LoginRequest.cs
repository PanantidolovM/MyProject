namespace EmployeeManagement.Services.DtoEntities;

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResult
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; } 
}