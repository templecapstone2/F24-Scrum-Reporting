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


        public IActionResult Scrums()
        {
            return View();
        }

        public IActionResult ScrumView()
        {


            // Default Scrum data using your existing Scrum model
            var scrums = new List<Scrum>
            {
                new Scrum(1, "Scrum #1", new DateTime(2024, 9, 7), true),
                new Scrum(2, "Scrum #2", new DateTime(2024, 9, 14), true),
                new Scrum(3, "Scrum #3", new DateTime(2024, 9, 21), false)
            };

            return View(scrums);
        }


        public IActionResult UserManagement()
        {
            return View();
        }

        public IActionResult TeamManagement()
        {
            return View();
        }

        public IActionResult ScrumManagement()
        {
            return View();
        }

        public IActionResult ViewReports()
        {
            return View();
        }

        public IActionResult StudentHome()
        {
            return View();
        }
        public IActionResult ProfessorHome()
        {
            return View();
        }
    }
}
