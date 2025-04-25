using EmployeeManagement.Core.Enities;
using EmployeeManagement.Core.Interfaces;

namespace EmployeeManagement.Core.DomainServices;

public class EmployeeDomainService  : IEmployeeDomainService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeDomainService(IEmployeeRepository employeeRepository){
        
        _employeeRepository = employeeRepository;
    }
    // 社員登録
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

        //　
        if (string.IsNullOrWhiteSpace(employee.Mail))
        {
            throw new ArgumentException("メールを入力してください！");
        }
        _employeeRepository.AddEmployee(employee);
    }

    // 社員更新
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

    // 社員検索
    public Employee GetEmployeeDetails(int id){
        // Áp dụng các quy tắc nghiệp vụ phức tạp hoặc tính toán nếu cần trước khi trả về
        var employee = _employeeRepository.GetById(id);
        // Ví dụ: kiểm tra điều kiện, tính toán thêm các thông tin…
        return employee;
    }

    //社員一覧検索
    public IEnumerable<Employee> GetAllEmployees(){
        return _employeeRepository.GetAll();
    }

    // 社員削除
    public void DelEmployee(int id){
        var employee = _employeeRepository.GetById(id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"社員の{id}が検索できません.");
        }
        _employeeRepository.DelEmployee(id);
    }
}
