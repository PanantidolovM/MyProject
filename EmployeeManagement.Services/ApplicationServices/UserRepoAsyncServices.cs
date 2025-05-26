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
    public UserRepoAsyncServices(IUserRepository userRepository, ILogger<EmployeeAsyncAppServices> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    // 社員登録処理
    public async Task AddUserAsync(DtoUser userDto)
    {
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
    public async Task UpdateUserAsync(DtoUser userDto)
    {
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
    public async Task<IEnumerable<DtoUser>> GetAllUsersAsync()
    {
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
    public async Task<DtoUser> GetUserDetailsAsync(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);

        // emailがNullの場合、ArgumentExceptionをスローします。
        if (string.IsNullOrWhiteSpace(email))
        {
            _logger.LogWarning("Invalid user Email: {Email}", email); // もし無効なら、エラーログを出力して、ArgumentExceptionをスローする
            throw new ArgumentException($"無効なEmail: {email}.");
        }
        // userのnullチェック
        if (user == null)
        {
            _logger.LogWarning("user not found for Email: {Email}", email); // もしnullなら、エラーログを出力して、KeyNotFoundExceptionをスローする
            throw new KeyNotFoundException($"ユーザの{email}が検索できません.");
        }

        var userDto = new DtoUser
        {
            Id = user.Id,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };

        _logger.LogInformation("User details: {UserJson}", JsonSerializer.Serialize(userDto, new JsonSerializerOptions { WriteIndented = true }));
        _logger.LogInformation("User details retrieved for Email: {Email}", email);
        return userDto;
    }

    // 社員削除処理
    public async Task DelUserAsync(int id)
    {
        await _userRepository.DelUser(id);
        // idが0以下の場合、ArgumentExceptionをスローします。
        if (id <= 0)
        {
            _logger.LogWarning("Invalid user ID: {Id}", id); // もし無効なら、エラーログを出力して、ArgumentExceptionをスローする
            throw new ArgumentException($"無効な{id}.");
        }
    }
}