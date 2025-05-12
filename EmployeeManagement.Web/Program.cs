using EmployeeManagement.Core.DomainServices;
using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Services.ApplicationServices;
using EmployeeManagement.Services.Interfaces;
using EmployeeManagement.Tests.UnitTest;
using EmployeeManagement.Tests.ApiTests;

var builder = WebApplication.CreateBuilder(args);

// Apply logging
builder.Services.AddLogging(logging =>{
    logging.AddConsole();
    logging.AddDebug();
});

// Apply Domain Service 
builder.Services.AddScoped<IEmployeeDomainService, EmployeeDomainService>();
builder.Services.AddScoped<IEmployeeRepository, NullEmployeeRepository>();

// Apply LoggingDecorator impliment IEmployeeAsyncService, base class cover:( manual when not have Scrutor package ) 
builder.Services.AddScoped<IEmployeeAsyncService, LoggingDecorator>(provider => {
    // Lấy instance của EmployeeDomainService từ container
    var innerService = provider.GetRequiredService<IEmployeeAsyncService >(); // dịch vụ gốc
    // Lấy ILogger của lớp decorator
    var logger = provider.GetRequiredService<ILogger<LoggingDecorator>>(); // Logger cho decorator
    // Trả về instance của decorator bọc lớp gốc
    return new LoggingDecorator(innerService, logger);
});

// Apply Application Service for using IEmployeeAsyncService (covered by LoggingDecorator)
builder.Services.AddScoped<IEmployeeAppService, EmployeeAppService>();

// Apply EmployeeApiTestController for testing
builder.Services.AddSingleton<EmployeeApiTester>();

// Apply UserApiTestController for testing
builder.Services.AddSingleton<IUserRepository, UserRepository>();

// Apply controller
builder.Services.AddControllers();

var app = builder.Build();

// Middleware
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => 
{
    var html = @"
    <!DOCTYPE html>
    <html>
    <head>
        <meta charset=""utf-8"" />
        <title>Trang Chủ</title>
        <link rel=""stylesheet"" href=""/css/bootstrap.min.css"" />
    </head>
    <body>
        <div class=""container"">
            <h1>Chào mừng đến với Employee Management</h1>
            <p>Đây là giao diện được tạo từ endpoint.</p>
        </div>
        <script src=""/js/bootstrap.bundle.min.js""></script>
    </body>
    </html>";
    return Results.Content(html, "text/html");
});

app.Run();

// Đăng ký Decorator (cần cài đặt package Scrutor qua NuGet)
// builder.Services.Decorate<IEmployeeDomainService, LoggingDecorator>();

