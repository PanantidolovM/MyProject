using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Tests.ApiTests;

namespace EmpApiTestController
{
    [ApiController]
    [Route("api/emp")]
    public class EmpController : ControllerBase
    {
        private readonly ILogger<EmpController> _logger;
        private readonly EmployeeApiTester _employeeApiTester;
        
        public EmpController(ILogger<EmpController> logger, EmployeeApiTester employeeApiTester)
        {
            _logger = logger;
            _employeeApiTester = employeeApiTester;
        }

        // POST api/emp/add
        [HttpPost("add")]
        public async Task<IActionResult> AddEmployee()
        {
            _logger.LogInformation("Controller: Thêm nhân viên mới");
           
            await _employeeApiTester.TestCreateEmployee();
            if (_employeeApiTester == null)
            {
                return BadRequest(Response.StatusCode = 400);
            }
            return Ok(Response.StatusCode = 200);
        }
        
        // GET api/emp/getAll
        [HttpGet("getAll")]
        public async Task<IActionResult> GetEmployees()
        {
            _logger.LogInformation("Controller: Lấy danh sách nhân viên");
            await _employeeApiTester.TestGetEmployees();
            if (_employeeApiTester == null)
            {
                return BadRequest(Response.StatusCode = 400);
            }
            return Ok(Response.StatusCode = 200);
        }

        // GET api/emp/get/{id}
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetEmployeeDetails(int id)
        {
            _logger.LogInformation($"Controller: Lấy thông tin nhân viên với ID: {id}");
            await _employeeApiTester.TestGetEmployeeDetails();
            if (_employeeApiTester == null)
            {
                return BadRequest(Response.StatusCode = 400);
            }
            return Ok(Response.StatusCode = 200);
        }
        // Thêm các endpoint khác nếu cần (POST, PUT, DELETE)
    }
}
