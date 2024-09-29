using Microsoft.AspNetCore.Mvc;

namespace DataAPI.Controllers
{
    public class ResponseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
