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
builder.Services.AddScoped<IEmployeeAsyncService, LoggingDecorator>();

// Apply Application Service
builder.Services.AddScoped<IEmployeeAppService, EmployeeAppService>();

// Sau đó đăng ký IEmployeeDomainService thông qua factory:( thu cong khi khong co Scrutor) 
builder.Services.AddScoped<IEmployeeDomainService>(provider => {
    // Lấy instance của EmployeeDomainService từ container
    var innerService = provider.GetRequiredService<EmployeeDomainService>();
    // Lấy ILogger của lớp decorator
    var logger = provider.GetRequiredService<ILogger<LoggingDecorator>>();
    // Trả về instance của decorator bọc lớp gốc
    return new LoggingDecorator(innerService, logger);
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

// Đăng ký Decorator (cần cài đặt package Scrutor qua NuGet)
// builder.Services.Decorate<IEmployeeDomainService, LoggingDecorator>();
