using EmployeeManagement.Core.Entities;
using EmployeeManagement.Services.DtoEntities;
using EmployeeManagement.Services.Interfaces;
using EmployeeManagement.Core.Interfaces;
using Microsoft.Extensions.Logging;
using JsonSerializer = System.Text.Json.JsonSerializer;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace EmployeeManagement.Services.ApplicationServices;

public class UserRepoAsyncServices : IUserAsyncService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<EmployeeAsyncAppServices> _logger;
    public UserRepoAsyncServices(IUserRepository userRepository,ILogger<EmployeeAsyncAppServices> logger){
        _userRepository = userRepository;
        _logger = logger;
    }

    // 社員登録処理
    public async Task AddUserAsync(DtoUser userDto){
        _logger.LogInformation("Adding user: {DtoUser}", userDto);

        // バリデーションチェック
        if (string.IsNullOrWhiteSpace(userDto.Email) ||
            string.IsNullOrWhiteSpace(userDto.PasswordHash))
        {
            _logger.LogWarning("User or Password cannot be empty.");
            throw new ArgumentException("ID、パスポートを入力してください!");
        }
        
        User user = new User(
            userDto.Email,
            userDto.PasswordHash,
            userDto.Role,
            DateTime.Now,
            DateTime.Now
        );

        await _userRepository.AddUser(user);
    }

    // 社員更新処理
    public async Task UpdateUserAsync(DtoUser userDto){
        _logger.LogInformation("Updating employee: {DtoUser}", userDto);

        //　Employeeのnullチェック
        if (userDto == null)
        {
            _logger.LogError("Employee is null, cannot update employee. Please check the input."); // もしnullなら、エラーログを出力して、ArgumentNullExceptionをスローする
            throw new ArgumentNullException(nameof(userDto));
        }

        // バリデーションチェック
        if (string.IsNullOrWhiteSpace(userDto.Email) ||
            string.IsNullOrWhiteSpace(userDto.PasswordHash))
        {
            _logger.LogWarning("User or Password cannot be empty.");
            throw new ArgumentException("ID、パスポートを入力してください!");
        }

        // Lấy employee gốc từ domain service theo id
        var originalEmployee = await _userRepository.GetUserByEmail(userDto.Email);

        User user = new User(
            userDto.Email,
            userDto.PasswordHash,
            userDto.Role,
            originalEmployee.CreatedAt,
            DateTime.Now
        );

        await _userRepository.UpdateUser(user);
    }
    
    // 社員一覧検索処理
    public async Task<IEnumerable<DtoUser>> GetAllEmployeesAsync(){
        var users = await _userRepository.GetAll();

        // Employeeのnullチェック
        if (!users.Any())
        {
            _logger.LogWarning("No employees found"); 
            return Enumerable.Empty<DtoUser>(); // もしnullなら、エラーログを出力して、Empty list を返す
        }

        foreach (var user in users)
        {
            _logger.LogInformation("User: {UserJson}", JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true }));
        }
        
        // DtoUserのリストを作成
        var userDtos = users.Select(e => new DtoUser
        {
            Id = e.Id,
            Email = e.Email,
            PasswordHash = e.PasswordHash,
            Role = e.Role,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt
        });
        
        return userDtos;
    }

    // 社員詳細検索処理
    public async Task<DtoUser> GetEmployeeDetailsAsync(int id){
        var employee = await _userRepository.GetUserByEmail(id);

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

        DtoUser userDto = new DtoUser(
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
            employee.Salary,
            employee.NyushaBi,
            employee.CreateDate,
            employee.UpdateDate
        );

        _logger.LogInformation("Employee details: {EmployeeJson}", JsonSerializer.Serialize(employee, new JsonSerializerOptions { WriteIndented = true }));
        _logger.LogInformation("Employee details retrieved for ID: {Id}", id);
        return userDto;
    }

    // 社員削除処理
    public async Task DelEmployeeAsync(int id){
        await _employeeDomainService.DelEmployee(id);
        // idが0以下の場合、ArgumentExceptionをスローします。
        if (id <= 0)
        {
            _logger.LogWarning("Invalid employee ID: {Id}", id); // もし無効なら、エラーログを出力して、ArgumentExceptionをスローする
            throw new ArgumentException($"無効な{id}.");
        }
    }
}