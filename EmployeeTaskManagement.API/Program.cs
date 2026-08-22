using EmployeeTaskManagement.API.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));    // builder.Services<- This is where we register services for Dependency Injection.



builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


/*  Important Concepts
builder.Services
This is where we register services for Dependency Injection.
AddDbContext<AppDbContext>()
This tells ASP.NET Core:
When something asks for AppDbContext, create one.
UseSqlServer(...)
This tells EF Core to use SQL Server.
builder.Configuration.GetConnectionString("DefaultConnection")
This reads the connection string from appsettings.json.   */