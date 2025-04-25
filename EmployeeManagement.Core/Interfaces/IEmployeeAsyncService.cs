using EmployeeManagement.Core.Enities;

namespace EmployeeManagement.Core.Interfaces;
public interface IEmployeeAsyncService{
    Task<Employee> GetEmployeeDetailsAsync(int id);
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task AddEmployeeAsync(Employee employee);
    Task UpdateEmployeeAsync(Employee employee);
    Task DelEmployeeAsync(int id);
}