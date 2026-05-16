using EmployeeManagementWeb.Data;
using EmployeeManagementWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementWeb.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        private bool IsLogin() => !string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"));
        private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";
        private string GetLoginUserName() => HttpContext.Session.GetString("UserName") ?? "";

        public IActionResult Index(string? employeeName, string? departmentName, int? employeeId)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");

            var query = _context.Employees.Include(e => e.Department).AsQueryable();
            if (!string.IsNullOrWhiteSpace(employeeName)) query = query.Where(e => e.EmployeeName.Contains(employeeName));
            if (!string.IsNullOrWhiteSpace(departmentName)) query = query.Where(e => e.Department != null && e.Department.DepartmentName.Contains(departmentName));
            if (employeeId.HasValue) query = query.Where(e => e.EmployeeId == employeeId.Value);

            ViewBag.IsAdmin = IsAdmin();
            return View(query.ToList());
        }

        public IActionResult Create() { if (!IsLogin()) return RedirectToAction("Login", "Account"); if (!IsAdmin()) return Forbid(); ViewBag.Departments = new SelectList(_context.Departments.ToList(), "DepartmentId", "DepartmentName"); return View(); }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (!IsAdmin()) return Forbid();
            if (!ModelState.IsValid) { ViewBag.Departments = new SelectList(_context.Departments.ToList(), "DepartmentId", "DepartmentName", employee.DepartmentId); return View(employee); }
            employee.CreatedBy = GetLoginUserName(); employee.UpdatedBy = GetLoginUserName(); employee.CreatedAt = DateTime.UtcNow; employee.UpdatedAt = DateTime.UtcNow; employee.IsDeleted = false;
            _context.Employees.Add(employee); _context.SaveChanges(); return RedirectToAction("Index");
        }

        public IActionResult Edit(int id) { if (!IsLogin()) return RedirectToAction("Login", "Account"); if (!IsAdmin()) return Forbid(); var employee = _context.Employees.Find(id); if (employee == null) return NotFound(); ViewBag.Departments = new SelectList(_context.Departments.ToList(), "DepartmentId", "DepartmentName", employee.DepartmentId); return View(employee); }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (!IsAdmin()) return Forbid();
            if (!ModelState.IsValid) { ViewBag.Departments = new SelectList(_context.Departments.ToList(), "DepartmentId", "DepartmentName", employee.DepartmentId); return View(employee); }
            var target = _context.Employees.IgnoreQueryFilters().FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
            if (target == null) return NotFound();
            target.EmployeeNumber = employee.EmployeeNumber; target.EmployeeName = employee.EmployeeName; target.Email = employee.Email; target.HireDate = employee.HireDate; target.Position = employee.Position; target.DepartmentId = employee.DepartmentId; target.UpdatedBy = GetLoginUserName(); target.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges(); return RedirectToAction("Index");
        }

        public IActionResult Delete(int id) { if (!IsLogin()) return RedirectToAction("Login", "Account"); if (!IsAdmin()) return Forbid(); var employee = _context.Employees.Include(e => e.Department).FirstOrDefault(e => e.EmployeeId == id); if (employee == null) return NotFound(); return View(employee); }

        [HttpPost]
        public IActionResult DeleteConfirmed(int employeeId)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (!IsAdmin()) return Forbid();
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
            if (employee == null) return NotFound();
            employee.IsDeleted = true; employee.UpdatedBy = GetLoginUserName(); employee.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges(); return RedirectToAction("Index");
        }
    }
}
