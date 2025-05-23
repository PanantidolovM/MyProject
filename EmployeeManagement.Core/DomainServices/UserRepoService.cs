namespace EmployeeManagement.Core.DomainServices;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

public class UserRepository : IUserRepository
{
    private readonly IUserRepository _userRepository;
    private readonly IEmployeeIdGenerator _idGenerator;
    public UserRepository(IUserRepository userRepository, IEmployeeIdGenerator idGenerator)
    {
        _userRepository = userRepository;
        _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
    }

    public async Task AddUser(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (user.Id == 0)
        {
            user.Id = _idGenerator.GetNextEmployeeId();
        }
        // Check if the email already exists
        var existingUser = await _userRepository.GetUserByEmail(user.Email);
        if (existingUser != null)
        {
            throw new Exception("Email already exists");
        }

        await _userRepository.AddUser(user);
    }
    public async Task<User> GetUserByEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));

        var user = await _userRepository.GetUserByEmail(email);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return user;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        var users = await _userRepository.GetAll();
        if (users == null || !users.Any())
        {
            throw new Exception("No users found");
        }

        return users;
    }

    public async Task UpdateUser(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        var existingUser = await _userRepository.GetUserByEmail(user.Email);
        if (existingUser == null)
        {
            throw new Exception("User not found");
        }
        existingUser.Email = user.Email;
        existingUser.PasswordHash = user.PasswordHash;
        existingUser.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateUser(user);
    }

    public async Task DelUser(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

        var user = await _userRepository.GetUserByEmail(id.ToString());
        if (user == null)
        {
            throw new Exception("User not found");
        }

        await _userRepository.DelUser(id);
    }
}