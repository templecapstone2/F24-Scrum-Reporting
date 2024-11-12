using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Capstone.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Xml.Linq;
using Capstone.ViewModels;
using System.Collections.Generic;

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

        [HttpPost("DeleteScrum")]
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
        public async Task<IActionResult> StudentManagement()
        {
            List<User> students = await userService.GetStudents();
            List<Team> teams = await teamService.GetTeams();
            List<TeamUser> teamUsers = await teamUserService.GetTeamUsers();

            var model = new StudentManagementModel(students, teams, teamUsers);
            return View("~/Views/Secure/Professor/StudentManagement.cshtml", model);
        }

        [HttpPost("StudentManagement")]
        public async Task<IActionResult> StudentManagementPost(int userID, int? newTeamID)
        {
            // Get the current team association for the user
            var currentTeamUser = (await teamUserService.GetTeamUsers()).FirstOrDefault(tu => tu.UserID == userID);

            if (newTeamID == null) // Check if set to "Unassigned"
            {
                if (currentTeamUser != null)
                {
                    // Delete if set to unassigned
                    await teamUserService.DeleteTeamUser(userID);
                }
            }
            else if (currentTeamUser != null && currentTeamUser.TeamID != newTeamID)
            {
                // Modify TeamUser based on dropdown newTeamID
                await teamUserService.ModifyTeamUser(newTeamID.Value, userID);
            }
            else if (currentTeamUser == null)
            {
                // Create a new association if user does not have one
                await teamUserService.AddTeamUser(newTeamID.Value, userID);
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
            return View("~/Views/Secure/Professor/TeamManagement.cshtml", teams);
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

        [HttpPost("DeleteTeam")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            bool isDeleted = await teamService.DeleteTeam(id); // Capture the result

            if (isDeleted)
            {
                TempData["SuccessMessage"] = "Team deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "You cannot delete a team that has users assigned to it!";
            }
            return RedirectToAction("TeamManagement");
        }

        [HttpPost("NukeDatabase")]
        public async Task<IActionResult> NukeDatabase()
        {
            try
            {
                await teamUserService.DeleteTeamUsers();
                await teamService.DeleteTeams();
                await responseService.DeleteResponses();
                await scrumService.DeleteScrums();
                await userService.DeleteStudents();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while starting a new semester. Please try again.";

                return RedirectToAction("ScrumManagement");

            }
            TempData["SuccessMessage"] = "The database was successfully cleared for the new semester.";
            return RedirectToAction("Dashboard");
        }


        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            var fullName = HttpContext.Session.GetString("fullname");
            ViewBag.Full_Name = fullName;

            return View("~/Views/Secure/Professor/Dashboard.cshtml");
        }
    }
}
