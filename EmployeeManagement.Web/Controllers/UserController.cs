using EmployeeManagement.Services.DtoEntities;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JsonSerializer = System.Text.Json.JsonSerializer;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace UserApiTestController;

[ApiController]
[Route("api/users")]
public class UsersController(IUserAsyncService userAsyncService, ILogger<UsersController> logger) : ControllerBase
{
    private readonly ILogger<UsersController> _logger = logger;
    private readonly IUserAsyncService _userAsyncService = userAsyncService;

    [HttpGet("dashboard")]
    [Authorize(Roles = "admin")]
    public IActionResult GetUserDashboard()
    {
        return Ok("Dữ liệu dashboard dành cho Admin.");
    }
    
    [HttpPost("add")] // POST api/users/add
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddUser([FromBody] DtoUser userDto)
    {
        try
        {
            _logger.LogInformation("Controller: Adding new user: {UserJson}", JsonSerializer.Serialize(userDto, new JsonSerializerOptions { WriteIndented = true }));
            await _userAsyncService.AddUserAsync(userDto);
            _logger.LogInformation("✅ User added successfully: {UserJson}", JsonSerializer.Serialize(userDto, new JsonSerializerOptions { WriteIndented = true }));
            return CreatedAtAction(nameof(GetUserDetails), new { id = userDto.Id }, new { message = "User added successfully!", user = userDto });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while adding the user." }); // Trả về 500 Internal Server Error
        }
    }

    [HttpGet("get/{id}")] // GET api/users/get/{email}
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserDetails(int id)
    {
        try
        {
            _logger.LogInformation("Controller: Getting user details for id: {Id}", id);
            var userDto = await _userAsyncService.GetUserDetailsAsync(id);

            if (userDto == null)
            {
                _logger.LogWarning("User not found for id: {Id}", id);
                return NotFound(new { message = $"User with id {id} not found." });
            }
            
            _logger.LogInformation("✅ User details retrieved successfully: {UserJson}", 
                JsonSerializer.Serialize(userDto, new JsonSerializerOptions { WriteIndented = true }));
            return Ok(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving user details for id {Id}: {ErrorMessage}", id, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving user details." });
        }
    }

    [HttpGet("getAll")] // GET api/users/getAll
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllUsers()
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
            _logger.LogError("Error retrieving users: {ErrorMessage}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving users." });
        }
    }
    
    [HttpPut("update")]// PUT api/users/update
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser([FromBody] DtoUser userDto)
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
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the user." });
        }
    }

    [HttpDelete("delete/{id}")]// DELETE api/users/delete/{id}
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(int id)
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
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the user." });
        }
    }
}

