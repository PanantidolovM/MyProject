using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Services.Interfaces;
using EmployeeManagement.Services.DtoEntities;
[ApiController]
[Route("api/employees")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeAppService _employeeAppService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IEmployeeAppService employeeAppService, ILogger<EmployeeController> logger)
    {
        _employeeAppService = employeeAppService;
        _logger = logger;
    }

    [HttpPost("add")] // POST api/employees/add
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddEmployee([FromBody] DtoEmployee employeeDto)
    {
        _logger.LogInformation("Controller: Adding new employee");
        await _employeeAppService.AddEmployeeAsync(employeeDto);
        _logger.LogInformation("✅ Nhân viên được tạo: {@newEmployee}", employeeDto);
        return Ok(new { message = "Employee added successfully!" });
    }

    [HttpGet("get/{id}")] // GET api/employees/get/{id}
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployeeDetails(int id)
    {
        _logger.LogInformation("Controller: Getting employee details for ID: {Id}", id);
        var employee = await _employeeAppService.GetEmployeeDetailsAsync(id);
        if (employee == null)
        {
            return NotFound($"Employee with ID {id} not found.");
        }
        return Ok(employee);
    }

    [HttpGet("getAll")] // GET api/employees/getAll
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEmployees()
    {
        _logger.LogInformation("Controller: Getting all employees");
        var employees = await _employeeAppService.GetAllEmployeesAsync();
        return Ok(employees);
    }
    
    [HttpPut("update")]// PUT api/employees/update
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateEmployee([FromBody] DtoEmployee employeeDto)
    {
        _logger.LogInformation("Controller: Updating employee with ID: {Id}", employeeDto.Id);
        if (employeeDto == null)
        {
            return BadRequest("Invalid employee data.");
        }
        await _employeeAppService.UpdateEmployeeAsync(employeeDto);
        return NoContent();
    }

    [HttpDelete("delete/{id}")]// DELETE api/employees/delete/{id}
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        _logger.LogInformation("Controller: Deleting employee with ID: {Id}", id);
        await _employeeAppService.DelEmployeeAsync(id);
        return NoContent();
    }
}