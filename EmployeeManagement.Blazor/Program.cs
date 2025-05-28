using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using EmployeeManagement.Blazor;
using System.IdentityModel.Tokens.Jwt;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Xóa mapping mặc định của JwtSecurityTokenHandler một lần khi khởi động ứng dụng.
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Đăng ký HttpClient với BaseAddress
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5048/") });

// Đăng ký Options (cần thiết cho Authorization)
builder.Services.AddOptions();

// Đăng ký AuthorizationCore cho các dịch vụ phân quyền cơ bản
builder.Services.AddAuthorizationCore();

// Đăng ký dịch vụ AuthenticationStateProvider tùy chỉnh và cho phép tiêm trực tiếp đối tượng CustomAuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();

await builder.Build().RunAsync();
