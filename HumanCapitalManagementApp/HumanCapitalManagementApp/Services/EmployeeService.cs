using HumanCapitalManagementApp.Data;
using HumanCapitalManagementApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HumanCapitalManagementApp.Services
{
    public class EmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        // Constructor with dependency injection for DB context and Identity user manager
        public EmployeeService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Retrieves a paginated list of employees with optional filtering by search term, department, and employee type
        public async Task<(List<Employee> Employees, int TotalCount)> GetEmployees(
            string? searchTerm,
            int? departmentId,
            int? employeeTypeId,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Include(e => e.EmployeeType)
                .AsQueryable();

            // Filter by name if search term is provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e => e.FullName.Contains(searchTerm));
            }

            // Filter by department if departmentId is provided
            if (departmentId.HasValue && departmentId.Value > 0)
            {
                query = query.Where(e => e.DepartmentId == departmentId.Value);
            }

            // Filter by employee type if provided
            if (employeeTypeId.HasValue && employeeTypeId.Value > 0)
            {
                query = query.Where(e => e.EmployeeTypeId == employeeTypeId.Value);
            }

            // Count total matching employees (before pagination)
            var totalCount = await query.CountAsync();

            // Fetch paginated result
            var employees = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (employees, totalCount);
        }

        // Retrieve single employee by ID, including relations
        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Include(e => e.EmployeeType)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // Create a new employee record in the database
        public async Task CreateEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        // Update an existing employee
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Delete employee by ID, also deletes associated IdentityUser if exists
        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await GetEmployeeByIdAsync(id);
            if (employee != null)
            {
                // If employee is linked to Identity account, delete both employee and user account
                if (!string.IsNullOrEmpty(employee.IdentityUserId))
                {
                    var user = await _userManager.FindByIdAsync(employee.IdentityUserId);
                    if (user != null)
                    {
                        // First delete employee record
                        _context.Employees.Remove(employee);
                        await _context.SaveChangesAsync();

                        // Then delete identity user account
                        await _userManager.DeleteAsync(user);
                        return;
                    }
                }

                // If no IdentityUser linked, only delete employee
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        // Fetch all departments for use in forms (dropdowns)
        public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await _context.Departments.AsNoTracking().ToListAsync();
        }

        // Fetch all employee types (Permanent, Intern, etc.)
        public async Task<List<EmployeeType>> GetEmployeeTypesAsync()
        {
            return await _context.EmployeeTypes.AsNoTracking().ToListAsync();
        }

        // Fetch designations by department for cascading dropdown functionality
        public async Task<List<Designation>> GetDesignationsByDepartmentAsync(int departmentId)
        {
            return await _context.Designations
                .Where(d => d.DepartmentId == departmentId)
                .AsNoTracking()
                .ToListAsync();
        }

        // Retrieve employee by email address (used for logged-in user profile fetch)
        public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Include(e => e.EmployeeType)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        // Retrieve employees by department (used for Manager view), supports search and pagination
        public async Task<(List<Employee> Employees, int TotalCount)> GetEmployeesByDepartment(
            int departmentId,
            string? searchTerm,
            int? employeeTypeId,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Include(e => e.EmployeeType)
                .Where(e => e.DepartmentId == departmentId)
                .AsQueryable();

            // Optional search filter by name
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e => e.FullName.Contains(searchTerm));
            }

            // Optional filter by employee type
            if (employeeTypeId.HasValue && employeeTypeId.Value > 0)
            {
                query = query.Where(e => e.EmployeeTypeId == employeeTypeId.Value);
            }

            var totalCount = await query.CountAsync();

            var employees = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (employees, totalCount);
        }
    }
}

