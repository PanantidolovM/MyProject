using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Test Authorize bang JWT token controller
[ApiController]
[Route("api/[controller]")] // /api/admin
[Authorize]
public class AdminController : ControllerBase
{
    [HttpGet("dashboard")]
    [Authorize(Roles = "admin")]
    public IActionResult GetAdminDashboard()
    {
        return Ok("Dữ liệu dashboard dành cho Admin.");
    }
}
