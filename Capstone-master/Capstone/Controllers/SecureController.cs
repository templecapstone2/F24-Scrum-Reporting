using Capstone.Models;
using Capstone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceReference1;
using System.ServiceModel;
using System.Text.Json;

namespace Capstone.Controllers
{
    public class SecureController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly UserService userService;
        private readonly TeamUserService teamUserService;

        public SecureController(TeamUserService teamUserService, UserService userService, IConfiguration configuration)
        {
            this.teamUserService = teamUserService;
            this.userService = userService;
            this.configuration = configuration;
        }

        //--------------------------------------------------------------------
        //--------------------------------------------------------------------
        //              *** SHIBBOLETH TEST AND STUDENT TEST***
        //--------------------------------------------------------------------
        //--------------------------------------------------------------------

        public async Task<IActionResult> Index()
        {
            // For Publish Testing
            //var id = GetShibbolethHeaderAttributes();

            // For Professor Testing
            var id = "916524704";

            // For Student Testing
            //var id = "915905753";

            ViewData["tuid"] = id;
            HttpContext.Session.SetString("TUID", id);


            // Create the request object for the search
            var searchRequestBody = new SearchRequestBody
            {
                username = configuration["LDAPSettings:Username"],
                password = configuration["LDAPSettings:Password"],
                attribute = configuration["LDAPSettings:Attribute"],
                value = id
            };

            TempleLDAPEntry templeInformation = null;

            try
            {
                var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                var endpoint = new EndpointAddress("https://preprod-wsw.temple.edu/ws_ldapsearch/ldap_search.asmx?wsdl");

                using (var client = new LDAP_SearchSoapClient(binding, endpoint))
                {
                    var response = await client.SearchAsync(
                        searchRequestBody.username,
                        searchRequestBody.password,
                        searchRequestBody.attribute,
                        searchRequestBody.value);

                    if (response != null && response.Body != null && response.Body.SearchResult != null && response.Body.SearchResult.Length > 0)
                    {
                        templeInformation = response.Body.SearchResult[0];
                        HttpContext.Session.SetString("fullname", templeInformation.givenName + " " + templeInformation.sn);
                        HttpContext.Session.SetString("usertype", templeInformation.eduPersonAffiliation);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching data {ex.Message}");
            }

            if (templeInformation != null)
            {
                List<User> users = await userService.GetUsers();
                bool userExists = false;
                foreach (User user in users)
                {
                    if (user.TUID == id)
                    {
                        var userJson = JsonSerializer.Serialize(user);
                        HttpContext.Session.SetString("currentUser", userJson);
                        userExists = true;
                    }
                }

                if (!userExists)
                {
                    var newUser = new User
                    {
                        TUID = id,
                        FirstName = templeInformation.givenName,
                        LastName = templeInformation.sn,
                        UserType = GetUserType(templeInformation.eduPersonAffiliation)
                    };

                    var addedUser = await userService.AddUser(newUser);
                }

                if (templeInformation.eduPersonAffiliation.Contains("instructor"))
                {
                    Console.WriteLine("Redirecting to ProfessorHome.");
                    return RedirectToAction("Dashboard", "Professor");
                }
                else if (templeInformation.eduPersonAffiliation.Contains("student"))
                {
                    Console.WriteLine("Redirecting to StudentHome.");
                    return RedirectToAction("Dashboard", "Student");
                }
            }
            return View();
        }

        private string GetUserType(string affiliation)
        {
            if (affiliation.Contains("student", StringComparison.OrdinalIgnoreCase))
            {
                return "student";
            }
            else if (affiliation.Contains("instructor", StringComparison.OrdinalIgnoreCase))
            {
                return "instructor";
            }
            return "other";
        }

        protected string GetShibbolethHeaderAttributes()
        {
            string ID = Request.Headers.ContainsKey("employeeNumber") ? Request.Headers["employeeNumber"].ToString() : null;
            HttpContext.Session.SetString("SSO_Attribute_employeeNumber", ID ?? "N/A");
            return (string.IsNullOrWhiteSpace(ID) ? "N/A" : ID);
        }
    }
}
