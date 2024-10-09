using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
    public class LoginController : Controller
    {

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
