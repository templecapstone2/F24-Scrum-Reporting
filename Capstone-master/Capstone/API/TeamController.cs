using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Capstone.Models;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Capstone.API
{
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        private readonly DBConnect dbConnect;
        SqlCommand? sqlCommand;

        public TeamController(DBConnect dbConnect)
        {
            this.dbConnect = dbConnect;
        }

        [HttpPost]
        public IActionResult AddTeam([FromBody] Team team)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "AddTeam";
                sqlCommand.Parameters.AddWithValue("@name", team.Name);
                //idk if I want to pass the object in body or just a name string as url param
                if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) == 1)
                {
                    return Ok("Team " + team.Name + " added successfully");
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

        [HttpGet]
        public IActionResult GetTeams()
        {
            try
            {
                List<Team> teams = new List<Team>();
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "GetTeams";
                DataSet ds = dbConnect.GetDataSetUsingCmdObj(sqlCommand);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Team team = new Team();
                        foreach (var property in team.GetType().GetProperties())
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
                                    property.SetValue(team, value);
                                }
                                else
                                {
                                    property.SetValue(team, null);
                                }
                            }
                        }
                        teams.Add(team);
                    }
                    return Ok(teams);
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

        [HttpDelete("{id:int}")]
        public IActionResult DeleteTeam(int id)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "DeleteTeam";
                sqlCommand.Parameters.AddWithValue("@id", id);

                if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) == 1)
                {
                    return Ok("Team with ID " + id + " deleted successfully");
                }
                else
                {
                    return NotFound("No team found with ID " + id);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
