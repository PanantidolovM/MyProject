using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.DomainServices;
using EmployeeManagement.Services.Interfaces;
using EmployeeManagement.Services.DtoEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using JsonSerializer = System.Text.Json.JsonSerializer;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace EmployeeManagement.Web.Controllers;

[ApiController]
[Route("api/auth")] // /api/auth
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserDomainService _userDomainService;
    private readonly IUserAsyncService _userAsyncService;
    private readonly ILogger<AuthController> _logger;
    public AuthController(IConfiguration configuration, UserDomainService userDomainService, IUserAsyncService userAsyncService, ILogger<AuthController> logger)
    {
        _configuration = configuration;
        _userDomainService = userDomainService;
        _userAsyncService = userAsyncService;
        _logger = logger;
    }

    [HttpPost("login")] // POST api/auth/login
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    /// <summary>
    /// Login method to authenticate user and generate JWT token.
    /// /// </summary>
    /// <param name="login">Login request containing email and password.</param>
    public async Task<IActionResult> Login([FromBody] LoginRequest login)
    {
        // Xác thực người dùng. (Ví dụ: kiểm tra email và mật khẩu)
        var user = await _userDomainService.Authenticate(login.Email, login.Password);
        if (user == null)
        {
            return Unauthorized("Email or password invalid.");
        }

        // Log thông tin đăng nhập (chỉ dùng cho debug - không log password trong production)
        _logger.LogInformation("Login attempt: Email: {Email}, Password: {Password}, Role: {Role}",
            login.Email, login.Password, user.Role);

        // Gọi GenerateJwtToken chỉ với đối tượng user
        var tokenData = GenerateJwtToken(user);

        // Trả về token và thời gian hết hạn
        return Ok(new{tokenData.Token, tokenData.Expiration});
    }

    private (string Token, DateTime Expiration) GenerateJwtToken(User user)
    {
        var expiration = DateTime.Now.AddHours(2);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim("UserId", user.Id.ToString()),
            new Claim("role", user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Key create
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.")));
        // /*  SigningCredentials create
        //     SigningCredentials là thông tin để ký token
        //     Chúng ta sử dụng HMAC SHA256 để ký token
        //     Chúng ta cũng cần cung cấp key để xác thực token
        //     Chúng ta sẽ sử dụng key mà chúng ta đã tạo ở trên
        //     Chúng ta cũng cần cung cấp algorithm để xác thực token
        // */
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Token create
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds);

        // token return
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return (tokenString, token.ValidTo);
    }

    [HttpPost("register")] // POST api/auth/register
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        try
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }

            DtoUser userDto = new DtoUser(
                user.Email,
                user.PasswordHash,
                user.Role,
                DateTime.Now,
                DateTime.Now
            );

            CreatePasswordHash(user.PasswordHash, out byte[] passwordHash, out byte[] passwordSalt);
            _logger.LogInformation("Controller: Adding new user: {UserJson}", JsonSerializer.Serialize(userDto, new JsonSerializerOptions { WriteIndented = true }));
            await _userAsyncService.AddUserAsync(userDto);
             _logger.LogInformation("✅ User added successfully: {UserJson}", JsonSerializer.Serialize(userDto, new JsonSerializerOptions { WriteIndented = true }));
            return CreatedAtAction(nameof(Register), new { email = user.Email }, new { message = "User added successfully!", user = userDto });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while adding the user." }); // Trả về 500 Internal Server Error
        }
        
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
    
    /*
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAll();
            return Ok(users);
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto updateDto)
        {
            var user = await _userRepository.GetUserByEmail(updateDto.Email);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = updateDto.Email;
            user.Role = updateDto.Role;

            await _userRepository.UpdateUser(user);
            return NoContent();
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserByEmail(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.DelUser(id);
            return NoContent();
        }
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByEmail(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Implement logout logic if needed
            return Ok("Logged out successfully");
        }
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            // Implement refresh token logic if needed
            return Ok("Token refreshed successfully");
        }
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            // Implement forgot password logic if needed
            return Ok("Password reset link sent");
        }
        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            // Implement reset password logic if needed
            return Ok("Password reset successfully");
        }
        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            // Implement change password logic if needed
            return Ok("Password changed successfully");
        }
        [HttpPost("verify-email")]
        public IActionResult VerifyEmail([FromBody] VerifyEmailDto verifyEmailDto)
        {
            // Implement email verification logic if needed
            return Ok("Email verified successfully");
        }
        [HttpPost("resend-verification-email")]
        public IActionResult ResendVerificationEmail([FromBody] ResendVerificationEmailDto resendVerificationEmailDto)
        {
            // Implement resend verification email logic if needed
            return Ok("Verification email resent successfully");
        }
        [HttpPost("send-verification-email")]
        public IActionResult SendVerificationEmail([FromBody] SendVerificationEmailDto sendVerificationEmailDto)
        {
            // Implement send verification email logic if needed
            return Ok("Verification email sent successfully");
        }
        [HttpPost("verify-phone")]
        public IActionResult VerifyPhone([FromBody] VerifyPhoneDto verifyPhoneDto)
        {
            // Implement phone verification logic if needed
            return Ok("Phone verified successfully");
        }
        [HttpPost("resend-verification-phone")]
        public IActionResult ResendVerificationPhone([FromBody] ResendVerificationPhoneDto resendVerificationPhoneDto)
        {
            // Implement resend verification phone logic if needed
            return Ok("Verification phone resent successfully");
        }
        [HttpPost("send-verification-phone")]
        public IActionResult SendVerificationPhone([FromBody] SendVerificationPhoneDto sendVerificationPhoneDto)
        {
            // Implement send verification phone logic if needed
            return Ok("Verification phone sent successfully");
        }
        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpDto verifyOtpDto)
        {
            // Implement OTP verification logic if needed
            return Ok("OTP verified successfully");
        }
        [HttpPost("resend-otp")]
        public IActionResult ResendOtp([FromBody] ResendOtpDto resendOtpDto)
        {
            // Implement resend OTP logic if needed
            return Ok("OTP resent successfully");
        }
        [HttpPost("send-otp")]
        public IActionResult SendOtp([FromBody] SendOtpDto sendOtpDto)
        {
            // Implement send OTP logic if needed
            return Ok("OTP sent successfully");
        }
        [HttpPost("verify-security-question")]
        public IActionResult VerifySecurityQuestion([FromBody] VerifySecurityQuestionDto verifySecurityQuestionDto)
        {
            // Implement security question verification logic if needed
            return Ok("Security question verified successfully");
        }
        [HttpPost("resend-security-question")]
        public IActionResult ResendSecurityQuestion([FromBody] ResendSecurityQuestionDto resendSecurityQuestionDto)
        {
            // Implement resend security question logic if needed
            return Ok("Security question resent successfully");
        }
        [HttpPost("send-security-question")]
        public IActionResult SendSecurityQuestion([FromBody] SendSecurityQuestionDto sendSecurityQuestionDto)
        {
            // Implement send security question logic if needed
            return Ok("Security question sent successfully");
        }
        [HttpPost("verify-security-code")]
        public IActionResult VerifySecurityCode([FromBody] VerifySecurityCodeDto verifySecurityCodeDto)
        {
            // Implement security code verification logic if needed
            return Ok("Security code verified successfully");
        }
        [HttpPost("resend-security-code")]
        public IActionResult ResendSecurityCode([FromBody] ResendSecurityCodeDto resendSecurityCodeDto)
        {
            // Implement resend security code logic if needed
            return Ok("Security code resent successfully");
        }
        [HttpPost("send-security-code")]
        public IActionResult SendSecurityCode([FromBody] SendSecurityCodeDto sendSecurityCodeDto)
        {
            // Implement send security code logic if needed
            return Ok("Security code sent successfully");
        }
        [HttpPost("verify-security-token")]
        public IActionResult VerifySecurityToken([FromBody] VerifySecurityTokenDto verifySecurityTokenDto)
        {
            // Implement security token verification logic if needed
            return Ok("Security token verified successfully");
        }
        [HttpPost("resend-security-token")]
        public IActionResult ResendSecurityToken([FromBody] ResendSecurityTokenDto resendSecurityTokenDto)
        {
            // Implement resend security token logic if needed
            return Ok("Security token resent successfully");
        }
        [HttpPost("send-security-token")]
        public IActionResult SendSecurityToken([FromBody] SendSecurityTokenDto sendSecurityTokenDto)
        {
            // Implement send security token logic if needed
            return Ok("Security token sent successfully");
        }
        [HttpPost("verify-security-key")]
        public IActionResult VerifySecurityKey([FromBody] VerifySecurityKeyDto verifySecurityKeyDto)
        {
            // Implement security key verification logic if needed
            return Ok("Security key verified successfully");
        }
        [HttpPost("resend-security-key")]
        public IActionResult ResendSecurityKey([FromBody] ResendSecurityKeyDto resendSecurityKeyDto)
        {
            // Implement resend security key logic if needed
            return Ok("Security key resent successfully");
        }
        [HttpPost("send-security-key")]
        public IActionResult SendSecurityKey([FromBody] SendSecurityKeyDto sendSecurityKeyDto)
        {
            // Implement send security key logic if needed
            return Ok("Security key sent successfully");
        }
        [HttpPost("verify-security-pin")]
        public IActionResult VerifySecurityPin([FromBody] VerifySecurityPinDto verifySecurityPinDto)
        {
            // Implement security pin verification logic if needed
            return Ok("Security pin verified successfully");
        }
        [HttpPost("resend-security-pin")]
        public IActionResult ResendSecurityPin([FromBody] ResendSecurityPinDto resendSecurityPinDto)
        {
            // Implement resend security pin logic if needed
            return Ok("Security pin resent successfully");
        }
        [HttpPost("send-security-pin")]
        public IActionResult SendSecurityPin([FromBody] SendSecurityPinDto sendSecurityPinDto)
        {
            // Implement send security pin logic if needed
            return Ok("Security pin sent successfully");
        }

        public class Userlogin
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class UserRegisterDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class UserUpdateDto
        {
            public string Email { get; set; }
            public string Role { get; set; }
        }
        public class ForgotPasswordDto
        {
            public string Email { get; set; }
        }
        public class ResetPasswordDto
        {
            public string Email { get; set; }
            public string NewPassword { get; set; }
        }
        public class ChangePasswordDto
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }
        public class VerifyEmailDto
        {
            public string Email { get; set; }
            public string VerificationCode { get; set; }
        }
        public class ResendVerificationEmailDto
        {
            public string Email { get; set; }
        }
        public class SendVerificationEmailDto
        {
            public string Email { get; set; }
        }
        public class VerifyPhoneDto
        {
            public string PhoneNumber { get; set; }
            public string VerificationCode { get; set; }
        }
        public class ResendVerificationPhoneDto
        {
            public string PhoneNumber { get; set; }
        }
        public class SendVerificationPhoneDto
        {
            public string PhoneNumber { get; set; }
        }
        public class VerifyOtpDto
        {
            public string Otp { get; set; }
        }
        public class ResendOtpDto
        {
            public string PhoneNumber { get; set; }
        }
        public class SendOtpDto
        {
            public string PhoneNumber { get; set; }
        }
        public class VerifySecurityQuestionDto
        {
            public string Question { get; set; }
            public string Answer { get; set; }
        }
        public class ResendSecurityQuestionDto
        {
            public string Email { get; set; }
        }
        public class SendSecurityQuestionDto
        {
            public string Email { get; set; }
        }
        public class VerifySecurityCodeDto
        {
            public string Code { get; set; }
        }
        public class ResendSecurityCodeDto
        {
            public string PhoneNumber { get; set; }
        }
        public class SendSecurityCodeDto
        {
            public string PhoneNumber { get; set; }
        }
        public class VerifySecurityTokenDto
        {
            public string Token { get; set; }
        }
        public class ResendSecurityTokenDto
        {
            public string Email { get; set; }
        }
        public class SendSecurityTokenDto
        {
            public string Email { get; set; }
        }
        public class VerifySecurityKeyDto
        {
            public string Key { get; set; }
        }
        public class ResendSecurityKeyDto
        {
            public string Email { get; set; }
        }
        public class SendSecurityKeyDto
        {
            public string Email { get; set; }
        }
        public class VerifySecurityPinDto
        {
            public string Pin { get; set; }
        }
        public class ResendSecurityPinDto
        {
            public string PhoneNumber { get; set; }
        }
        public class SendSecurityPinDto
        {
            public string PhoneNumber { get; set; }
        }
    */
}
    

