using EmployeeManagement.Core.Enities;
using EmployeeManagement.Core.Interfaces;
using Microsoft.Extensions.Logging;


namespace EmployeeManagement.Core.DomainServices;
public class LoggingDecorator : IEmployeeDomainService{
    private readonly IEmployeeDomainService _innerService;
    private readonly ILogger<LoggingDecorator> _logger;

    public LoggingDecorator(IEmployeeDomainService innerService,ILogger<LoggingDecorator> logger){
        _innerService = innerService;
        _logger = logger;
    }

    public Employee GetEmployeeDetails(int id){
        Employee employee = _innerService.GetEmployeeDetails(id);
        return employee;
    }
}

