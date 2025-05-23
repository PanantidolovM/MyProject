using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Services.DtoEntities;
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
        private readonly IUserRepository _userRepository;

        public UsersController(ILogger<UsersController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
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
                await _userRepository.AddUser(userDto);
                _logger.LogInformation("✅ Employee added successfully: {EmployeeJson}", JsonSerializer.Serialize(userDto, new JsonSerializerOptions { WriteIndented = true }));
                return CreatedAtAction(nameof(GetEmployeeDetails), new { id = employeeDto.Id }, new { message = "Employee added successfully!", employee = employeeDto });
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
        public async Task<IActionResult> GetEmployeeDetails(int id)
        {
            try
            {
                _logger.LogInformation("Controller: Getting employee details for ID: {Id}", id);
                var employeeDto = await _employeeAsyncAppService.GetEmployeeDetailsAsync(id);

                if (employeeDto == null)
                {
                    _logger.LogWarning("Employee not found for ID: {Id}", id);
                    return NotFound(new { message = $"Employee with ID {id} not found." });
                }
                
                _logger.LogInformation("✅ Employee details retrieved successfully: {EmployeeJson}", 
                    JsonSerializer.Serialize(employeeDto, new JsonSerializerOptions { WriteIndented = true }));
                return Ok(employeeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving employee details for ID {Id}: {ErrorMessage}", id, ex.Message);
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
                _logger.LogInformation("Controller: Getting all employees");
                var employees = await _employeeAsyncAppService.GetAllEmployeesAsync();
                _logger.LogInformation("✅ Retrieved {Count} employees: {EmployeesJson}", 
                    employees.Count(), JsonSerializer.Serialize(employees, new JsonSerializerOptions { WriteIndented = true }));
                return Ok(employees);
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
        public async Task<IActionResult> UpdateEmployee([FromBody] DtoEmployee employeeDto)
        {
            try
            {
                _logger.LogInformation("Controller: Updating employee with ID: {Id}", employeeDto.Id);
                await _employeeAsyncAppService.UpdateEmployeeAsync(employeeDto);
                _logger.LogInformation("✅ Employee updated successfully: {EmployeeJson}", 
                    JsonSerializer.Serialize(employeeDto, new JsonSerializerOptions { WriteIndented = true }));
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating employee with ID {Id}: {ErrorMessage}", employeeDto.Id, ex.Message);
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
                _logger.LogInformation("Controller: Deleting employee with ID: {Id}", id);
                await _employeeAsyncAppService.DelEmployeeAsync(id);
                _logger.LogInformation("✅ Employee deleted successfully: ID {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error deleting employee with ID {Id}: {ErrorMessage}", id, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the employee." });
            }
        }
    }
}
