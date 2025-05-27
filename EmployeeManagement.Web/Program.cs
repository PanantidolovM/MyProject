using EmployeeManagement.Core.DomainServices;
using EmployeeManagement.Core.Infrastructure;
using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Services.Interfaces;
using EmployeeManagement.Tests.ApiTests;
using EmployeeManagement.Services.ApplicationServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure Authentication service with JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    // Cofigure JWT token validation
    // Validate the token using the secret key and issuer/audience
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.")
            )
        )
    };
});
// Configure Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("role", "admin"));
});


// Apply logging
builder.Services.AddLogging(logging =>{
    logging.AddConsole();
    logging.AddDebug();
});

// Apply Domain Service 
builder.Services.AddSingleton<IEmployeeDomainService, EmployeeDomainService>();
builder.Services.AddSingleton<UserDomainService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

// Apply Infrastructure Service
builder.Services.AddSingleton<IEmployeeIdGenerator, InMemoryEmployeeIdGenerator>();
builder.Services.AddSingleton<IUserIdGenerator, InMemoryUserIdGenerator>();

// Apply Application Service
builder.Services.AddSingleton<IEmployeeAsyncAppService, EmployeeAsyncAppServices>();
builder.Services.AddSingleton<IUserAsyncService, UserRepoAsyncServices>();

// Apply EmployeeApiTestController for testing
builder.Services.AddSingleton<EmployeeApiTester>();

// Apply controller
builder.Services.AddControllers();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Middleware
app.UseRouting();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();

// Map controllers
app.MapControllers();
app.MapGet("/", () => "Employee Management API is running.");
// app.MapGet("/", () => 
// {
//     var html = @"
//     <!DOCTYPE html>
//     <html>
//     <head>
//         <meta charset=""utf-8"" />
//         <title>Trang Chủ</title>
//         <link rel=""stylesheet"" href=""/css/bootstrap.min.css"" />
//     </head>
//     <body>
//         <div class=""container"">
//             <h1>Chào mừng đến với Employee Management</h1>
//             <p>Đây là giao diện được tạo từ endpoint.</p>
//         </div>
//         <script src=""/js/bootstrap.bundle.min.js""></script>
//     </body>
//     </html>";
//     return Results.Content(html, "text/html");
// });
app.Run();

// Aplly Decorator (need install Scrutor package by using NuGet)
// builder.Services.Decorate<IEmployeeDomainService, LoggingDecorator>();

