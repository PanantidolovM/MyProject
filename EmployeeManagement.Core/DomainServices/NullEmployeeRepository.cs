using EmployeeManagement.Core.Enities;
using EmployeeManagement.Core.Interfaces;
namespace EmployeeManagement.Core.DomainServices.NullEmployeeRepository;

public class NullEmployeeRepository : IEmployeeRepository
{
    // Giả sử IEmployeeRepository có một vài phương thức, ta triển khai trả về giá trị mặc định
    public Employee? GetById(int id) => null;
    public IEnumerable<Employee> GetAll() => Enumerable.Empty<Employee>();
    public void UpdateEmployee(Employee employee) { /* ko làm gì */ }
    public void AddEmployee(Employee employee) { /* ko làm gì */ }
    public void DelEmployee(int id) { /* ko làm gì */ }
    // Các phương thức khác nếu có...
}
