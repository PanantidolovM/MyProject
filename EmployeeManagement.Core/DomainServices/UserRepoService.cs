namespace EmployeeManagement.Core.DomainServices;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new List<User>();
    private readonly IUserIdGenerator _idGenerator;
    public UserRepository(IUserIdGenerator idGenerator)
    {
        _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
    }

    public async Task AddUser(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (user.Id == 0)
        {
            user.Id = _idGenerator.GetNextUserId();
        }

        // Nếu Password chưa được tính toán, gán nó bằng cách băm mật khẩu tùy chỉnh.
        // if (string.IsNullOrEmpty(user.Password))
        // {
        //     // Ví dụ: gán cho mật khẩu mặc định "Bmprao1234@"
        //     user.Password = PasswordHelper.ComputeHash("Bmprao1234@");
        // }

        // Check if the email already exists
        var existingUser = await Task.Run(() => _users.FirstOrDefault(e => e.Email == user.Email));

        if (existingUser != null)
        {
            throw new Exception("Email already exists");
        }

        // Hash the password before storing
        if (!string.IsNullOrEmpty(user.PasswordHash))
        {
            user.PasswordHash = PasswordHelper.ComputeHash(user.PasswordHash);
            user.PasswordHash = ""; // Clear plain password
        }

        _users.Add(user);

        await Task.CompletedTask;
    }

    public async Task<User> GetUserById(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

        var user = await Task.Run(() => _users.FirstOrDefault(e => e.Id == id));

        if (user == null)
        {
            throw new Exception("User not found");
        }

        return user;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));

        var user = await Task.Run(() => _users.FirstOrDefault(e => e.Email == email));

        if (user == null)
        {
            throw new Exception("User not found");
        }

        return user;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await Task.FromResult(_users);
    }

    public async Task UpdateUser(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        var existingUser = _users.FirstOrDefault(e => e.Email == user.Email);

        if (existingUser == null)
        {
            throw new Exception("User not found");
        }

        existingUser.Email = user.Email;
        existingUser.PasswordHash = user.PasswordHash;
        existingUser.UpdatedAt = DateTime.UtcNow;
        await Task.CompletedTask;
    }

    public async Task DelUser(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

        var user = _users.FirstOrDefault(e => e.Id == id);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        _users.Remove(user);
        await Task.CompletedTask;
    }
}