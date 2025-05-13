using EmployeeManagement.Services.DtoEntities;

namespace EmployeeManagement.Services.Interfaces;
public interface IEmployeeAppService{
    Task AddEmployeeAsync(DtoEmployee employeeDto);
    Task UpdateEmployeeAsync(DtoEmployee employeeDto);
    Task<DtoEmployee> GetEmployeeDetailsAsync(int id);
    Task<IEnumerable<DtoEmployee>> GetAllEmployeesAsync();
    Task DelEmployeeAsync(int id);
}
