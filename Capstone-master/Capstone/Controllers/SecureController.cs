using Microsoft.AspNetCore.Mvc;
using ServiceReference1;

namespace Capstone.Controllers
{
    public class SecureController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var attributes = GetShibbolethHeaderAttributes();
            ViewData["tuid"] = attributes.ID;  
            ViewData["firstName"] = attributes.Name;
            ViewData["test"] = attributes.Test;

            //// Call the web service to get LDAP entry by employee number (TUID)
            //TempleLDAPEntry templeInformation = await ServiceReference1.getLDAPEntryByTUIDAsync(ID);

            //// Check if the retrieved information is valid
            //if (templeInformation != null)
            //{
            //    // Log and store relevant user information in the session
            //    Console.WriteLine($"Retrieved Temple Information: {templeInformation.displayNameField}"); // Log the display name

            //    HttpContext.Session.SetString("TU_ID", templeInformation.templeEduTUID ?? "N/A");
            //    HttpContext.Session.SetString("Email", templeInformation.mailField ?? "N/A");
            //    HttpContext.Session.SetString("Full_Name", templeInformation.displayNameField ?? "N/A");
            //    HttpContext.Session.SetString("Affiliation_Primary", templeInformation.eduPersonPrimaryAffiliationField ?? "N/A");

                // Log the employee number for debugging
                Console.WriteLine($"Employee Number: {attributes.ID}");

            if (string.IsNullOrEmpty(attributes.ID))
            {
                Console.WriteLine("No employee number found. Redirecting to error page.");
                return RedirectToAction("Error", "Home");
            }

            //// Fetch user information directly (simulate this for now)
            //var templeInformation = await GetUserInfoByEmployeeNumberAsync(employeeNumber);
            //Console.WriteLine($"Temple Information: {templeInformation}"); // Log the retrieved information

            //if (templeInformation != null)
            //{
            //    // Store user information in the session
            //    HttpContext.Session.SetInt32("TU_ID", templeInformation.ID);
            //    HttpContext.Session.SetString("Email", templeInformation.Email);
            //    HttpContext.Session.SetString("Full_Name", templeInformation.Name);
            //    HttpContext.Session.SetString("Affiliation_Primary", templeInformation.UserType);

            //    // Redirect based on user type
            //    if (templeInformation.UserType == "Professor")
            //    {
            //        Console.WriteLine("Redirecting to ProfessorHome.");
            //        return RedirectToAction("ProfessorHome", "Home");
            //    }
            //    else if (templeInformation.UserType == "Student")
            //    {
            //        Console.WriteLine("Redirecting to StudentHome.");
            //        return RedirectToAction("StudentHome", "Home");
            //    }
            //}

            // Fallback if user information is not available
            Console.WriteLine("User information not available. Redirecting to ProfessorHome.");
            return View();
        }

        protected (string ID, string Name, string Test) GetShibbolethHeaderAttributes()
        {
            string ID = Request.Headers.ContainsKey("employeeNumber") ? Request.Headers["employeeNumber"].ToString() : null;
            string Name = Request.Headers.ContainsKey("eppn") ? Request.Headers["eppn"].ToString() : null;
            string Test = Request.Headers.ContainsKey("eduPersonPrimaryAffiliation") ? Request.Headers["eduPersonPrimaryAffiliation"].ToString() : null;

            // Set session variables for the retrieved attributes
            HttpContext.Session.SetString("SSO_Attribute_employeeNumber", ID ?? "N/A");
            HttpContext.Session.SetString("SSO_Attribute_Name", Name ?? "N/A");
            HttpContext.Session.SetString("SSO_Attribute_Name", Name ?? "N/A");

            return (
                string.IsNullOrWhiteSpace(ID) ? "N/A" : ID,
                string.IsNullOrWhiteSpace(Name) ? "N/A" : Name,
                string.IsNullOrWhiteSpace(Test) ? "N/A" : Test
            );
        }
    }
}
