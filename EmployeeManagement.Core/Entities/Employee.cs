
namespace EmployeeManagement.Core.Enities;
public class Employee
{
    public int Id { get; internal set; }
    public string FirstName { get;  set; } = string.Empty;
    public string LastName { get;  set; } = string.Empty;
    public string KokuSeki { get;  set; } = string.Empty;
    public string Passport { get;  set; } = string.Empty;
    public string Shikaku { get;  set; } = string.Empty;
    public string MyNumber { get;  set; } = string.Empty;
    public string BiKou{get;  set;} = string.Empty;
    public string JuuSho{get;  set;} = string.Empty;
    public string Keitai{get;  set;} = string.Empty;
    public string Mail{get;  set;} = string.Empty;
    public decimal Salary { get;  set; }

    // public Employee(){}

    public Employee(int id,string firstname,string lastname, string kokuseki, string passport, string shikaku, string mynumber,string bikou, string juusho, string keitai, string mail ,decimal salary){
            Id = id;
            FirstName = firstname;
            LastName = lastname;
            KokuSeki = kokuseki;
            Passport = passport;
            Shikaku = shikaku;
            MyNumber = mynumber;
            BiKou = bikou;
            JuuSho = juusho;
            Keitai = keitai;
            Mail = mail;
            Salary = salary;
    }

// 社員情報更新
public void UpdateInfo(string firstname, string lastname, string kokuseki, string passport, string shikaku, string mynumber,string bikou, string juusho, string keitai, string mail ,decimal salary)
    {
        // Thực hiện các validate nghiệp vụ (nếu có) ở đây...
        FirstName = firstname;
        LastName = lastname;
        KokuSeki = kokuseki;
        Passport = passport;
        Shikaku = shikaku;
        MyNumber = mynumber;
        BiKou = bikou;
        JuuSho = juusho;
        Keitai = keitai;
        Mail = mail;
        Salary = salary;
    }
}
