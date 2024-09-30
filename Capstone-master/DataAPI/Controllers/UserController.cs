using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using DataAPI.Utilities;
using DataAPI.Models;
using System.Diagnostics;

namespace DataAPI.Controllers
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


        [HttpGet("users")]
        public IActionResult GetUsers()
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
                        if (row.Table.Columns.Contains(property.Name))
                        {
                            object value = row[property.Name];

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

        [HttpGet("students")]
        public IActionResult GetStudents()
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
                        if (row.Table.Columns.Contains(property.Name))
                        {
                            object value = row[property.Name];

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
    }
}
