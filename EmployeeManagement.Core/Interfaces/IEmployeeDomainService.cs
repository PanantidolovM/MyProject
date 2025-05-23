using EmployeeManagement.Core.Entities;

namespace EmployeeManagement.Core.Interfaces;
public interface IEmployeeDomainService
{
    //追加
    Task AddEmployee(Employee employee);
    //社員情報検索取得
    Task<Employee> GetEmployeeDetails(int id);
    //社員一覧検索取得
    Task<IEnumerable<Employee>> GetAllEmployees();
    //更新
    Task UpdateEmployee(Employee employee);
    //削除
    Task DelEmployee(int id);
}   
