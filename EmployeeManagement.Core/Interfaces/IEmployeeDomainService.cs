using EmployeeManagement.Core.Enities;

namespace EmployeeManagement.Core.Interfaces;

public interface IEmployeeDomainService
{
    //社員情報検索取得
    Employee GetEmployeeDetails(int id);

    //社員一覧検索取得
    IEnumerable<Employee> GetAllEmployees();

    //追加
    void AddEmployee(Employee employee);

    //更新
    void UpdateEmployee(Employee employee);

    //削除
    void DelEmployee(int id);
}   
