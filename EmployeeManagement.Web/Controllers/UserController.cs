using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Services.DtoEntities;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JsonSerializer = System.Text.Json.JsonSerializer;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace UserApiTestController
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserAsyncService _userAsyncService;

        public UsersController(ILogger<UsersController> logger, IUserAsyncService userAsyncService)
        {
            _logger = logger;
            _userAsyncService = userAsyncService;
        }

        [HttpGet("dashboard")]
        [Authorize(Roles = "User")]
        public IActionResult GetUserDashboard()
        {
            return Ok("Dữ liệu dashboard dành cho Admin.");
        }
        
        [HttpPost("add")] // POST api/users/add
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddEmployee([FromBody] DtoUser userDto)
        {
            try
            {
                _logger.LogInformation("Controller: Adding new employee: {EmployeeJson}", JsonSerializer.Serialize(userDto, new JsonSerializerOptions { WriteIndented = true }));
                await _userAsyncService.AddUserAsync(userDto);
                _logger.LogInformation("✅ Employee added successfully: {EmployeeJson}", JsonSerializer.Serialize(userDto, new JsonSerializerOptions { WriteIndented = true }));
                return CreatedAtAction(nameof(GetUserDetailsAsync), new { email = userDto.Email }, new { message = "User added successfully!", user = userDto });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while adding the employee." }); // Trả về 500 Internal Server Error
            }
        }

        [HttpGet("get/{id}")] // GET api/employees/get/{id}
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserDetailsAsync(string email)
        {
            try
            {
                _logger.LogInformation("Controller: Getting employee details for Email: {Email}", email);
                var userDto = await _userAsyncService.GetUserDetailsAsync(email);

                if (userDto == null)
                {
                    _logger.LogWarning("Employee not found for Email: {Email}", email);
                    return NotFound(new { message = $"User with Email {email} not found." });
                }
                
                _logger.LogInformation("✅ Employee details retrieved successfully: {EmployeeJson}", 
                    JsonSerializer.Serialize(userDto, new JsonSerializerOptions { WriteIndented = true }));
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving employee details for ID {Id}: {ErrorMessage}", email, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving employee details." });
            }
        }

        [HttpGet("getAll")] // GET api/employees/getAll
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                _logger.LogInformation("Controller: Getting all users");
                var users = await _userAsyncService.GetAllUsersAsync();
                _logger.LogInformation("✅ Retrieved {Count} users: {UsersJson}", 
                    users.Count(), JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving employees: {ErrorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving employees." });
            }
        }
        
        [HttpPut("update")]// PUT api/employees/update
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEmployee([FromBody] DtoUser userDto)
        {
            try
            {
                _logger.LogInformation("Controller: Updating user with Email: {Email}", userDto.Email);
                await _userAsyncService.UpdateUserAsync(userDto);
                _logger.LogInformation("✅ User updated successfully: {UserJson}", 
                    JsonSerializer.Serialize(userDto, new JsonSerializerOptions { WriteIndented = true }));
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating user with Email {Email}: {ErrorMessage}", userDto.Email, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the employee." });
            }
        }

        [HttpDelete("delete/{id}")]// DELETE api/employees/delete/{id}
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                _logger.LogInformation("Controller: Deleting user with ID: {Id}", id);
                await _userAsyncService.DelUserAsync(id);
                _logger.LogInformation("✅ User deleted successfully: ID {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting user with ID {Id}: {ErrorMessage}", id, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the employee." });
            }
        }
    }
}
