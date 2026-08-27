# Employee Task Management System - Learning Progress

Last updated: 2026-08-27

## Project Goal

Build a complete, professional Employee & Task Management System as a learning and portfolio project for a .NET software engineering job.

Important learning rule:

- The student types the code manually in Visual Studio 2022.
- Codex should guide step by step.
- Codex should not directly edit source code unless explicitly asked.
- If errors happen, Codex should explain the issue, point to the location, and give the fix for the student to type.

## Current Step

We are currently refactoring the employee feature into a service layer.

Latest completed main-thread step:

- `IEmployeeService` created.
- `EmployeeService` created.
- `EmployeeService.GetEmployees(...)` added.
- `EmployeeService.GetEmployeeById(int id)` added.

Next required step:

- Implement `CreateEmployee(CreateEmployeeDto createEmployeeDto)` inside `EmployeeService`.

## Current Backend Progress

Completed:

- Created ASP.NET Core Web API project.
- Enabled Swagger/OpenAPI.
- Created `Employee` model.
- Built initial in-memory Employee CRUD.
- Installed Entity Framework Core SQL Server packages.
- Created `AppDbContext`.
- Added SQL Server LocalDB connection string.
- Registered `AppDbContext` in `Program.cs`.
- Created first EF Core migration.
- Created SQL Server LocalDB database.
- Converted Employee CRUD from in-memory list to EF Core.
- Created `Department` model.
- Added Employee-to-Department relationship.
- Created Department migration.
- Created Department CRUD controller.
- Fixed JSON object cycle issue temporarily using JSON options.
- Added DTOs and removed the JSON cycle workaround.
- Created `Project` model.
- Created Project migration.
- Created Project CRUD controller.
- Created `WorkTask` model.
- Created WorkTask migration.
- Created WorkTask CRUD controller.
- Added DTOs for Employees, Departments, Projects, and WorkTasks.
- Updated main controllers to use DTOs.
- Added duplicate employee email validation.
- Added validation for invalid Employee `DepartmentId`.
- Added validation for invalid Project `ManagerId`.
- Added validation for invalid WorkTask `EmployeeId` and `ProjectId`.
- Added Employee search/filtering.
- Added Employee sorting and pagination.
- Added `PagedResultDto<T>`.
- Added Employee pagination metadata.
- Added Project search/filtering.
- Added WorkTask search/filtering.
- Started service layer refactor for Employee.

## Existing Main Entities

- `Employee`
- `Department`
- `Project`
- `WorkTask`

## Existing Main DTOs

- `EmployeeDto`
- `CreateEmployeeDto`
- `UpdateEmployeeDto`
- `DepartmentDto`
- `CreateDepartmentDto`
- `UpdateDepartmentDto`
- `ProjectDto`
- `CreateProjectDto`
- `UpdateProjectDto`
- `WorkTaskDto`
- `CreateWorkTaskDto`
- `UpdateWorkTaskDto`
- `PagedResultDto<T>`

## Existing Main Controllers

- `EmployeesController`
- `DepartmentsController`
- `ProjectsController`
- `WorkTasksController`

## Service Layer Progress

Created:

- `Services/IEmployeeService.cs`
- `Services/EmployeeService.cs`

Implemented in `EmployeeService`:

- `GetEmployees(...)`
- `GetEmployeeById(int id)`

Still to implement in `EmployeeService`:

- `CreateEmployee(CreateEmployeeDto createEmployeeDto)`
- `UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)`
- `DeleteEmployee(int id)`

Then:

- Register `IEmployeeService` and `EmployeeService` in `Program.cs`.
- Refactor `EmployeesController` to call `IEmployeeService`.

## Remaining Backend Roadmap

1. Finish `EmployeeService`.
2. Register `IEmployeeService` in Dependency Injection.
3. Refactor `EmployeesController` to use `IEmployeeService`.
4. Consider improving service return types for validation errors.
5. Add service layers for Departments, Projects, and WorkTasks.
6. Convert database calls to async/await.
7. Add enums for project status, task status, and task priority.
8. Add global exception handling middleware.
9. Add structured API error responses.
10. Add ASP.NET Core Identity.
11. Add JWT authentication.
12. Add role-based authorization.
13. Add unit tests.
14. Add integration tests.
15. Add React frontend.
16. Prepare Azure App Service and Azure SQL deployment.

## Important Recent Issue

Problem:

The API returned:

```text
System.Text.Json.JsonException: A possible object cycle was detected.
```

Cause:

Entity navigation properties created a loop:

```text
Employee -> Department -> Employees -> Department -> Employees
```

Temporary fix:

- Used `ReferenceHandler.IgnoreCycles`.

Professional fix:

- Added DTOs and changed API responses to return DTOs instead of EF Core entities.

Current status:

- JSON cycle workaround was removed.
- Employee, Department, Project, and WorkTask DTO CRUD works.

