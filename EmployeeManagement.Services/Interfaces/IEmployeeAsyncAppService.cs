using EmployeeManagement.Services.DtoEntities;

namespace EmployeeManagement.Services.Interfaces;

public interface IEmployeeAsyncAppService
{
    Task AddEmployeeAsync(DtoEmployee employeeDto);
    Task<DtoEmployee> GetEmployeeDetailsAsync(int id);
    Task<IEnumerable<DtoEmployee>> GetAllEmployeesAsync();
    Task UpdateEmployeeAsync(DtoEmployee employeeDto);
    Task DelEmployeeAsync(int id);
}
