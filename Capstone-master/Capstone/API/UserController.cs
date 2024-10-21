using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Capstone.Models;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Capstone.API
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly DBConnect dbConnect;
        SqlCommand? sqlCommand;

        public UserController(DBConnect dbConnect)
        {
            this.dbConnect = dbConnect;
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "AddUser";
                sqlCommand.Parameters.AddWithValue("@tuid", user.TUID);
                sqlCommand.Parameters.AddWithValue("@first_name", user.FirstName);
                sqlCommand.Parameters.AddWithValue("@last_name", user.LastName);
                sqlCommand.Parameters.AddWithValue("@user_type", user.UserType);

                SqlParameter outputIdParam = new SqlParameter("@newUserID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                sqlCommand.Parameters.Add(outputIdParam);

                if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) == 1)
                {
                    int newUserId = (int)outputIdParam.Value; 
                    user.ID = newUserId; 

                    return Ok(user); 
                }
                else
                {
                    return BadRequest("Error");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            try
            {
                List<User> users = new List<User>();
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "GetUsers";
                DataSet ds = dbConnect.GetDataSetUsingCmdObj(sqlCommand);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        User user = new User();
                        foreach (var property in user.GetType().GetProperties())
                        {
                            // Get the JsonPropertyName for each property of the model
                            var jsonPropertyName = property.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false)
                                               .FirstOrDefault() as JsonPropertyNameAttribute;

                            // Use json attribute name if present, otherwise use property name
                            string columnName = jsonPropertyName != null ? jsonPropertyName.Name : property.Name;

                            if (row.Table.Columns.Contains(columnName))
                            {
                                object value = row[columnName];

                                if (value != DBNull.Value)
                                {
                                    property.SetValue(user, value);
                                }
                                else
                                {
                                    property.SetValue(user, null);
                                }
                            }
                        }
                        users.Add(user);
                    }
                    return Ok(users);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("students")]
        public IActionResult GetStudents()
        {
            try
            {
                List<User> students = new List<User>();
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "GetStudents";
                DataSet ds = dbConnect.GetDataSetUsingCmdObj(sqlCommand);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        User student = new User();
                        foreach (var property in student.GetType().GetProperties())
                        {
                            // Get the JsonPropertyName for each property of the model
                            var jsonPropertyName = property.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false)
                       .FirstOrDefault() as JsonPropertyNameAttribute;

                            // Use json attribute name if present, otherwise use property name
                            string columnName = jsonPropertyName != null ? jsonPropertyName.Name : property.Name;

                            if (row.Table.Columns.Contains(columnName))
                            {
                                object value = row[columnName];

                                if (value != DBNull.Value)
                                {
                                    property.SetValue(student, value);
                                }
                                else
                                {
                                    property.SetValue(student, null);
                                }
                            }
                        }
                        students.Add(student);
                    }
                    return Ok(students);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteStudents()
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "DeleteStudents";

                if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) > 0)
                {
                    return Ok("Students deleted successfully");
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
