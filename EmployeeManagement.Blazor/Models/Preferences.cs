namespace EmployeeManagement.Blazor.Models;
public class Preferences
{
    public string Theme { get; set; } = "Light";
    public string Language { get; set; } = "en";
    public bool EnableNotifications { get; set; } = true;
    public string NotificationFrequency { get; set; } = "Immediate";
    public string ExportFormat { get; set; } = "CSV";
    public bool IncludeHeaders { get; set; } = true;
    public string CustomTheme { get; set; } = "";
    public string CustomFont { get; set; } = "";
}