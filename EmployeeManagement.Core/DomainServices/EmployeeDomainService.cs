using EmployeeManagement.Core.Enities;
using EmployeeManagement.Core.Interfaces;

namespace EmployeeManagement.Core.DomainServices;

public class EmployeeDomainService  : IEmployeeDomainService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeDomainService(IEmployeeRepository employeeRepository){
        
        _employeeRepository = employeeRepository;
    }
    // 社員登録処理
    public void AddEmployee(Employee employee){
         // 入力チェック
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));
        // Ví dụ: đảm bảo rằng các thông tin bắt buộc không được để trống
        if (string.IsNullOrWhiteSpace(employee.FirstName) ||
            string.IsNullOrWhiteSpace(employee.LastName))
        {
            throw new ArgumentException("姓名を入力してください！");
        }

        // Ví dụ: đảm bảo rằng các thông tin bắt buộc không được để trống
        if (string.IsNullOrWhiteSpace(employee.Mail))
        {
            throw new ArgumentException("メールを入力してください！");
        }
        _employeeRepository.AddEmployee(employee);
    }

    // 社員更新処理
    public void UpdateEmployee(Employee employee){
         // Kiểm tra đầu vào
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));
        
        if (string.IsNullOrWhiteSpace(employee.FirstName) ||
            string.IsNullOrWhiteSpace(employee.LastName))
        {
            throw new ArgumentException("Tên và họ của nhân viên không được để trống.");
        }

        if (string.IsNullOrWhiteSpace(employee.Mail))
        {
            throw new ArgumentException("Email của nhân viên không được để trống.");
        }
        _employeeRepository.UpdateEmployee(employee);
    }

    // 社員検索処理
    public Employee GetEmployeeDetails(int id){
        var employee = _employeeRepository.GetById(id);
        if (id <= 0)
        {
            throw new ArgumentException($"無効な{id}.");
        }
        if (employee == null)
        {
            throw new KeyNotFoundException($"社員の{id}が検索できません.");
        }
        return employee;
    }

    //社員一覧検索処理
    public IEnumerable<Employee> GetAllEmployees(){
        List<Employee> employees = _employeeRepository.GetAll().ToList();
        // Kiểm tra xem danh sách nhân viên có rỗng hay không
        if (_employeeRepository.GetAll() == null || !_employeeRepository.GetAll().Any())
        {
            throw new InvalidOperationException("Không có nhân viên nào trong danh sách.");
        }
        // Lấy danh sách nhân viên từ repository
        return employees;
    }

    // 社員削除処理
    public void DelEmployee(int id){
        var employee = _employeeRepository.GetById(id);
        if (id <= 0)
        {
            throw new ArgumentException($"無効な{id}.");
        }
        if (employee == null)
        {
            throw new KeyNotFoundException($"社員の{id}が検索できません.");
        }
        _employeeRepository.DelEmployee(id);
    }
    
}
