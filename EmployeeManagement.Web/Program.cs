using EmployeeManagement.Core.DomainServices;
using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Services.ApplicationServices;
using EmployeeManagement.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Apply logging
builder.Services.AddLogging(logging =>{
    logging.AddConsole();
});

// Apply Domain Service 
builder.Services.AddScoped<IEmployeeDomainService, EmployeeDomainService>();
builder.Services.AddScoped<LoggingDecorator>();
builder.Services.AddScoped<IEmployeeRepository, NullEmployeeRepository>();

// Apply Application Service
builder.Services.AddScoped<IEmployeeAppService, EmployeeAppService>();

// Sau đó đăng ký IEmployeeDomainService thông qua factory:( thu cong khi khong co Scrutor) 
builder.Services.AddScoped<IEmployeeAsyncService>(provider => {
    // Lấy instance của EmployeeDomainService từ container
    var innerService = provider.GetRequiredService<LoggingDecorator>();
    // Lấy ILogger của lớp decorator
    var logger = provider.GetRequiredService<ILogger<LoggingDecorator>>();
    // Trả về instance của decorator bọc lớp gốc
    return new LoggingDecorator(innerService, logger);
});

var app = builder.Build();

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

