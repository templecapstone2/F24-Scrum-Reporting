using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Capstone.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Xml.Linq;
using Capstone.ViewModels;

namespace Capstone.Controllers
{
    [Route("Secure/[controller]")]
    public class ProfessorController : Controller
    {
        private readonly ILogger<ProfessorController> _logger;
        private readonly UserService userService;
        private readonly TeamService teamService;
        private readonly TeamUserService teamUserService;
        private readonly ScrumService scrumService;
        private readonly ResponseService responseService;

        public ProfessorController(ResponseService responseService, ScrumService scrumService, TeamUserService teamUserService, TeamService teamService, UserService userService, ILogger<ProfessorController> logger)
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

        [HttpGet("ScrumManagement")]
        public async Task<IActionResult> ScrumManagement()
        {
            var scrums = await scrumService.GetScrums();
            return View("~/Views/Secure/Professor/ScrumManagement.cshtml", scrums);
        }

        [HttpPost("ScrumManagement")]
        public async Task<IActionResult> ScrumManagement(string action, DateTime? DateDue, int scrumID)
        {
            if (action == "AddScrum")
            {
                var newScrum = new Scrum
                {
                    Name = $"Scrum #{GetNextScrumNumber()}",
                    DateDue = DateDue ?? DateTime.Now,
                    IsActive = false
                };

                await scrumService.AddScrum(newScrum);
                return RedirectToAction("ScrumManagement");
            }
            else if (action == "Publish" || action == "Unpublish")
            {
                var scrums = await scrumService.GetScrums();
                var scrum = scrums.FirstOrDefault(s => s.ID == scrumID);

                if (scrum != null)
                {
                    scrum.IsActive = !scrum.IsActive;
                    scrum.DateDue = DateDue.HasValue ? DateDue.Value : scrum.DateDue;

                    await scrumService.ModifyScrum(scrumID, scrum);
                }
                return RedirectToAction("ScrumManagement");
            }
            return RedirectToAction("ScrumManagement");
        }

        [HttpPost]
        public IActionResult DeleteScrum(int scrumID)
        {
            var success = scrumService.DeleteScrum(scrumID);
            return RedirectToAction("ScrumManagement");
        }

        private int GetNextScrumNumber()
        {
            var scrums = scrumService.GetScrums().Result;
            return scrums.Count > 0 ? scrums.Max(s => int.Parse(s.Name.Split('#')[1])) + 1 : 1;
        }

        [HttpGet("StudentManagement")]
        public async Task<IActionResult> StudentManagement(int? userID, int? newTeamID)
        {
            var students = await userService.GetStudents();
            var teams = await teamService.GetTeams();
            var teamUsers = await teamUserService.GetTeamUsers();

            // Prepare SelectList for each student
            foreach (var student in students)
            {
                var currentTeamID = teamUsers.FirstOrDefault(tu => tu.UserID == student.ID)?.TeamID;
                student.SelectList = new SelectList(teams, "ID", "Name", currentTeamID);
            }

            ViewBag.Teams = teams;
            ViewBag.TeamUsers = teamUsers;
            return View("~/Views/Secure/Professor/StudentManagement.cshtml", students);
        }

        [HttpPost("StudentManagement")]
        public async Task<IActionResult> StudentManagementPost(int userID, int newTeamID)
        {
            var currentTeamID = (await teamUserService.GetTeamUsers()).FirstOrDefault(tu => tu.UserID == userID)?.TeamID;

            if (currentTeamID != newTeamID)
            {
                await teamUserService.ModifyTeamUser(newTeamID, userID);
            }

            // Redirect to the GET method after processing the POST
            return RedirectToAction("StudentManagement", new { userID });
        }

        [HttpGet("AggragateView")]
        public async Task<IActionResult> AggregateView()
        {
            List<Response> responses = await responseService.GetResponses();
            List<Scrum> scrums = await scrumService.GetScrums();
            List<Team> teams = await teamService.GetTeams();
            List<User> students = await userService.GetStudents();
            List<TeamUser> teamUsers = await teamUserService.GetTeamUsers();

            var model = new AggregateViewModel(responses, scrums, teams, students, teamUsers);
            return View("~/Views/Secure/Professor/AggregateView.cshtml", model);
        }

        [HttpGet("TeamManagement")]
        public async Task<IActionResult> TeamManagement()
        {
            var teams = await teamService.GetTeams();
            var teamsToDisplay = teams.Skip(1).ToList();
            return View("~/Views/Secure/Professor/TeamManagement.cshtml", teamsToDisplay);
        }


        [HttpPost("TeamManagement")]
        public async Task<IActionResult> TeamManagement(Team team)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await teamService.AddTeam(team);
                    return RedirectToAction("TeamManagement");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error adding team: " + ex.Message);
                }
            }
            var existingTeams = await teamService.GetTeams();
            return View("~/Views/Secure/Professor/TeamManagement.cshtml", existingTeams);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            try
            {
                await teamService.DeleteTeam(id);
                return RedirectToAction("TeamManagement");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error deleting team: " + ex.Message);
                return RedirectToAction("TeamManagement");
            }
        }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            var tuId = HttpContext.Session.GetString("TUID");
            var usertype = HttpContext.Session.GetString("usertype");
            var fullName = HttpContext.Session.GetString("fullname");

            ViewBag.Full_Name = fullName;
            ViewBag.TU_ID = tuId;
            ViewBag.User_Type = usertype;

            return View("~/Views/Secure/Professor/Dashboard.cshtml");
        }
    }
}
