using EmployeeManagement.Services.DtoEntities;
using EmployeeManagement.Services.Interfaces;
using EmployeeManagement.Core.Enities;
using EmployeeManagement.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Services.ApplicationServices;

public class EmployeeAppService : IEmployeeAppService
{
    private readonly IEmployeeAppService _employeeAppService;
    private readonly ILogger<EmployeeAppService> _logger;
    public EmployeeAppService(IEmployeeAppService employeeAppService,ILogger<EmployeeAppService> logger)
    {
        _employeeAppService = employeeAppService;
        _logger = logger;
    }
    public async Task AddEmployeeAsync(DtoEmployee employeeDto)
    {
        _logger.LogInformation("Application Service: Creating a new employee: {DtoEmployee}", employeeDto);
        var employee = new Employee(employeeDto.Id, employeeDto.FirstName, employeeDto.LastName, employeeDto.KokuSeki, employeeDto.Passport, employeeDto.Shikaku, employeeDto.MyNumber, employeeDto.BiKou, employeeDto.JuuSho, employeeDto.Keitai, employeeDto.Mail, employeeDto.Salary);
        await _employeeAppService.AddEmployeeAsync(employee);
    }
    public async Task UpdateEmployeeAsync(DtoEmployee employeeDto)
    {
        _logger.LogInformation("Application Service: Updating employee: {DtoEmployee}", employeeDto);
        var employee = new Employee(employeeDto.Id, employeeDto.FirstName, employeeDto.LastName, employeeDto.KokuSeki, employeeDto.Passport, employeeDto.Shikaku, employeeDto.MyNumber, employeeDto.BiKou, employeeDto.JuuSho, employeeDto.Keitai, employeeDto.Mail, employeeDto.Salary);
        await _employeeAppService.UpdateEmployeeAsync(employee);
    }
    public async Task<DtoEmployee> GetEmployeeDetailsAsync(int id)
    {
        _logger.LogInformation("Application Service: Getting employee details for ID: {Id}", id);
        var employee = await _employeeAppService.GetEmployeeDetailsAsync(id);
        if (employee == null)
        {
            _logger.LogWarning("Không tìm thấy nhân viên với Id {Id}", id);
            throw new KeyNotFoundException($"Không tìm thấy nhân viên với ID {id}.");
        }
        return new DtoEmployee(employee.Id, employee.FirstName, employee.LastName, employee.KokuSeki, employee.Passport, employee.Shikaku, employee.MyNumber, employee.BiKou, employee.JuuSho, employee.Keitai, employee.Mail, employee.Salary);
    }
    public async Task<IEnumerable<DtoEmployee>> GetAllEmployeesAsync()
    {
        _logger.LogInformation("Application Service: Getting all employees");
        var employees = await _employeeAppService.GetAllEmployeesAsync();
        // mapping từ Employee sang DtoEmployee
        var employeeDtos = employees.Select(employee => new DtoEmployee(employee.Id, employee.FirstName, employee.LastName, employee.KokuSeki, employee.Passport, employee.Shikaku, employee.MyNumber, employee.BiKou, employee.JuuSho, employee.Keitai, employee.Mail, employee.Salary)).ToList();
        return employeeDtos;
    }
    public async Task DelEmployeeAsync(int id)
    {
        _logger.LogInformation("Application Service: Deleting employee with ID: {Id}", id);
        await _employeeAppService.DelEmployeeAsync(id);
    }
}
    


 