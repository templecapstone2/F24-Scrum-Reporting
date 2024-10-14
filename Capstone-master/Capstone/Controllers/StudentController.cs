using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Capstone.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Xml.Linq;

namespace Capstone.Controllers
{
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly UserService userService;
        private readonly TeamService teamService;
        private readonly TeamUserService teamUserService;
        private readonly ScrumService scrumService;
        private readonly ResponseService responseService;

        public StudentController(ResponseService responseService, ScrumService scrumService, TeamUserService teamUserService, TeamService teamService, UserService userService, ILogger<StudentController> logger)
        {
            this.responseService = responseService;
            this.scrumService = scrumService;
            this.teamUserService = teamUserService;
            this.teamService = teamService;
            this.userService = userService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            var tuId = HttpContext.Session.GetString("TUID");
            var usertype = HttpContext.Session.GetString("usertype");
            var fullName = HttpContext.Session.GetString("fullname");

            ViewBag.Full_Name = fullName;
            ViewBag.TU_ID = tuId;
            ViewBag.User_Type = usertype;

            return View();
        }

        public async Task<IActionResult> Response(string name)
        {
            var tuid = HttpContext.Session.GetString("TUID");
            var students = await userService.GetStudents();
            var teams = await teamService.GetTeams();
            var teamUsers = await teamUserService.GetTeamUsers();

            // Get the current user's ID from the session

            var userEntry = students.FirstOrDefault(tu => tu.TUID == tuid);
            var currentUserId = userEntry.ID;

            var teamUserEntry = teamUsers.FirstOrDefault(tu => tu.UserID == currentUserId);
            var currentTeamID = teamUserEntry.TeamID;

            // Get the team name based on the current team ID
            var currentTeamName = currentTeamID != null ? teams.FirstOrDefault(t => t.ID == currentTeamID)?.Name : null;

            var fullname = HttpContext.Session.GetString("fullname");
            var scrum_name = HttpContext.Session.GetString("ScrumName");

            ViewBag.CurrentTeamName = currentTeamName;
            ViewBag.Name = fullname;
            ViewBag.ScrumName = name;

            var scrums = await scrumService.GetScrums();
            var currentScrum = scrums.FirstOrDefault(s => s.Name == name);
            var scrumID = currentScrum.ID;

            ViewBag.ScrumID = scrumID;

            var responses = await responseService.GetResponses();
            var userResponse = responses.FirstOrDefault(r => r.UserID == currentUserId);

            if (userResponse == null)
            {
                userResponse = new Response
                {
                    QuestionOne = string.Empty,
                    QuestionTwo = string.Empty,
                    QuestionThree = string.Empty,
                    DateSubmitted = DateTime.Now,
                    ScrumID = scrumID,
                    UserID = currentUserId
                };

                await responseService.AddResponse(userResponse);
            }

            ViewBag.Responses = responses;

            return View(userResponse);
        }

        [HttpPost]
        public async Task<IActionResult> ModifyResponses(int id, Response response, string ScrumName)
        {
            response.DateSubmitted = DateTime.Now;
            await responseService.ModifyResponse(id, response);
            return RedirectToAction("Scrums", new { name = ScrumName });
        }
        public async Task<IActionResult> Scrums(string name)
        {
            var tuid = HttpContext.Session.GetString("TUID");
            var fullname = HttpContext.Session.GetString("fullname");
            HttpContext.Session.SetString("TUID", tuid);
            HttpContext.Session.SetString("fullname", fullname);

            var scrums = await scrumService.GetScrums();
            var publishedScrums = scrums.Where(scrum => scrum.IsActive).ToList();
            return View(publishedScrums);
        }
    }
}
