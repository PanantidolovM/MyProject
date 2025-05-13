using Xunit;
using Moq;
using EmployeeManagement.Core.Enities;

namespace EmployeeManagement.Tests.UnitTest;

public class EmployeeDomainServiceTests
{
    [Fact]
    public void AddEmployee_ShouldAddEmployee_WhenValidEmployee()
    {
        // Arrange
        var repositoryMock = new Mock<List<Employee>>();
        var employeeService = new List<Employee>(repositoryMock.Object);
        var employee = new Employee(
            id: 1,
            firstname: "John",
            lastname: "Doe",
            kokuseki: "Japanese",
            passport: "BA3456789",
            shikaku: "Engineer",
            mynumber: "987654321",
            bikou: "No remarks",
            juusho: "Tokyo",
            keitai: "090-1234-5678",
            mail: "vothaibaominh1502@gmail.com",
            salary: 50000m
        );

        // Act
        employeeService.Add(employee);

        repositoryMock.Setup(r => r.Add(It.IsAny<Employee>()))
              .Callback<Employee>(e => repositoryMock.Setup(r => r.FirstOrDefault(e => e.Id == employee.Id)).Returns(e))
              .Verifiable(); // 設定したモックを検証するための設定

        // Assert
        var addedEmployee = employeeService.FirstOrDefault(e => e.Id == employee.Id);
        Assert.NotNull(addedEmployee);
        Assert.Equal(employee.Id, addedEmployee.Id);
        Assert.Equal(employee.FirstName, addedEmployee.FirstName);
        Assert.Equal(employee.LastName, addedEmployee.LastName);
        Assert.Equal(employee.KokuSeki, addedEmployee.KokuSeki);
        Assert.Equal(employee.Passport, addedEmployee.Passport);
        Assert.Equal(employee.Shikaku, addedEmployee.Shikaku);
        Assert.Equal(employee.MyNumber, addedEmployee.MyNumber);
        Assert.Equal(employee.BiKou, addedEmployee.BiKou);
        Assert.Equal(employee.JuuSho, addedEmployee.JuuSho);
        Assert.Equal(employee.Keitai, addedEmployee.Keitai);
        Assert.Equal(employee.Mail, addedEmployee.Mail);
        Assert.Equal(employee.Salary, addedEmployee.Salary);
    }

    [Fact]
    public void GetEmployeeDetails_ShouldReturnEmployee_WhenValidId()
    {
        // Arrange
        var repositoryMock = new Mock<List<Employee>>();
        var employeeService = new List<Employee>(repositoryMock.Object);
        var employee = new Employee(
            id: 1,
            firstname: "John",
            lastname: "Doe",
            kokuseki: "Japanese",
            passport: "BA3456789",
            shikaku: "Engineer",
            mynumber: "987654321",
            bikou: "No remarks",
            juusho: "Tokyo",
            keitai: "090-1234-5678",
            mail: "vothaibaominh1502@gmail.com",
            salary: 50000m
        );


        // Act
        var result = employeeService.FirstOrDefault(e => e.Id == employee.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(employee.Id, result.Id);
        Assert.Equal(employee.FirstName, result.FirstName);
        Assert.Equal(employee.LastName, result.LastName);
        Assert.Equal(employee.KokuSeki, result.KokuSeki);
        Assert.Equal(employee.Passport, result.Passport);
        Assert.Equal(employee.Shikaku, result.Shikaku);
        Assert.Equal(employee.MyNumber, result.MyNumber);
        Assert.Equal(employee.BiKou, result.BiKou);
        Assert.Equal(employee.JuuSho, result.JuuSho);
        Assert.Equal(employee.Keitai, result.Keitai);
        Assert.Equal(employee.Mail, result.Mail);
        Assert.Equal(employee.Salary, result.Salary);
    }

    [Fact]
    public void UpdateEmployee_ShouldUpdateEmployee_WhenValidEmployee()
    {
        // Arrange
        var repositoryMock = new Mock<List<Employee>>();
        var employeeService = new List<Employee>(repositoryMock.Object);
        var employee = new Employee(
            id: 1,
            firstname: "John",
            lastname: "Doe",
            kokuseki: "Japanese",
            passport: "BA3456789",
            shikaku: "Engineer",
            mynumber: "987654321",
            bikou: "No remarks",
            juusho: "Tokyo",
            keitai: "090-1234-5678",
            mail: "vothaibaominh1502@gmail.com",
            salary: 50000m
        );

        // Setup the repository to return the employee when GetById is called
        repositoryMock.Setup(repo => repo.FirstOrDefault(e => e.Id == employee.Id)).Returns(employee);
        employeeService.Add(employee);

        // Act
        employee.FirstName = "Jane";
        employee.LastName = "Smith";
        employeeService.AddRange(employee);

        // Assert
        var updatedEmployee = employeeService.FirstOrDefault(e => e.Id == employee.Id);
        Assert.NotNull(updatedEmployee);
        Assert.Equal(employee.FirstName, updatedEmployee.FirstName);
        Assert.Equal(employee.LastName, updatedEmployee.LastName);
    }

    [Fact]
    public void DelEmployee_ShouldDeleteEmployee_WhenValidId()
    {
        // Arrange
        var repositoryMock = new Mock<List<Employee>>();
        var employeeService = new List<Employee>(repositoryMock.Object);
        var employee = new Employee(
            id: 1,
            firstname: "John",
            lastname: "Doe",
            kokuseki: "Japanese",
            passport: "BA3456789",
            shikaku: "Engineer",
            mynumber: "987654321",
            bikou: "No remarks",
            juusho: "Tokyo",
            keitai: "090-1234-5678",
            mail: "vothaibaominh1502@gmail.com",
            salary: 50000m
        );

        // Setup the repository to return the employee when GetById is called
        repositoryMock.Setup(repo => repo.FirstOrDefault(e => e.Id == employee.Id)).Returns(employee);
        repositoryMock.Setup(repo => repo.RemoveAt(1))
                  .Callback(() => repositoryMock.Setup(r => r.FirstOrDefault(e => e.Id == employee.Id)).Throws<KeyNotFoundException>())
                  .Verifiable();  
        // Act
        employeeService.Remove(employee);

        // Assert
        repositoryMock.Verify(repo => repo.RemoveAt(1), Times.Once);
        Assert.Throws<KeyNotFoundException>(() => employeeService.FirstOrDefault(e => e.Id == employee.Id));
    }
}
