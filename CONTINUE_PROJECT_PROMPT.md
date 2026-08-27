# Continue This Project Prompt

Use this prompt if continuing the project in a new Codex or ChatGPT account.

---

I am building an Employee & Task Management System as a learning and portfolio project for a .NET software engineering job.

Important rule:

Do not build the project for me automatically. Do not directly edit source code unless I explicitly ask. I want to type the code myself in Visual Studio 2022. Guide me step by step. For each step:

1. Tell me what we are building.
2. Explain why we are building it.
3. Tell me exactly which file to create or open.
4. Give me a manageable code block to type.
5. Explain the important parts of the code.
6. Tell me how to build/run/test it.
7. Tell me what result to expect.
8. Wait for my confirmation before moving to the next step.

If I get an error, help me understand and debug the error before continuing.

## Project Tech Stack

- C#
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server LocalDB
- DTOs
- Dependency Injection
- Service Layer
- RESTful APIs
- Swagger/OpenAPI
- Later: ASP.NET Core Identity, JWT authentication, role-based authorization, React.js, testing, Azure

## Current Project State

The backend project already exists:

```text
EmployeeTaskManagement
EmployeeTaskManagement.API
```

Completed so far:

- ASP.NET Core Web API project created.
- Swagger works.
- EF Core SQL Server packages installed.
- `AppDbContext` created and registered.
- SQL Server LocalDB database created with EF Core migrations.
- Models created:
  - `Employee`
  - `Department`
  - `Project`
  - `WorkTask`
- Relationships added:
  - Department has many Employees.
  - Employee belongs to one Department.
  - Employee can manage Projects.
  - Project has many WorkTasks.
  - Employee has many WorkTasks.
  - WorkTask belongs to one Employee and one Project.
- CRUD controllers created for:
  - Employees
  - Departments
  - Projects
  - WorkTasks
- DTOs created and used for:
  - Employees
  - Departments
  - Projects
  - WorkTasks
- `PagedResultDto<T>` created.
- Duplicate employee email validation added.
- Invalid relationship validation added:
  - Employee `DepartmentId`
  - Project `ManagerId`
  - WorkTask `EmployeeId`
  - WorkTask `ProjectId`
- Employee search/filter/sort/pagination works.
- Employee pagination metadata works.
- Project search/filter works.
- WorkTask search/filter works.
- JSON object cycle issue was solved professionally by using DTOs.

## Exact Current Learning Position

We started service layer refactoring for Employee.

Already created:

```text
Services/IEmployeeService.cs
Services/EmployeeService.cs
```

`IEmployeeService` currently has methods like:

```csharp
PagedResultDto<EmployeeDto> GetEmployees(
    string? search,
    int? departmentId,
    string? position,
    string? sortBy,
    string? sortOrder,
    int pageNumber,
    int pageSize);

EmployeeDto? GetEmployeeById(int id);

EmployeeDto CreateEmployee(CreateEmployeeDto createEmployeeDto);

bool UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto);

bool DeleteEmployee(int id);
```

`EmployeeService` currently has:

- constructor injection for `AppDbContext`
- `GetEmployees(...)`
- `GetEmployeeById(int id)`

## Next Step To Continue

Continue with:

```text
Implement CreateEmployee(CreateEmployeeDto createEmployeeDto) inside EmployeeService.
```

Important:

The current interface returns `EmployeeDto` for create and `bool` for update/delete. We may need to improve this later because validation errors like duplicate email and invalid department ID need clearer service results.

For now, continue beginner-friendly and step by step.

