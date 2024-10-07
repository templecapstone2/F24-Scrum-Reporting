using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
    public class SecureController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Submit()
        {
            bool loginSuccessful = true;

            if (loginSuccessful)
            {
                return RedirectToAction("ProfessorHome", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Login failed. Please try again.";
                return View("~/Secure/Login");
            }
        }
    }
}
