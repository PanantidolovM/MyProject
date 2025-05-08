using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Tests.UnitTest;
namespace ApiTestExample
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;
        
        public UsersController(ILogger<UsersController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        
        // GET api/users
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            _logger.LogInformation("Controller: Lấy danh sách user");
            var users = _userRepository.GetUsers();
            return Ok(users);
        }
        // Thêm các endpoint khác nếu cần (POST, PUT, DELETE)
    }
}
