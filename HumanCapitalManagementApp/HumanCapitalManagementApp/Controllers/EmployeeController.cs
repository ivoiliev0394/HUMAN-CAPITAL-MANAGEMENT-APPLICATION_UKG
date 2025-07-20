using HumanCapitalManagementApp.Models;
using HumanCapitalManagementApp.Services;
using HumanCapitalManagementApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HumanCapitalManagementApp.Controllers
{
    // Only authenticated users can access this controller
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;
        private readonly UserManager<IdentityUser> _userManager;

        // Injecting EmployeeService and UserManager for accessing employees and identity users
        public EmployeeController(EmployeeService employeeService, UserManager<IdentityUser> userManager)
        {
            _employeeService = employeeService;
            _userManager = userManager;
        }

        // Redirect user to correct view based on their role
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Employee")) return RedirectToAction("Details", "Employee");
            if (User.IsInRole("Manager") || User.IsInRole("HR Admin")) return RedirectToAction("List", "Employee");
            return View();
        }

        // List employees, filtered by role (Managers see their department, HR Admin sees all)
        [Authorize(Roles = "Manager, HR Admin")]
        [HttpGet]
        public async Task<IActionResult> List(string? searchTerm, int? SelectedDepartmentId, int? SelectedEmployeeTypeId, int pageNumber = 1, int pageSize = 5)
        {
            if (User.IsInRole("HR Admin"))
            {
                // HR Admin can see all employees with filters
                var (employees, totalCount) = await _employeeService.GetEmployees(searchTerm, SelectedDepartmentId, SelectedEmployeeTypeId, pageNumber, pageSize);
                var viewModel = await BuildEmployeeListViewModel(employees, totalCount, searchTerm, SelectedDepartmentId, SelectedEmployeeTypeId, pageNumber, pageSize);
                return View(viewModel);
            }
            else if (User.IsInRole("Manager"))
            {
                // Managers only see employees from their department
                var currentUserEmail = User.Identity!.Name!;
                var manager = await _employeeService.GetEmployeeByEmailAsync(currentUserEmail);
                if (manager == null) return Forbid();

                var (employees, totalCount) = await _employeeService.GetEmployeesByDepartment(manager.DepartmentId, searchTerm, SelectedEmployeeTypeId, pageNumber, pageSize);
                var viewModel = await BuildEmployeeListViewModel(employees, totalCount, searchTerm, manager.DepartmentId, SelectedEmployeeTypeId, pageNumber, pageSize);
                return View(viewModel);
            }
            return Forbid();
        }

        // Employee can view their own details
        [Authorize(Roles = "Employee")]
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var userEmail = User.Identity!.Name!;
            var employee = await _employeeService.GetEmployeeByEmailAsync(userEmail);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // Managers and HR Admin can view any employee by ID
        [Authorize(Roles = "Manager, HR Admin")]
        [HttpGet]
        public async Task<IActionResult> DetailsById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();
            return View("Details", employee);
        }

        // HR Admin can open the Create employee page
        [Authorize(Roles = "HR Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Prepare the ViewModel with dropdown lists
            var vm = new EmployeeCreateUpdateViewModel
            {
                Departments = await _employeeService.GetDepartmentsAsync(),
                EmployeeTypes = await _employeeService.GetEmployeeTypesAsync(),
                Designations = new List<Designation>(),
                Roles = new List<string> { "Employee", "Manager", "HR Admin" }
            };
            return View(vm);
        }

        // HR Admin submits a form to create new Employee + IdentityUser
        [Authorize(Roles = "HR Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateUpdateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // Create Employee entity
                var employee = new Employee
                {
                    FullName = vm.FullName,
                    Email = vm.Email,
                    DepartmentId = vm.DepartmentId,
                    DesignationId = vm.DesignationId,
                    HireDate = vm.HireDate,
                    DateOfBirth = vm.DateOfBirth,
                    EmployeeTypeId = vm.EmployeeTypeId,
                    Gender = vm.Gender,
                    Salary = vm.Salary,
                    Password = vm.Password
                };

                // Create IdentityUser
                var user = new IdentityUser
                {
                    UserName = vm.Email,
                    Email = vm.Email,
                    EmailConfirmed = true
                };

                // Create Identity user account
                var result = await _userManager.CreateAsync(user, vm.Password);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to create Identity account");
                    vm.Departments = await _employeeService.GetDepartmentsAsync();
                    vm.EmployeeTypes = await _employeeService.GetEmployeeTypesAsync();
                    vm.Designations = new List<Designation>();
                    vm.Roles = new List<string> { "Employee", "Manager", "HR Admin" };
                    return View(vm);
                }

                // Assign chosen role
                await _userManager.AddToRoleAsync(user, vm.Role);

                // Link Employee with Identity account
                employee.IdentityUserId = user.Id;
                await _employeeService.CreateEmployeeAsync(employee);
                return RedirectToAction("Success", new { id = employee.Id });
            }

            // Reload dropdowns on error
            vm.Departments = await _employeeService.GetDepartmentsAsync();
            vm.EmployeeTypes = await _employeeService.GetEmployeeTypesAsync();
            vm.Designations = new List<Designation>();
            vm.Roles = new List<string> { "Employee", "Manager", "HR Admin" };
            return View(vm);
        }

        // Manager and HR Admin can update employees
        [Authorize(Roles = "Manager, HR Admin")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();

            var user = await _userManager.FindByIdAsync(employee.IdentityUserId!);
            if (user == null) return NotFound();

            // Get current role to pre-select in form
            var userRoles = await _userManager.GetRolesAsync(user);
            var currentRole = userRoles.FirstOrDefault();

            // Prepare update form ViewModel
            var vm = new EmployeeCreateUpdateViewModel
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                DesignationId = employee.DesignationId,
                HireDate = employee.HireDate,
                DateOfBirth = employee.DateOfBirth,
                EmployeeTypeId = employee.EmployeeTypeId,
                Gender = employee.Gender,
                Salary = employee.Salary,
                Role = currentRole,
                Roles = new List<string> { "Employee", "Manager", "HR Admin" },
                Password = null, // Password remains blank on edit
                Departments = await _employeeService.GetDepartmentsAsync(),
                EmployeeTypes = await _employeeService.GetEmployeeTypesAsync(),
                Designations = await _employeeService.GetDesignationsByDepartmentAsync(employee.DepartmentId)
            };

            return View(vm);
        }

        // POST update logic
        [Authorize(Roles = "Manager, HR Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(EmployeeCreateUpdateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(vm.Id!.Value);
                if (employee == null) return NotFound();

                var user = await _userManager.FindByIdAsync(employee.IdentityUserId!);
                if (user == null) return NotFound();

                // Update employee data
                employee.FullName = vm.FullName;
                employee.Email = vm.Email;
                employee.DepartmentId = vm.DepartmentId;
                employee.DesignationId = vm.DesignationId;
                employee.HireDate = vm.HireDate;
                employee.DateOfBirth = vm.DateOfBirth;
                employee.EmployeeTypeId = vm.EmployeeTypeId;
                employee.Gender = vm.Gender;
                employee.Salary = vm.Salary;

                // Update IdentityUser email
                user.Email = vm.Email;
                user.UserName = vm.Email;
                await _userManager.UpdateAsync(user);

                // Update user role
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, vm.Role);

                // Change password if provided
                if (!string.IsNullOrWhiteSpace(vm.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, vm.Password);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Error changing password.");
                        vm.Departments = await _employeeService.GetDepartmentsAsync();
                        vm.EmployeeTypes = await _employeeService.GetEmployeeTypesAsync();
                        vm.Designations = new List<Designation>();
                        return View(vm);
                    }
                }

                await _employeeService.UpdateEmployeeAsync(employee);
                TempData["Message"] = $"Employee {employee.FullName} updated successfully!";
                return RedirectToAction("List");
            }

            // Reload dropdowns on validation error
            vm.Departments = await _employeeService.GetDepartmentsAsync();
            vm.EmployeeTypes = await _employeeService.GetEmployeeTypesAsync();
            vm.Designations = new List<Designation>();
            return View(vm);
        }

        // GET: Delete confirmation
        [Authorize(Roles = "Manager, HR Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // POST: Confirm delete
        [Authorize(Roles = "Manager, HR Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();
            await _employeeService.DeleteEmployeeAsync(id);
            TempData["Message"] = $"Employee with ID {id} and Name {employee.FullName} has been deleted.";
            return RedirectToAction("List");
        }

        // Returns designations for selected department via JSON (used in AJAX dropdown)
        [HttpGet]
        public async Task<JsonResult> GetDesignations(int departmentId)
        {
            var designations = await _employeeService.GetDesignationsByDepartmentAsync(departmentId);
            var result = designations.Select(d => new { id = d.Id, name = d.Name }).ToList();
            return Json(result);
        }

        // Success page after employee creation
        [HttpGet]
        public async Task<IActionResult> Success(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // Builds EmployeeListViewModel for listing employees
        private async Task<EmployeeListViewModel> BuildEmployeeListViewModel(List<Employee> employees, int totalCount, string? searchTerm, int? selectedDepartmentId, int? selectedEmployeeTypeId, int pageNumber, int pageSize)
        {
            return new EmployeeListViewModel
            {
                Employees = employees,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Total = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                SearchTerm = searchTerm,
                SelectedDepartmentId = selectedDepartmentId,
                SelectedEmployeeTypeId = selectedEmployeeTypeId,
                Departments = await _employeeService.GetDepartmentsAsync(),
                EmployeeTypes = await _employeeService.GetEmployeeTypesAsync()
            };
        }

    }
}
