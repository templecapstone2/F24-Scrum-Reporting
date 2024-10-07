using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
