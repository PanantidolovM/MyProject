using EmployeeManagement.Core.DomainServices;
using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Services.Interfaces;
using EmployeeManagement.Tests.UnitTest;
using EmployeeManagement.Tests.ApiTests;
using EmployeeManagement.Services.ApplicationServices;

var builder = WebApplication.CreateBuilder(args);

// Apply logging
builder.Services.AddLogging(logging =>{
    logging.AddConsole();
    logging.AddDebug();
});

// Apply Domain Service 
builder.Services.AddSingleton<IEmployeeDomainService, EmployeeDomainService>();
builder.Services.AddSingleton<IEmployeeRepository, NullEmployeeRepository>();

// Apply Application Service
builder.Services.AddSingleton<IEmployeeAsyncAppService, EmployeeAsyncAppServices>();

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

// Aplly Decorator (need install Scrutor package by using NuGet)
// builder.Services.Decorate<IEmployeeDomainService, LoggingDecorator>();

