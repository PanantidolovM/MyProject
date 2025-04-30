using System.Numerics;
namespace EmployeeManagement.Core.Enities;
public class Employee
{
    public BigInteger Id { get; internal set; }
    public string FirstName { get;  set; }
    public string LastName { get;  set; }
    public string KokuSeki { get;  set; }
    public string Passport { get;  set; }
    public string Shikaku { get;  set; }
    public string MyNumber { get;  set; }
    public string BiKou{get;  set;}
    public string JuuSho{get;  set;}
    public string Keitai{get;  set;}
    public string Mail{get;  set;}
    public decimal Salary { get;  set; }

    // public Employee(){}

    public Employee(BigInteger id,string firstname,string lastname, string kokuseki, string passport, string shikaku, string mynumber,string bikou, string juusho, string keitai, string mail ,decimal salary){
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
