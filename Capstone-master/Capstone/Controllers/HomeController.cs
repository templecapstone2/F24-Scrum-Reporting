using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Capstone.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Capstone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService userService;
        private readonly TeamService teamService;
        private readonly TeamUserService teamUserService;

        public HomeController(TeamUserService teamUserService, TeamService teamService, UserService userService, ILogger<HomeController> logger)
        {
            this.teamUserService = teamUserService;
            this.teamService = teamService;
            this.userService = userService;
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


        public async Task<IActionResult> UserManagement(int? userID, int? newTeamID)
        {
            var students = await userService.GetStudents();
            var teams = await teamService.GetTeams();
            var teamUsers = await teamUserService.GetTeamUsers();

            if (userID.HasValue && newTeamID.HasValue)
            {
                var currentTeamID = teamUsers.FirstOrDefault(tu => tu.UserID == userID.Value)?.TeamID;

                if (currentTeamID != newTeamID.Value) 
                {
                    await teamUserService.ModifyTeamUser(newTeamID.Value, userID.Value);
                }

            }

            foreach (var student in students)
            {
                var currentTeamID = teamUsers.FirstOrDefault(tu => tu.UserID == student.ID)?.TeamID;
                student.SelectList = new SelectList(teams, "ID", "Name", currentTeamID);

            }

            ViewBag.Teams = teams;
            ViewBag.TeamUsers = teamUsers;
            return View(students); 
        }
        public async Task<IActionResult> TeamManagement()
        {
            var teams = await teamService.GetTeams();
            return View(teams);
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
            var tuId = HttpContext.Session.GetString("TUID");
            var usertype = HttpContext.Session.GetString("usertype");
            var fullName = HttpContext.Session.GetString("fullname");

            // Debugging output
            Console.WriteLine($"TUID: {tuId}, UserType: {usertype}, FullName: {fullName}");
            _logger.LogInformation($"Retrieved from session: TUID={tuId}, UserType={usertype}, FullName={fullName}");

            ViewBag.Full_Name = fullName;
            ViewBag.TU_ID = tuId;
            ViewBag.User_Type = usertype;

            return View();
        }
        public IActionResult ProfessorHome()
        {
            var tuId = HttpContext.Session.GetString("TUID");
            var usertype = HttpContext.Session.GetString("usertype");
            var fullName = HttpContext.Session.GetString("fullname");
            ViewBag.Full_Name = fullName;

            ViewBag.TU_ID = tuId;
            ViewBag.User_Type = usertype;
            ViewBag.Full_Name = fullName;

            return View();
        }



        public IActionResult ScrumReport()
        {
            var model = new ScrumReport
            {
                ScrumItems = new List<ScrumItem>
                {
                    new ScrumItem { Name = "Scrum #1", DueDate = DateTime.Parse("2024-09-07"), IsSubmitted = true },
                    new ScrumItem { Name = "Scrum #2", DueDate = DateTime.Parse("2024-09-14"), IsSubmitted = false },
                    new ScrumItem { Name = "Scrum #3", DueDate = DateTime.Parse("2024-09-14"), IsSubmitted = false }
                }
            };
            return View(model);
        }
     
    }
}
