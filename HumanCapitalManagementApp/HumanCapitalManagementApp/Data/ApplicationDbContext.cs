using HumanCapitalManagementApp.Models;
using HumanCapitalManagementApp.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace HumanCapitalManagementApp.Data
{
    // ApplicationDbContext inherits from IdentityDbContext to include ASP.NET Core Identity support
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        // DbSets represent tables in the database
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }

        // Configures the database model using Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Prevent cascading delete for Designation foreign key in Employee
            modelBuilder.Entity<Employee>()
            .HasOne(e => e.Designation)
            .WithMany()
            .HasForeignKey(e => e.DesignationId)
            .OnDelete(DeleteBehavior.Restrict);

            // Define a ValueConverter to automatically encrypt/decrypt IBAN values during database operations
            var ibanConverter = new ValueConverter<string, string>(
                // Convert to encrypted string before saving to the database
                v => EncryptionHelper.Encrypt(v),

                // Convert back to plain text when reading from the database
                v => EncryptionHelper.Decrypt(v)
            );

            // Apply the converter to the IBAN property of the Employee entity
            modelBuilder.Entity<Employee>()
                .Property(e => e.EncryptedIBAN)
                .HasConversion(ibanConverter); // Use the defined converter for this property

            // Seed initial data for EmployeeType table
            modelBuilder.Entity<EmployeeType>().HasData(
                new EmployeeType { Id = 1, Name = "Permanent" },
                new EmployeeType { Id = 2, Name = "Temporary" },
                new EmployeeType { Id = 3, Name = "Contract" },
                new EmployeeType { Id = 4, Name = "Intern" }
            );

            // Seed initial data for Department table
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "IT" },
                new Department { Id = 2, Name = "HR" },
                new Department { Id = 3, Name = "Sales" },
                new Department { Id = 4, Name = "Admin" }
            );

            // Seed initial data for Designation table, each linked to a department
            modelBuilder.Entity<Designation>().HasData(
                new Designation { Id = 1, Name = "Software Developer", DepartmentId = 1 },
                new Designation { Id = 2, Name = "System Administrator", DepartmentId = 1 },
                new Designation { Id = 3, Name = "Network Engineer", DepartmentId = 1 },
                new Designation { Id = 4, Name = "HR Specialist", DepartmentId = 2 },
                new Designation { Id = 5, Name = "HR Manager", DepartmentId = 2 },
                new Designation { Id = 6, Name = "Talent Acquisition Coordinator", DepartmentId = 2 },
                new Designation { Id = 7, Name = "Sales Executive", DepartmentId = 3 },
                new Designation { Id = 8, Name = "Sales Manager", DepartmentId = 3 },
                new Designation { Id = 9, Name = "Account Executive", DepartmentId = 3 },
                new Designation { Id = 10, Name = "Office Manager", DepartmentId = 4 },
                new Designation { Id = 11, Name = "Executive Assistant", DepartmentId = 4 },
                new Designation { Id = 12, Name = "Receptionist", DepartmentId = 4 }
            );

            // Seed initial data for Employee table
            modelBuilder.Entity<Employee>().HasData(
                 new Employee { Id = 1, FullName = "John Doe", Email = "john@example.com", Password = "John123!", DepartmentId = 1, DesignationId = 1, HireDate = new DateTime(2020, 1, 15), DateOfBirth = new DateTime(1990, 3, 12), EmployeeTypeId = 1, Gender = "Male", Salary = 60000m, Country = "Bulgaria", CountryCode = "bg", EncryptedIBAN = "BG80BNBG96611020345678" },
                 new Employee { Id = 2, FullName = "Jane Smith", Email = "jane@example.com", Password = "Jane123!", DepartmentId = 2, DesignationId = 5, HireDate = new DateTime(2018, 5, 20), DateOfBirth = new DateTime(1985, 8, 22), EmployeeTypeId = 1, Gender = "Female", Salary = 80000m, Country = "Germany", CountryCode = "de", EncryptedIBAN = "DE89370400440532013000" },
                 new Employee { Id = 3, FullName = "Sam Wilson", Email = "sam@example.com", Password = "Sam123!", DepartmentId = 3, DesignationId = 7, HireDate = new DateTime(2021, 3, 10), DateOfBirth = new DateTime(1992, 6, 30), EmployeeTypeId = 3, Gender = "Male", Salary = 50000m, Country = "France", CountryCode = "fr", EncryptedIBAN = "FR7630006000011234567890189" },
                 new Employee { Id = 4, FullName = "Anna Taylor", Email = "anna@example.com", Password = "Anna123!", DepartmentId = 4, DesignationId = 11, HireDate = new DateTime(2022, 7, 5), DateOfBirth = new DateTime(1995, 11, 15), EmployeeTypeId = 2, Gender = "Female", Salary = 40000m, Country = "Italy", CountryCode = "it", EncryptedIBAN = "IT60X0542811101000000123456" },
                 new Employee { Id = 5, FullName = "Tom Brown", Email = "tom@example.com", Password = "Tom123!", DepartmentId = 1, DesignationId = 3, HireDate = new DateTime(2019, 4, 18), DateOfBirth = new DateTime(1989, 2, 25), EmployeeTypeId = 1, Gender = "Male", Salary = 70000m, Country = "Spain", CountryCode = "es", EncryptedIBAN = "ES9121000418450200051332" },
                 new Employee { Id = 6, FullName = "Emma Davis", Email = "emma@example.com", Password = "Emma123!", DepartmentId = 2, DesignationId = 4, HireDate = new DateTime(2017, 10, 12), DateOfBirth = new DateTime(1987, 9, 10), EmployeeTypeId = 1, Gender = "Female", Salary = 75000m, Country = "Poland", CountryCode = "pl", EncryptedIBAN = "PL61109010140000071219812874" },
                 new Employee { Id = 7, FullName = "Luke Miller", Email = "luke@example.com", Password = "Luke123!", DepartmentId = 3, DesignationId = 8, HireDate = new DateTime(2020, 2, 20), DateOfBirth = new DateTime(1990, 1, 5), EmployeeTypeId = 3, Gender = "Male", Salary = 85000m, Country = "Romania", CountryCode = "ro", EncryptedIBAN = "RO49AAAA1B31007593840000" },
                 new Employee { Id = 8, FullName = "Olivia Johnson", Email = "olivia@example.com", Password = "Olivia123!", DepartmentId = 4, DesignationId = 10, HireDate = new DateTime(2021, 6, 8), DateOfBirth = new DateTime(1993, 4, 18), EmployeeTypeId = 1, Gender = "Female", Salary = 65000m, Country = "Sweden", CountryCode = "se", EncryptedIBAN = "SE3550000000054910000003" },
                 new Employee { Id = 9, FullName = "Mia Moore", Email = "mia@example.com", Password = "Mia123!", DepartmentId = 1, DesignationId = 2, HireDate = new DateTime(2022, 8, 15), DateOfBirth = new DateTime(1997, 12, 20), EmployeeTypeId = 4, Gender = "Female", Salary = 30000m, Country = "Greece", CountryCode = "gr", EncryptedIBAN = "GR1601101250000000012300695" },
                 new Employee { Id = 10, FullName = "Chris Evans", Email = "chris@example.com", Password = "Chris123!", DepartmentId = 2, DesignationId = 6, HireDate = new DateTime(2018, 11, 25), DateOfBirth = new DateTime(1986, 7, 12), EmployeeTypeId = 2, Gender = "Other", Salary = 55000m, Country = "Netherlands", CountryCode = "nl", EncryptedIBAN = "NL91ABNA0417164300" },
                 new Employee { Id = 11, FullName = "Sophia White", Email = "sophia@example.com", Password = "Sophia123!", DepartmentId = 3, DesignationId = 7, HireDate = new DateTime(2019, 9, 10), DateOfBirth = new DateTime(1994, 5, 6), EmployeeTypeId = 1, Gender = "Female", Salary = 52000m, Country = "Belgium", CountryCode = "be", EncryptedIBAN = "BE68539007547034" },
                 new Employee { Id = 12, FullName = "Liam Green", Email = "liam@example.com", Password = "Liam123!", DepartmentId = 4, DesignationId = 12, HireDate = new DateTime(2020, 10, 3), DateOfBirth = new DateTime(1996, 8, 21), EmployeeTypeId = 2, Gender = "Male", Salary = 38000m, Country = "Czech Republic", CountryCode = "cz", EncryptedIBAN = "CZ6508000000192000145399" },
                 new Employee { Id = 13, FullName = "Noah Black", Email = "noah@example.com", Password = "Noah123!", DepartmentId = 1, DesignationId = 2, HireDate = new DateTime(2018, 12, 1), DateOfBirth = new DateTime(1991, 9, 18), EmployeeTypeId = 1, Gender = "Male", Salary = 65000m, Country = "Austria", CountryCode = "at", EncryptedIBAN = "AT611904300234573201" },
                 new Employee { Id = 14, FullName = "Isabella Blue", Email = "isabella@example.com", Password = "Isabella123!", DepartmentId = 2, DesignationId = 4, HireDate = new DateTime(2017, 11, 30), DateOfBirth = new DateTime(1988, 4, 2), EmployeeTypeId = 1, Gender = "Female", Salary = 76000m, Country = "Portugal", CountryCode = "pt", EncryptedIBAN = "PT50000201231234567890154" },
                 new Employee { Id = 15, FullName = "James Brown", Email = "james@example.com", Password = "James123!", DepartmentId = 3, DesignationId = 9, HireDate = new DateTime(2021, 7, 21), DateOfBirth = new DateTime(1993, 3, 17), EmployeeTypeId = 3, Gender = "Male", Salary = 62000m, Country = "Croatia", CountryCode = "hr", EncryptedIBAN = "HR1210010051863000160" }
            );

        }
    }
}
