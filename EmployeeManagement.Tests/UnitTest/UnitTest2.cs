using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Core.DomainServices  ;
using EmployeeManagement.Core.Enities;
using Xunit;

namespace EmployeeManagement.Tests.UnitTest;

public class EmployeeDomainServiceTests
{
    [Fact]
    public void AddEmployee_ShouldAddEmployee_WhenValidEmployee()
    {
        // Arrange
        var repositoryMock = new Mock<IEmployeeRepository>();
        var employeeService = new EmployeeDomainService(repositoryMock.Object);
        var employee = new Employee(
            123456789, // Id
            "John", // FirstName
            "Doe", // LastName
            "Japanese", // KokuSeki
            "123456789", // Passport
            "Engineer", // Shikaku
            "987654321", // MyNumber
            "No remarks", // BiKou
            "Tokyo", // JuuSho
            "090-1234-5678", // Keitai
            "vothaibaominh1502@gmail.com", // Mail
            50000m // Salary
        );

        // Act
        employeeService.AddEmployee(employee);

        // Assert
        var addedEmployee = employeeService.GetEmployeeById(1);
        Assert.NotNull(addedEmployee);
        Assert.Equal("John Doe", addedEmployee.Name);
    }

    private class Mock<T>
    {
        public Mock()
        {
        }

        public IEmployeeRepository Object { get; internal set; }
    }
}

internal class FactAttribute : Attribute
{
}