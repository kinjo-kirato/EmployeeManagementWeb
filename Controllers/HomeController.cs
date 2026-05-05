using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserName = userName;
            return View();
        }
    }
}   