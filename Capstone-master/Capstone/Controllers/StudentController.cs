using Capstone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Capstone.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Xml.Linq;
using Capstone.ViewModels;
using System.Text.Json;
using Capstone.API;

namespace Capstone.Controllers
{
    [Route("Secure/[controller]")]
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

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            var tuId = HttpContext.Session.GetString("TUID");
            var usertype = HttpContext.Session.GetString("usertype");
            var fullName = HttpContext.Session.GetString("fullname");

            ViewBag.Full_Name = fullName;
            ViewBag.TU_ID = tuId;
            ViewBag.User_Type = usertype;

            return View("~/Views/Secure/Student/Dashboard.cshtml");
        }

        [HttpGet("Scrums")]
        public async Task<IActionResult> Scrums()
        {
            var userJson = HttpContext.Session.GetString("currentUser");
            User loggedInUser = JsonSerializer.Deserialize<User>(userJson);

            var scrums = await scrumService.GetScrums();
            var publishedScrums = scrums.Where(scrum => scrum.IsActive).ToList();

            List<Response> responses = await responseService.GetResponses();
            List<Response> studentResponses = new List<Response>();
            if (loggedInUser != null)
            {
                foreach (Response response in responses)
                {
                    if (response.UserID == loggedInUser.ID)
                    {
                        studentResponses.Add(response);
                    }
                }
            }

            StudentScrumModel studentScrumModel = new StudentScrumModel(publishedScrums, studentResponses);
            return View("~/Views/Secure/Student/Scrums.cshtml", studentScrumModel);
        }

        [HttpGet("Response")]
        public async Task<IActionResult> Response(int scrumID, int? responseID)
        {
            var userJson = HttpContext.Session.GetString("currentUser");
            User loggedInUser = JsonSerializer.Deserialize<User>(userJson);
            //its OK to get all and iterate through them rather than writing GetObjectByID methods bc the dataset will always be small due to end-of-semester deletions
            var scrums = await scrumService.GetScrums();
            var teamUsers = await teamUserService.GetTeamUsers();
            var teams = await teamService.GetTeams();
            var responses = await responseService.GetResponses();

            Scrum scrum = scrums.FirstOrDefault(s => s.ID == scrumID);
            TeamUser teamUser = teamUsers.FirstOrDefault(tu => tu.UserID == loggedInUser.ID);
            Team team = teamUser != null ? teams.FirstOrDefault(t => t.ID == teamUser.TeamID) : null;
            Response response = responseID != null ? responses.FirstOrDefault(r => r.ID == responseID.Value) : new Response
            {
                ScrumID = scrumID,
                UserID = loggedInUser.ID,
                ID = 0
            };

            // Check for null values and handle accordingly
            if (scrum == null)
            {
                return NotFound("Scrum not found.");
            }
            if (responseID.HasValue && response == null)
            {
                return NotFound("Response not found.");
            }

            ResponseModel responseModel = new ResponseModel(scrum, loggedInUser, team, response);
            return View("~/Views/Secure/Student/Response.cshtml", responseModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddResponse(Response response, int responseID)
        {
            var userJson = HttpContext.Session.GetString("currentUser");
            User loggedInUser = JsonSerializer.Deserialize<User>(userJson);
            int id = loggedInUser.ID;
            response.DateSubmitted = DateTime.Now;

            if (responseID == 0)
            {
                await responseService.AddResponse(response);
            }
            else
            {
                await responseService.ModifyResponse(id, response);
            }

            return RedirectToAction("Scrums");
        }
    }
}
