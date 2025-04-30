using EmployeeManagement.Services.DtoEntities;
using EmployeeManagement.Services.Interfaces;
using EmployeeManagement.Core.Enities;
using EmployeeManagement.Core.Interfaces;

namespace EmployeeManagement.Services.ApplicationServices;

public class EmployeeAppService : IEmployeeAppService
{
    private readonly IEmployeeAsyncService _employeeAsyncService;
    private readonly ILogger<EmployeeAppService> _logger;
    public EmployeeAppService(IEmployeeAsyncService employeeAsyncService,ILogger<EmployeeAppService> logger)
    {
        _employeeAsyncService = employeeAsyncService;
        _logger = logger;
    }
    public async Task AddEmployeeAsync(DtoEmployee employeeDto)
    {
        _logger.LogInformation("Application Service: Creating a new employee: {DtoEmployee}", employeeDto);
        var employee = new Employee(employeeDto.Id, employeeDto.FirstName, employeeDto.LastName, employeeDto.KokuSeki, employeeDto.Passport, employeeDto.Shikaku, employeeDto.MyNumber, employeeDto.BiKou, employeeDto.JuuSho, employeeDto.Keitai, employeeDto.Mail, employeeDto.Salary);
        await _employeeAsyncService.AddEmployeeAsync(employee);
    }
    public async Task UpdateEmployeeAsync(DtoEmployee employeeDto)
    {
        _logger.LogInformation("Application Service: Updating employee: {DtoEmployee}", employeeDto);
        var employee = new Employee(employeeDto.Id, employeeDto.FirstName, employeeDto.LastName, employeeDto.KokuSeki, employeeDto.Passport, employeeDto.Shikaku, employeeDto.MyNumber, employeeDto.BiKou, employeeDto.JuuSho, employeeDto.Keitai, employeeDto.Mail, employeeDto.Salary);
        await _employeeAsyncService.UpdateEmployeeAsync(employee);
    }
    public async Task<DtoEmployee> GetEmployeeDetailsAsync(int id)
    {
        _logger.LogInformation("Application Service: Getting employee details for ID: {Id}", id);
        var employee = await _employeeAsyncService.GetEmployeeDetailsAsync(id);
        if (employee == null)
        {
            _logger.LogWarning("Không tìm thấy nhân viên với Id {Id}", id);
            return null;
        }
        return new DtoEmployee(employee.Id, employee.FirstName, employee.LastName, employee.KokuSeki, employee.Passport, employee.Shikaku, employee.MyNumber, employee.BiKou, employee.JuuSho, employee.Keitai, employee.Mail, employee.Salary);
    }
    public async Task<IEnumerable<DtoEmployee>> GetAllEmployeesAsync()
    {
        _logger.LogInformation("Application Service: Getting all employees");
        var employees = await _employeeAsyncService.GetAllEmployeesAsync();
        // mapping từ Employee sang DtoEmployee
        var employeeDtos = employees.Select(employee => new DtoEmployee(employee.Id, employee.FirstName, employee.LastName, employee.KokuSeki, employee.Passport, employee.Shikaku, employee.MyNumber, employee.BiKou, employee.JuuSho, employee.Keitai, employee.Mail, employee.Salary)).ToList();
        // var employeeDtos = new List<DtoEmployee>();
        // foreach (var employee in employees)
        // {
        //     employeeDtos.Add(new DtoEmployee(employee.Id, employee.FirstName, employee.LastName, employee.KokuSeki, employee.Passport, employee.Shikaku, employee.MyNumber, employee.BiKou, employee.JuuSho, employee.Keitai, employee.Mail, employee.Salary));
        // }
        return employeeDtos;
    }
    public async Task DelEmployeeAsync(int id)
    {
        _logger.LogInformation("Application Service: Deleting employee with ID: {Id}", id);
        await _employeeAsyncService.DelEmployeeAsync(id);
    }
}
    


 