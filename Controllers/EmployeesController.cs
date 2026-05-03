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

        private bool IsLogin()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("UserName"));
        }

        private string GetLoginUserName()
        {
            return HttpContext.Session.GetString("UserName") ?? "";
        }

        public IActionResult Index()
        {
            if (!IsLogin())
            {
                return RedirectToAction("Login", "Account");
            }

            var employees = _context.Employees
                .Include(e => e.Department)
                .ToList();

            return View(employees);
        }

        public IActionResult Create()
        {
            if (!IsLogin())
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Departments = new SelectList(
                _context.Departments.ToList(),
                "DepartmentId",
                "DepartmentName"
            );

            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (!IsLogin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(
                    _context.Departments.ToList(),
                    "DepartmentId",
                    "DepartmentName",
                    employee.DepartmentId
                );

                return View(employee);
            }

            employee.CreatedBy = GetLoginUserName();
            employee.UpdatedBy = GetLoginUserName();

            _context.Employees.Add(employee);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (!IsLogin())
            {
                return RedirectToAction("Login", "Account");
            }

            var employee = _context.Employees.Find(id);

            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.Departments = new SelectList(
                _context.Departments.ToList(),
                "DepartmentId",
                "DepartmentName",
                employee.DepartmentId
            );

            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (!IsLogin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(
                    _context.Departments.ToList(),
                    "DepartmentId",
                    "DepartmentName",
                    employee.DepartmentId
                );

                return View(employee);
            }

            var target = _context.Employees.Find(employee.EmployeeId);

            if (target == null)
            {
                return NotFound();
            }

            target.EmployeeName = employee.EmployeeName;
            target.DepartmentId = employee.DepartmentId;
            target.UpdatedBy = GetLoginUserName();

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (!IsLogin())
            {
                return RedirectToAction("Login", "Account");
            }

            var employee = _context.Employees
                .Include(e => e.Department)
                .FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int employeeId)
        {
            if (!IsLogin())
            {
                return RedirectToAction("Login", "Account");
            }

            var employee = _context.Employees.Find(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}