using EmployeeManagement.Core.Enities;
using EmployeeManagement.Core.Interfaces;

namespace EmployeeManagement.Core.DomainServices;

public class EmployeeDomainService : IEmployeeDomainService
{
    // 一時的なリポジトリの実装
    // ここでは、リストを使用して社員情報を保存します。
    // 実際のアプリケーションでは、データベースや他の永続化ストレージを使用することが一般的です。
    // 社員情報を保存するリスト
    private readonly List<Employee> _employees = new List<Employee>();

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
        _employees.Add(employee);
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

         var existingEmployee = _employees.FirstOrDefault(e => e.Id == employee.Id);
        if (existingEmployee == null)
        {
            throw new KeyNotFoundException($"Không tìm thấy nhân viên với ID {employee.Id}.");
        }
        existingEmployee.UpdateInfo(employee.FirstName, employee.LastName, employee.KokuSeki, employee.Passport,
                                     employee.Shikaku, employee.MyNumber, employee.BiKou, employee.JuuSho, employee.Keitai,
                                     employee.Mail, employee.Salary);
    }

    // 社員検索処理
    public Employee GetEmployeeDetails(int id){
        // 入力チェック
        if (id <= 0)
        {
            throw new ArgumentException($"無効な{id}.");
        }
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"社員の{id}が検索できません.");
        }
        return employee;
    }

    //社員一覧検索処理
    public IEnumerable<Employee> GetAllEmployees(){
        // Kiểm tra xem danh sách nhân viên có rỗng hay không
        if (_employees == null || !_employees.Any())
        {
            throw new InvalidOperationException("Không có nhân viên nào trong danh sách.");
        }
        // Lấy danh sách nhân viên từ repository
        return _employees;
    }

    // 社員削除処理
    public void DelEmployee(int id){
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        if (id <= 0)
        {
            throw new ArgumentException($"無効な{id}.");
        }
        if (employee == null)
        {
            throw new KeyNotFoundException($"社員の{id}が検索できません.");
        }
        _employees.Remove(employee);
    }
}
