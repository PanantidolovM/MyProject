using EmployeeManagement.Services.DtoEntities;

namespace EmployeeManagement.Services.Interfaces;

public interface IUserAsyncService
{
    
    Task AddUserAsync(DtoUser userDto);
    Task<DtoUser> GetUserDetailsAsync(int id);
    Task<IEnumerable<DtoUser>> GetAllUsersAsync();
    Task UpdateUserAsync(DtoUser userDto);
    Task DelUserAsync(int id);
}
