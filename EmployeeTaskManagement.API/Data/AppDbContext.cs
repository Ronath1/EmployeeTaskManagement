using EmployeeTaskManagement.API.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;
using EmployeeTaskManagement.API.Models;

namespace EmployeeTaskManagement.API.Data

{
    public class AppDbContext : IdentityDbContext<ApplicationUser>

    {
     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)   //This receives configuration, like which database provider to use and what connection string to use.
                                                                                   //: base(options) <-  This passes the configuration to the parent DbContext class.    

        {
        }
        public DbSet<Employee> Employees { get; set; }  // This represents the Employees table in the database.
        public DbSet<Department> Departments { get; set; }  // This represents the Departments table in the database.
        public DbSet<Project> Projects { get; set; } // This represents the Projects table in the database.
        public DbSet<WorkTask> WorkTasks { get; set; }

    }
}
