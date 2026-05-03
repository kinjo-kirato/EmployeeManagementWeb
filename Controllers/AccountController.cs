using EmployeeManagementWeb.Data;
using EmployeeManagementWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == model.UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = "ユーザーIDまたはパスワードが間違っています。";
                return View(model);
            }

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ViewBag.ErrorMessage = "ユーザーIDまたはパスワードが間違っています。";
                return View(model);
            }

            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("UserId", user.UserId);
            HttpContext.Session.SetString("Role", user.Role);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
