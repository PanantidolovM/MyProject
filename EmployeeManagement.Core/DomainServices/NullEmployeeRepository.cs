<<<<<<< HEAD
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
=======
using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Core.Enities;
namespace EmployeeManagement.Core.DomainServices;

    public class NullEmployeeRepository : IEmployeeRepository
    {
        public Employee? GetById(int id) => null; // Return null for non-existing employee
        public IEnumerable<Employee> GetAll() => Enumerable.Empty<Employee>(); // Return empty list for no employees
        public void UpdateEmployee(Employee employee) { /* Do nothing */ }
        public void AddEmployee(Employee employee) { /* Do nothing */ }
        public void DelEmployee(int id) { /* Do nothing */ }
        
    }
>>>>>>> e71eef15e9ce9729b87bf22f34796ef6909ad53f
