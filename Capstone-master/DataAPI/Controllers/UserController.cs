using Microsoft.AspNetCore.Mvc;

namespace DataAPI.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
