using EmployeeTaskManagement.API.Data;
using EmployeeTaskManagement.API.DTOs;
using EmployeeTaskManagement.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace EmployeeTaskManagement.Tests
{
    public class EmployeeServiceTests
    {
        private AppDbContext CreateDbContext()

        {
            
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);


        }
        [Fact]
        public void CreateEmployee_ShouldCreateEmployee_WhenDataIsValid()
        {
            // Arrange
            using var context = CreateDbContext();

            var service = new EmployeeService(
                context,
                NullLogger<EmployeeService>.Instance);

            var createEmployeeDto = new CreateEmployeeDto
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test.user@example.com",
                Phone = "0771234567",
                Position = "Developer",
                HireDate = new DateTime(2025, 1, 1),
                DepartmentId = null
            };

            // Act
            var result = service.CreateEmployee(createEmployeeDto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("test.user@example.com", result.Data.Email);
            Assert.Equal(1, context.Employees.Count());
        }

        [Fact]
        public void CreateEmployee_ShouldFail_WhenEmailAlreadyExists()
        {
            // Arrange
            using var context = CreateDbContext();

            context.Employees.Add(new EmployeeTaskManagement.API.Models.Employee
            {
                FirstName = "Existing",
                LastName = "User",
                Email = "existing.user@example.com",
                Position = "Developer",
                HireDate = new DateTime(2024, 1, 1)
            });

            context.SaveChanges();

            var service = new EmployeeService(
                context,
                NullLogger<EmployeeService>.Instance);

            var createEmployeeDto = new CreateEmployeeDto
            {
                FirstName = "New",
                LastName = "User",
                Email = "existing.user@example.com",
                Position = "QA Engineer",
                HireDate = new DateTime(2025, 1, 1)
            };

            // Act
            var result = service.CreateEmployee(createEmployeeDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("An employee with this email already exists.", result.Message);
            Assert.Equal(1, context.Employees.Count());
        }

        [Fact]
        public void CreateEmployee_ShouldFail_WhenDepartmentDoesNotExist()
        {
            // Arrange
            using var context = CreateDbContext();

            var service = new EmployeeService(
                context,
                NullLogger<EmployeeService>.Instance);

            var createEmployeeDto = new CreateEmployeeDto
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test.department@example.com",
                Position = "Developer",
                HireDate = new DateTime(2025, 1, 1),
                DepartmentId = 999
            };

            // Act
            var result = service.CreateEmployee(createEmployeeDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("The selected department does not exist.", result.Message);
            Assert.Equal(0, context.Employees.Count());
        }

        [Fact]
        public void UpdateEmployee_ShouldUpdateEmployee_WhenDataIsValid()
        {
            // Arrange
            using var context = CreateDbContext();

            var employee = new EmployeeTaskManagement.API.Models.Employee
            {
                FirstName = "Old",
                LastName = "Name",
                Email = "old.email@example.com",
                Position = "Junior Developer",
                HireDate = new DateTime(2024, 1, 1)
            };

            context.Employees.Add(employee);
            context.SaveChanges();

            var service = new EmployeeService(
                context,
                NullLogger<EmployeeService>.Instance);

            var updateEmployeeDto = new UpdateEmployeeDto
            {
                FirstName = "New",
                LastName = "Name",
                Email = "new.email@example.com",
                Position = "Senior Developer",
                HireDate = new DateTime(2024, 1, 1)
            };

            // Act
            var result = service.UpdateEmployee(employee.Id, updateEmployeeDto);

            // Assert
            Assert.True(result.Success);

            var updatedEmployee = context.Employees.First(e => e.Id == employee.Id);

            Assert.Equal("New", updatedEmployee.FirstName);
            Assert.Equal("new.email@example.com", updatedEmployee.Email);
            Assert.Equal("Senior Developer", updatedEmployee.Position);
        }

        [Fact]
        public void UpdateEmployee_ShouldFail_WhenEmployeeDoesNotExist()
        {
            // Arrange
            using var context = CreateDbContext();

            var service = new EmployeeService(
                context,
                NullLogger<EmployeeService>.Instance);

            var updateEmployeeDto = new UpdateEmployeeDto
            {
                FirstName = "Missing",
                LastName = "User",
                Email = "missing.user@example.com",
                Position = "Developer",
                HireDate = new DateTime(2025, 1, 1)
            };

            // Act
            var result = service.UpdateEmployee(999, updateEmployeeDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Employee not found.", result.Message);
        }

        [Fact]
        public void DeleteEmployee_ShouldDeleteEmployee_WhenEmployeeExists()
        {
            // Arrange
            using var context = CreateDbContext();

            var employee = new EmployeeTaskManagement.API.Models.Employee
            {
                FirstName = "Delete",
                LastName = "Me",
                Email = "delete.me@example.com",
                Position = "Developer",
                HireDate = new DateTime(2024, 1, 1)
            };

            context.Employees.Add(employee);
            context.SaveChanges();

            var service = new EmployeeService(
                context,
                NullLogger<EmployeeService>.Instance);

            // Act
            var result = service.DeleteEmployee(employee.Id);

            // Assert
            Assert.True(result);
            Assert.Equal(0, context.Employees.Count());
        }

        [Fact]
        public void DeleteEmployee_ShouldReturnFalse_WhenEmployeeDoesNotExist()
        {
            // Arrange
            using var context = CreateDbContext();

            var service = new EmployeeService(
                context,
                NullLogger<EmployeeService>.Instance);

            // Act
            var result = service.DeleteEmployee(999);

            // Assert
            Assert.False(result);
        }
    }
}