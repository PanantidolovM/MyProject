using System;
using System.Collections.Generic;
using EmployeeManagement.Core.Enities;


public interface IEmployeeService{
    Task<Employee> GetEmployeeDetailsAsync(int id);
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task AddEmployeeAsync(Employee employee);
    Task UpdateEmployeeAsync(Employee employee);
    Task DelEmployeeAsync(int id);
}