using EmployeeManagementWeb.Data;
using EmployeeManagementWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementWeb.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        private bool IsLogin() => !string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"));
        private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";
        private string GetLoginUserName() => HttpContext.Session.GetString("UserName") ?? "";

        public IActionResult Index()
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");

            var departments = _context.Departments.ToList();
            ViewBag.IsAdmin = IsAdmin();
            return View(departments);
        }

        public IActionResult Create()
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (!IsAdmin()) return Forbid();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (!IsAdmin()) return Forbid();
            if (!ModelState.IsValid) return View(department);

            department.CreatedBy = GetLoginUserName();
            department.UpdatedBy = GetLoginUserName();
            _context.Departments.Add(department);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (!IsAdmin()) return Forbid();
            var department = _context.Departments.Find(id);
            if (department == null) return NotFound();
            return View(department);
        }

        [HttpPost]
        public IActionResult Edit(Department department)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (!IsAdmin()) return Forbid();
            if (!ModelState.IsValid) return View(department);

            var target = _context.Departments.Find(department.DepartmentId);
            if (target == null) return NotFound();

            target.DepartmentName = department.DepartmentName;
            target.UpdatedBy = GetLoginUserName();
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (!IsAdmin()) return Forbid();
            var department = _context.Departments.Find(id);
            if (department == null) return NotFound();

            var hasEmployees = _context.Employees.Any(e => e.DepartmentId == id);
            ViewBag.HasEmployees = hasEmployees;
            return View(department);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int departmentId)
        {
            if (!IsLogin()) return RedirectToAction("Login", "Account");
            if (!IsAdmin()) return Forbid();
            var department = _context.Departments.Find(departmentId);
            if (department == null) return NotFound();

            var hasEmployees = _context.Employees.Any(e => e.DepartmentId == departmentId);
            if (hasEmployees)
            {
                TempData["ErrorMessage"] = "この部門に所属している社員がいるため削除できません。";
                return RedirectToAction("Index");
            }

            _context.Departments.Remove(department);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
