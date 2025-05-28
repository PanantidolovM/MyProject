public class LoginModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string Email { get; set; } = string.Empty;
        [System.ComponentModel.DataAnnotations.Required]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResult
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }