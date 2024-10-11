using Capstone.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceReference1;
using System.ServiceModel;

namespace Capstone.Controllers
{
    public class SecureController : Controller
    {
        private readonly IConfiguration configuration;

        public SecureController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            // For Publish Testing
            //var id = GetShibbolethHeaderAttributes();

            // For Local Testing
            var id = "915905753";

            ViewData["tuid"] = id;

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

                    // Access the SearchResult from the response
                    if (response != null && response.Body != null && response.Body.SearchResult != null && response.Body.SearchResult.Length > 0)
                    {
                        templeInformation = response.Body.SearchResult[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching data {ex.Message}");
            }

            if (templeInformation != null)
            {
                // Store user information in the session
                HttpContext.Session.SetString("TUID", templeInformation.templeEduTUID); 
                HttpContext.Session.SetString("Email", templeInformation.mail); 
                HttpContext.Session.SetString("Name", templeInformation.cn); 
                HttpContext.Session.SetString("Role", templeInformation.eduPersonPrimaryAffiliation); 
                ViewData["firstname"] = templeInformation.cn;
                ViewData["lastname"] = templeInformation.sn;
                ViewData["usertype"] = templeInformation.eduPersonPrimaryAffiliation;

                // Redirect based on user type
                if (templeInformation.eduPersonPrimaryAffiliation == "professor") 
                {
                    Console.WriteLine("Redirecting to ProfessorHome.");
                    return RedirectToAction("ProfessorHome", "Home");
                }
                else if (templeInformation.eduPersonPrimaryAffiliation == "student") 
                {
                    Console.WriteLine("Redirecting to StudentHome.");
                    return RedirectToAction("StudentHome", "Home");
                }
            }
            return View();
        }

        protected string GetShibbolethHeaderAttributes()
        {
            string ID = Request.Headers.ContainsKey("employeeNumber") ? Request.Headers["employeeNumber"].ToString() : null;
            HttpContext.Session.SetString("SSO_Attribute_employeeNumber", ID ?? "N/A");
            return (string.IsNullOrWhiteSpace(ID) ? "N/A" : ID);
        }
    }
}
