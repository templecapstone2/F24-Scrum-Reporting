using Microsoft.AspNetCore.Mvc;

namespace DataAPI.Controllers
{
    public class ScrumController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
