using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Interfaces;

namespace EmployeeManagement.Core.DomainServices;

public class EmployeeDomainService : IEmployeeDomainService
{
    // 一時的なリポジトリの実装
    // ここでは、リストを使用して社員情報を保存します。
    // 実際のアプリケーションでは、データベースや他の永続化ストレージを使用することが一般的です。
    // 社員情報を保存するリスト
    private readonly List<Employee> _employees = new List<Employee>();
    // 社員IDを生成するためのインターフェース
    private readonly IEmployeeIdGenerator _idGenerator;

    // コンストラクタ
    public EmployeeDomainService(IEmployeeIdGenerator idGenerator)
    {
        _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
    }

    // 社員登録処理
    public async Task AddEmployee(Employee employee)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee));

        if (employee.Id == 0)
        {
            employee.Id = _idGenerator.GetNextEmployeeId();
        }

        _employees.Add(employee);

        await Task.CompletedTask;
    }

    public async Task UpdateEmployee(Employee employee)
    {
        var existingEmployee = _employees.FirstOrDefault(e => e.Id == employee.Id);

        if (existingEmployee == null)
        {
            throw new KeyNotFoundException($"社員の{employee.Id}が存在しません.");
        }
        
        existingEmployee.FirstName = employee.FirstName;
        existingEmployee.LastName = employee.LastName;
        existingEmployee.KokuSeki = employee.KokuSeki;
        existingEmployee.Passport = employee.Passport;
        existingEmployee.Shikaku = employee.Shikaku;
        existingEmployee.MyNumber = employee.MyNumber;
        existingEmployee.BiKou = employee.BiKou;
        existingEmployee.JuuSho = employee.JuuSho;
        existingEmployee.Keitai = employee.Keitai;
        existingEmployee.Mail = employee.Mail;
        existingEmployee.Salary = employee.Salary;
        existingEmployee.NyushaBi = employee.NyushaBi;
        existingEmployee.UpdateDate = DateTime.Now;
        await Task.CompletedTask;
    }

    // 社員検索処理
    public async Task<Employee> GetEmployeeDetails(int id){
        // 社員情報の検索
        var employee = await Task.Run(() => _employees.FirstOrDefault(e => e.Id == id));
        // 社員が見つからない場合、KeyNotFoundExceptionをスローします。
        if (employee == null)
        {
            throw new KeyNotFoundException($"社員の{id}が検索できません.");
        }
        return employee;
    }

    //社員一覧検索処理
    public async Task<IEnumerable<Employee>> GetAllEmployees(){
        // 社員一覧の取得
        // ここでは、Task.FromResultを使用して、非同期メソッドのように振る舞います。
        return await Task.FromResult(_employees);
    }

    // 社員削除処理
    public async Task DelEmployee(int id){
        var employee = _employees.FirstOrDefault(e => e.Id == id);

        // 社員が見つからない場合、KeyNotFoundExceptionをスローします。 
        if (employee == null)
        {
            throw new KeyNotFoundException($"社員の{id}が検索できません.");
        }

        // 社員情報の削除
        _employees.Remove(employee);

        // メソッドが非同期パターンに従っていることを確認します
        // ここでは、Task.CompletedTaskを使用して、非同期メソッドのように振る舞います。
        await Task.CompletedTask;
    }
}
