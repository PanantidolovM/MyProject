using Xunit;
using Moq;
using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Core.DomainServices  ;
using EmployeeManagement.Core.Enities;

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
        var addedEmployee = employeeService.GetEmployeeDetails(1);
        Assert.NotNull(addedEmployee);
        Assert.Equal(123456789, addedEmployee.Id);
        Assert.Equal("John", addedEmployee.FirstName);
        Assert.Equal("Doe", addedEmployee.LastName);
        Assert.Equal("Japanese", addedEmployee.KokuSeki);
        Assert.Equal("123456789", addedEmployee.Passport);
        Assert.Equal("Engineer", addedEmployee.Shikaku);
        Assert.Equal("987654321", addedEmployee.MyNumber);
        Assert.Equal("No remarks", addedEmployee.BiKou);
        Assert.Equal("Tokyo", addedEmployee.JuuSho);
        Assert.Equal("090-1234-5678", addedEmployee.Keitai);
        Assert.Equal("vothaibaominh1502@gmail.com", addedEmployee.Mail);
        Assert.Equal(50000m, addedEmployee.Salary);
    }

    [Fact]
    public void GetEmployeeDetails_ShouldReturnEmployee_WhenValidId()
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
        repositoryMock.Setup(repo => repo.GetById(1)).Returns(employee);

        // Act
        var result = employeeService.GetEmployeeDetails(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(123456789, result.Id);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName);
        Assert.Equal("Japanese", result.KokuSeki);
        Assert.Equal("123456789", result.Passport);
        Assert.Equal("Engineer", result.Shikaku);
        Assert.Equal("987654321", result.MyNumber);
        Assert.Equal("No remarks", result.BiKou);
        Assert.Equal("Tokyo", result.JuuSho);
        Assert.Equal("090-1234-5678", result.Keitai);
        Assert.Equal("vothaibaominh1502@gmail.com", result.Mail);
        Assert.Equal(50000m, result.Salary);
    }

    [Fact]
    public void UpdateEmployee_ShouldUpdateEmployee_WhenValidEmployee()
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
        employeeService.AddEmployee(employee);

        // Act
        employee.FirstName = "Jane";
        employee.LastName = "Smith";
        employeeService.UpdateEmployee(employee);

        // Assert
        var updatedEmployee = employeeService.GetEmployeeDetails(123456789);
        Assert.NotNull(updatedEmployee);
        Assert.Equal("Jane", updatedEmployee.FirstName);
        Assert.Equal("Smith", updatedEmployee.LastName);
    }
    [Fact]
    public void AddEmployee_ShouldThrowArgumentNullException_WhenEmployeeIsNull()
    {
        // Arrange
        var repositoryMock = new Mock<IEmployeeRepository>();
        var employeeService = new EmployeeDomainService(repositoryMock.Object);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => employeeService.AddEmployee(null));
    }
    
    [Fact]
    public void DelEmployee_ShouldDeleteEmployee_WhenValidId()
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
        repositoryMock.Setup(repo => repo.GetById(123456789)).Returns(employee);
        repositoryMock.Setup(repo => repo.DelEmployee(123456789)).Verifiable();  

        // Act
        employeeService.DelEmployee(123456789);

        // Assert
        repositoryMock.Verify(repo => repo.DelEmployee(123456789), Times.Once);
    }
}
