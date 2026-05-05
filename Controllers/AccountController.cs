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

        public IActionResult Profile()
        {
            var user = GetLoginUser();
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new ProfileViewModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Role = user.Role
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult ChangePassword(ProfileViewModel model)
        {
            var user = GetLoginUser();
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            model.UserId = user.UserId;
            model.UserName = user.UserName;
            model.Role = user.Role;

            if (!ModelState.IsValid)
            {
                return View("Profile", model);
            }

            var hasher = new PasswordHasher<User>();
            var verifyResult = hasher.VerifyHashedPassword(user, user.PasswordHash, model.CurrentPassword);
            if (verifyResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(nameof(model.CurrentPassword), "現在のパスワードが正しくありません。");
                return View("Profile", model);
            }

            user.PasswordHash = hasher.HashPassword(user, model.NewPassword);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "パスワードを変更しました。";
            return RedirectToAction("Profile");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        private User? GetLoginUser()
        {
            var loginUserId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrWhiteSpace(loginUserId))
            {
                return null;
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == loginUserId);
            if (user == null)
            {
                HttpContext.Session.Clear();
            }

            return user;
        }
    }
}
