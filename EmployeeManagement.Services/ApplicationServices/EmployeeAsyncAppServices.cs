using EmployeeManagement.Core.Enities;
using EmployeeManagement.Services.DtoEntities;
using EmployeeManagement.Services.Interfaces;
using EmployeeManagement.Core.Interfaces;
using Microsoft.Extensions.Logging;
using JsonSerializer = System.Text.Json.JsonSerializer;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace EmployeeManagement.Services.ApplicationServices;

public class EmployeeAsyncAppServices : IEmployeeAsyncAppService
{
    private readonly IEmployeeDomainService _employeeDomainService;
    private readonly ILogger<EmployeeAsyncAppServices> _logger;
    public EmployeeAsyncAppServices(IEmployeeDomainService employeeDomainService,ILogger<EmployeeAsyncAppServices> logger){
        _employeeDomainService = employeeDomainService;
        _logger = logger;
    }

    // 社員登録処理
    public async Task AddEmployeeAsync(DtoEmployee employeeDto){
        _logger.LogInformation("Adding employee: {DtoEmployee}", employeeDto);

        // バリデーションチェック
        if (string.IsNullOrWhiteSpace(employeeDto.FirstName) ||
            string.IsNullOrWhiteSpace(employeeDto.LastName))
        {
            _logger.LogWarning("First name or last name is empty, cannot add employee.");
            throw new ArgumentException("姓名を入力してください！");
        }
        if (string.IsNullOrWhiteSpace(employeeDto.Mail))
        {
            _logger.LogWarning("Email is empty, cannot add employee.");
            throw new ArgumentException("メールを入力してください！");
        }

        Employee employee = new Employee(
            employeeDto.Id,
            employeeDto.FirstName,
            employeeDto.LastName,
            employeeDto.KokuSeki,
            employeeDto.Passport,
            employeeDto.Shikaku,
            employeeDto.MyNumber,
            employeeDto.BiKou,
            employeeDto.JuuSho,
            employeeDto.Keitai,
            employeeDto.Mail,
            employeeDto.Salary
        );

        await _employeeDomainService.AddEmployee(employee);
        _logger.LogInformation("Employee added: {EmployeeJson}", JsonSerializer.Serialize(employee, new JsonSerializerOptions { WriteIndented = true }));
    }

    // 社員更新処理
    public async Task UpdateEmployeeAsync(DtoEmployee employeeDto){
        _logger.LogInformation("Updating employee: {DtoEmployee}", employeeDto);

        //　Employeeのnullチェック
        if (employeeDto == null)
        {
            _logger.LogError("Employee is null, cannot update employee. Please check the input."); // もしnullなら、エラーログを出力して、ArgumentNullExceptionをスローする
            throw new ArgumentNullException(nameof(employeeDto));
        }

        // バリデーションチェック
        if (string.IsNullOrWhiteSpace(employeeDto.FirstName) ||
            string.IsNullOrWhiteSpace(employeeDto.LastName))
        {
            _logger.LogWarning("First name or last name is empty, cannot update employee."); // もし空なら、エラーログを出力して、ArgumentExceptionをスローする
            throw new ArgumentException("社員の名前は空にできません。");
        }
        if (string.IsNullOrWhiteSpace(employeeDto.Mail))
        {
            _logger.LogWarning("Email is empty, cannot update employee."); // もし空なら、エラーログを出力して、ArgumentExceptionをスローする
            throw new ArgumentException("社員のメールは空にできません。");
        }

        Employee employee = new Employee(
            employeeDto.Id,
            employeeDto.FirstName,
            employeeDto.LastName,
            employeeDto.KokuSeki,
            employeeDto.Passport,
            employeeDto.Shikaku,
            employeeDto.MyNumber,
            employeeDto.BiKou,
            employeeDto.JuuSho,
            employeeDto.Keitai,
            employeeDto.Mail,
            employeeDto.Salary
        );

        await _employeeDomainService.UpdateEmployee(employee);
        _logger.LogInformation("Employee updated: {EmployeeJson}", JsonSerializer.Serialize(employee, new JsonSerializerOptions { WriteIndented = true }));
    }   
    
    // 社員一覧検索処理
    public async Task<IEnumerable<DtoEmployee>> GetAllEmployeesAsync(){
        _logger.LogInformation("Getting all employees");
        var employees = await _employeeDomainService.GetAllEmployees();

        // Employeeのnullチェック
        if (!employees.Any())
        {
            _logger.LogWarning("No employees found"); 
            return Enumerable.Empty<DtoEmployee>(); // もしnullなら、エラーログを出力して、Empty list を返す
        }

        foreach (var employee in employees)
        {
            _logger.LogInformation("Employee: {EmployeeJson}", JsonSerializer.Serialize(employee, new JsonSerializerOptions { WriteIndented = true }));
        }
        
        // DtoEmployeeのリストを作成
        var employeeDtos = employees.Select(e => new DtoEmployee(
            e.Id, e.FirstName, e.LastName, e.KokuSeki, e.Passport, e.Shikaku,
            e.MyNumber, e.BiKou, e.JuuSho, e.Keitai, e.Mail, e.Salary
        ));

        _logger.LogInformation("Retrieved {Count} employees", employees.Count());
        return employeeDtos;
    }

    // 社員詳細検索処理
    public async Task<DtoEmployee> GetEmployeeDetailsAsync(int id){
        _logger.LogInformation("Getting employee details for ID: {Id}", id);
        var employee = await _employeeDomainService.GetEmployeeDetails(id);

        // idが0以下の場合、ArgumentExceptionをスローします。
        if (id <= 0)
        {
            _logger.LogWarning("Invalid employee ID: {Id}", id);// もし無効なら、エラーログを出力して、ArgumentExceptionをスローする
            throw new ArgumentException($"無効な{id}.");
        }
        // Employeeのnullチェック
        if (employee == null)
        {
            _logger.LogWarning("Employee not found for ID: {Id}", id); // もしnullなら、エラーログを出力して、KeyNotFoundExceptionをスローする
            throw new KeyNotFoundException($"社員の{id}が検索できません.");
        }

        DtoEmployee employeeDto = new DtoEmployee(
            id,
            employee.FirstName,
            employee.LastName,
            employee.KokuSeki,
            employee.Passport,
            employee.Shikaku,
            employee.MyNumber,
            employee.BiKou,
            employee.JuuSho,
            employee.Keitai,
            employee.Mail,
            employee.Salary
        );

        _logger.LogInformation("Employee details: {EmployeeJson}", JsonSerializer.Serialize(employee, new JsonSerializerOptions { WriteIndented = true }));
        _logger.LogInformation("Employee details retrieved for ID: {Id}", id);
        return employeeDto;
    }

    // 社員削除処理
    public async Task DelEmployeeAsync(int id){
        _logger.LogInformation("Deleting employee with ID: {Id}", id);
        await _employeeDomainService.DelEmployee(id);
        // idが0以下の場合、ArgumentExceptionをスローします。
        if (id <= 0)
        {
            _logger.LogWarning("Invalid employee ID: {Id}", id); // もし無効なら、エラーログを出力して、ArgumentExceptionをスローする
            throw new ArgumentException($"無効な{id}.");
        }
        
        _logger.LogInformation("Employee with ID {Id} deleted", id);
    }
}