using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;
    private const string TokenKey = "jwtToken";

    public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        // Thiết lập mapping toàn cục một lần khi khởi động.
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Lấy token từ localStorage.
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
        if (string.IsNullOrWhiteSpace(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        try
        {
            // (Nếu chưa được gọi toàn cục, bạn có thể gọi lại tại đây)
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Tạo ClaimsIdentity với name claim là "sub" và role được nhận dạng qua ClaimTypes.Role.
            var identity = new ClaimsIdentity(
                jwtToken.Claims,
                "jwt",
                JwtRegisteredClaimNames.Sub,
                ClaimTypes.Role);
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }
        catch
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public async Task NotifyUserAuthenticationAsync(string token)
    {
        // Lưu token và cập nhật trạng thái xác thực.
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        var authState = await GetAuthenticationStateAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public async Task NotifyUserLogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        NotifyAuthenticationStateChanged(Task.FromResult(
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
    }
}
