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
