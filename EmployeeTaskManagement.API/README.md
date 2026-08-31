# Employee & Task Management System

A full-stack Employee and Task Management System built as a .NET portfolio project.

The goal of this project is to demonstrate backend development using ASP.NET Core Web API, Entity Framework Core, SQL Server, authentication, authorization, and RESTful API design.

## Tech Stack

- C#
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server / LocalDB
- ASP.NET Core Identity
- JWT Authentication
- Role-Based Authorization
- Swagger / OpenAPI
- React.js
- Azure

## Current Features

- Employee CRUD operations
- Department CRUD operations
- Project CRUD operations
- Task CRUD operations
- SQL Server database persistence
- EF Core migrations
- Entity relationships
- DTO-based API responses
- Search and filtering
- Sorting and pagination for employees
- Duplicate employee email validation
- Relationship validation
- Global exception handling
- Structured logging
- User registration
- User login
- JWT token generation
- Role-based authorization
- Current logged-in user endpoint
- Swagger JWT testing support

## Roles

### Admin

Can manage employees, departments, projects, and tasks.

### Manager

Can view employees and manage projects and tasks.

### Employee

Can view employees, projects, and tasks.

## API Areas

- `/api/Auth`
- `/api/Employees`
- `/api/Departments`
- `/api/Projects`
- `/api/WorkTasks`

## Architecture

The current backend uses a simple layered structure:

```text
Controllers
  ↓
Services
  ↓
Entity Framework Core / DbContext
  ↓
SQL Server