using EmployeeManagement.Core.Enities;

namespace EmployeeManagement.Core.Interfaces;
public interface IEmployeeRepository{
    //追加
    void AddEmployee(Employee employee);

    //検索
    Employee GetById(int id);
    IEnumerable<Employee> GetAll();

    //更新
    void UpdateEmployee(Employee employee);

    //追加
    void DelEmployee(int id);  
    }
