using EmployeeManagement.Core.Enities;
using EmployeeManagement.Services.Interfaces;
using Microsoft.Extensions.Logging;
using EmployeeManagement.Services.Interfaces; // Ensure the namespace containing IEmployeeAsyncAppService is imported

namespace EmployeeManagement.Services.ApplicationServices;

public class EmployeeAsyncAppServices : IEmployeeAsyncAppService{
    private readonly IEmployeeAsyncAppService _innerService;
    private readonly ILogger<EmployeeAsyncAppServices> _logger;
    public EmployeeAsyncAppServices(IEmployeeAsyncAppService innerService,ILogger<EmployeeAppServices> logger){
        _innerService = innerService;
        _logger = logger;
    }

    // 社員登録処理
    public async Task AddEmployeeAsync(Employee employee){
        _logger.LogInformation("Adding employee: {Employee}", employee);
        await _innerService.AddEmployeeAsync(employee);
        // 入力チェック
        if (string.IsNullOrWhiteSpace(employee.FirstName) ||
            string.IsNullOrWhiteSpace(employee.LastName))
        {
            _logger.LogWarning("First name or last name is empty, cannot add employee.");
            throw new ArgumentException("姓名を入力してください！");
        }
        if (string.IsNullOrWhiteSpace(employee.Mail))
        {
            _logger.LogWarning("Email is empty, cannot add employee.");
            throw new ArgumentException("メールを入力してください！");
        }
        _logger.LogInformation("Employee added: {Employee}", employee);
    }

    // 社員更新処理
    public async Task UpdateEmployeeAsync(Employee employee){
        _logger.LogInformation("Updating employee: {Employee}", employee);
        await _innerService.UpdateEmployeeAsync(employee);
        //　Employeeのnullチェック
        if (employee == null)
        {
            _logger.LogError("Employee is null, cannot update employee. Please check the input."); // もしnullなら、エラーログを出力して、ArgumentNullExceptionをスローする
            throw new ArgumentNullException(nameof(employee));
        }
        // 入力チェック
        if (string.IsNullOrWhiteSpace(employee.FirstName) ||
            string.IsNullOrWhiteSpace(employee.LastName))
        {
            _logger.LogWarning("First name or last name is empty, cannot update employee."); // もし空なら、エラーログを出力して、ArgumentExceptionをスローする
            throw new ArgumentException("社員の名前は空にできません。");
        }
        if (string.IsNullOrWhiteSpace(employee.Mail))
        {
            _logger.LogWarning("Email is empty, cannot update employee."); // もし空なら、エラーログを出力して、ArgumentExceptionをスローする
            throw new ArgumentException("社員のメールは空にできません。");
        }
        _logger.LogInformation("Employee updated: {Employee}", employee);
    }   
    
    // 社員一覧検索処理
    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(){
        _logger.LogInformation("Getting all employees");
        IEnumerable<Employee> employees = await _innerService.GetAllEmployeesAsync();
        // Employeeのnullチェック
        if (employees == null || !employees.Any())
        {
            _logger.LogWarning("No employees found"); // もしnullなら、エラーログを出力して、Empty list を返す
            return Enumerable.Empty<Employee>();
        }
        foreach (var employee in employees)
        {
            _logger.LogInformation("Employee: {Employee}", employee);
        }
        _logger.LogInformation("Retrieved {Count} employees", employees.Count());
        return employees;
    }

    // 社員詳細検索処理
    public async Task<Employee> GetEmployeeDetailsAsync(int id){
        _logger.LogInformation("Getting employee details for ID: {Id}", id);
        Employee employee = await _innerService.GetEmployeeDetailsAsync(id);
        // ID invalid check
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
        return employee;
    }

    // 社員削除処理
    public async Task DelEmployeeAsync(int id){
        _logger.LogInformation("Deleting employee with ID: {Id}", id);
        await _innerService.DelEmployeeAsync(id);
        // ID invalid check
        if (id <= 0)
        {
            _logger.LogWarning("Invalid employee ID: {Id}", id); // もし無効なら、エラーログを出力して、ArgumentExceptionをスローする
            throw new ArgumentException($"無効な{id}.");
        }
        _logger.LogInformation("Employee with ID {Id} deleted", id);
    }
}