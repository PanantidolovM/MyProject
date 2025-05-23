using EmployeeManagement.Services.DtoEntities;

namespace EmployeeManagement.Services.Interfaces;

public interface IUserAsyncService
{
    
    Task AddUserAsync(DtoEmployee employeeDto);
    Task<DtoEmployee> GetUserDetailsAsync(int id);
    Task<IEnumerable<DtoEmployee>> GetAllUsersAsync();
    Task UpdateUserAsync(DtoEmployee employeeDto);
    Task DelUserAsync(int id);
}
