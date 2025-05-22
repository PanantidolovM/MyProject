
using System.Net.Http.Json;
using EmployeeManagement.Services.DtoEntities;

namespace EmployeeManagement.Tests.ApiTests;

public class EmployeeApiTester
{
    public async Task Main()
    {
        Console.WriteLine("🔄 Bắt đầu kiểm thử API...");

        await TestCreateEmployee();
        await TestGetEmployees();
        await TestGetEmployeeDetails();
        await TestUpdateEmployee();
        await TestDeleteEmployee();

        Console.WriteLine("✅ Tất cả kiểm thử đã hoàn thành.");
        Console.WriteLine("Nhấn phím bất kỳ để thoát...");
        Console.ReadKey();
    }
    private  readonly HttpClient _client = new HttpClient{BaseAddress = new Uri("http://localhost:5048/")};

    // 1️⃣ Tạo nhân viên mới
    public  async Task TestCreateEmployee()
    {
        Console.WriteLine("📡 Đang gửi request POST /api/emp/add...");
        
        var newEmployee = new
        {
            Id = 0,
            FirstName = "Vo Thai Bao",
            LastName = "Minh",
            KokuSeki = "Viet Nam",
            Passport = "VN123456",
            Shikaku = "Engineer",
            MyNumber = "123456789",
            BiKou = "No remarks",
            JuuSho = "123 Main St",
            Keitai = "0987654321",
            Mail = "vothaibaominh1502@gmail.com",
            Salary = 6000
        };

        var response = await _client.PostAsJsonAsync("api/emp/add", newEmployee);
        
        if (response.IsSuccessStatusCode)
        {
            var createdEmployee = await response.Content.ReadAsStringAsync();
            Console.WriteLine("✅ Nhân viên được tạo:");
            Console.WriteLine(createdEmployee);
        }
        else
        {
            Console.WriteLine($"❌ Lỗi: {response.StatusCode}");
        }
    }

    public  async Task TestGetEmployeeDetails()
    {
        var employeeId = 0; // ID của nhân viên cần lấy thông tin
        var response = await _client.GetAsync($"api/emp/get/{employeeId}");
        if (response.IsSuccessStatusCode)
        {
            var employee = await response.Content.ReadFromJsonAsync<DtoEmployee>();
            if (employee == null)
            {
                Console.WriteLine("❌ Không tìm thấy nhân viên.");
                return;
            }
            Console.WriteLine($"✅ Thông tin nhân viên: {employee.FirstName} {employee.LastName}");
        }
        else
        {
            Console.WriteLine($"❌ Lỗi: {response.StatusCode}");
        }
    }

    public  async Task TestGetEmployees()
    {
        var response = await _client.GetAsync("api/emp/getAll");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"❌ Lỗi: {response.StatusCode}");
            return;
        }
        var employees = await response.Content.ReadFromJsonAsync<List<DtoEmployee>>();
        if (employees == null || employees.Count == 0)
        {
            Console.WriteLine("❌ Không có nhân viên nào.");
            return;
        }
        Console.WriteLine("✅ Danh sách nhân viên:");
        foreach (var emp in employees)
        {
            Console.WriteLine($"🔹\n" +
                            $"ID: {emp.Id}\n" +
                            $"FirstName: {emp.FirstName}\n" +
                            $"LastName: {emp.LastName}\n" +
                            $"KokuSeki: {emp.KokuSeki}\n" +
                            $"Passport: {emp.Passport}\n" +
                            $"Shikaku: {emp.Shikaku}\n" +
                            $"MyNumber: {emp.MyNumber}\n" +
                            $"BiKou: {emp.BiKou}\n" +
                            $"JuuSho: {emp.JuuSho}\n" +
                            $"Keitai: {emp.Keitai}\n" +
                            $"Mail: {emp.Mail}\n" +
                            $"Salary: {emp.Salary}");
        }
    }

    public  async Task TestUpdateEmployee()
    {
        var updatedEmployee = new DtoEmployee(
            1, // ID của nhân viên cần cập nhật
            "Vo Thai Bao",
            "Minh",
            "Viet Nam",
            "VN123456",
            "Senior Engineer",
            "123456789",
            "Updated remarks",
            "456 Main St",
            "0987654321",
            "vothaibaominh1111@gmail.com",
            7000,
            DateTime.Now,
            DateTime.Now,
            DateTime.Now
        );
        var response = await _client.PutAsJsonAsync("api/emp/update", updatedEmployee);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("✅ Nhân viên đã được cập nhật thành công.");
        }
        else
        {
            Console.WriteLine($"❌ Lỗi: {response.StatusCode}");
        }
    }

    public  async Task TestDeleteEmployee()
    {
        var employeeId = 1; // ID của nhân viên cần xóa
        var response = await _client.DeleteAsync($"api/emp/delete/{employeeId}");
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("✅ Nhân viên đã được xóa thành công.");
        }
        else
        {
            Console.WriteLine($"❌ Lỗi: {response.StatusCode}");
        }
    }
}
