using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string UserType)
        {
            if (UserType == "Professor")
            {
                return RedirectToAction("ProfessorHome", "Home");
            }
            else if (UserType == "Student")
            {
                return RedirectToAction("StudentHome", "Home");
            }

            return View();
        }
    }
}
