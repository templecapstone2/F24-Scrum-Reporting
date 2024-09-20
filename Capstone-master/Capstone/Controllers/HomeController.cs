using Capstone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Capstone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Aggregate()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }

        public IActionResult Report()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult StudentHome()
        {
            return View();
        }
    }
}
