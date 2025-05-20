namespace EmployeeManagement.Blazor.Models;

public class DtoEmployee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string KokuSeki { get; set; } = string.Empty;
    public string Passport { get; set; } = string.Empty;
    public string Shikaku { get; set; } = string.Empty;
    public string MyNumber { get; set; } = string.Empty;
    public string BiKou { get; set; } = string.Empty;
    public string JuuSho { get; set; } = string.Empty;
    public string Keitai { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public DateTime NyushaBi { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}

