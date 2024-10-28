using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Capstone.Models;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Capstone.API
{
    [Route("api/[controller]")]
    public class TeamUserController : Controller
    {
        private readonly DBConnect dbConnect;
        SqlCommand? sqlCommand;
        public TeamUserController(DBConnect dbConnect)
        {
            this.dbConnect = dbConnect;
        }

        [HttpPost("{teamID:int}/{userID:int}")]
        public IActionResult AddTeamUser(int teamID, int userID)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "AddTeamUser";
                sqlCommand.Parameters.AddWithValue("@team_id", teamID);
                sqlCommand.Parameters.AddWithValue("@user_id", userID);

                if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) == 1)
                {
                    return Ok("Team_User association added for team ID " + teamID + " and user ID " + userID);
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
        public IActionResult GetTeamUsers()
        {
            try
            {
                List<TeamUser> teamUsers = new List<TeamUser>();
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "GetTeamUsers";
                DataSet ds = dbConnect.GetDataSetUsingCmdObj(sqlCommand);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        TeamUser teamUser = new TeamUser();
                        foreach (var property in teamUser.GetType().GetProperties())
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
                                    property.SetValue(teamUser, value);
                                }
                                else
                                {
                                    property.SetValue(teamUser, null);
                                }
                            }
                        }
                        teamUsers.Add(teamUser);
                    }
                    return Ok(teamUsers);
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

        [HttpPut("{teamID:int}/{userID:int}")]
        public IActionResult ModifyTeamUser(int teamID, int userID)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "ModifyTeamUser";
                sqlCommand.Parameters.AddWithValue("@team_id", teamID);
                sqlCommand.Parameters.AddWithValue("@user_id", userID);

                if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) == 1)
                {
                    return Ok("Team_User modified successfully");
                }
                else
                {
                    return NotFound("No Team_User association found for team ID " + teamID + " and user ID " + userID);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{userID:int}")]
        public IActionResult DeleteTeamUser(int userID)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "DeleteTeamUser";
                sqlCommand.Parameters.AddWithValue("@user_id", userID);

                if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) == 1)
                {
                    return Ok(userID);
                }
                else
                {
                    return NotFound(userID);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteTeamUsers()
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "DeleteTeamUsers";

                if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) > 0)
                {
                    return Ok("All Team_User associations deleted successfully");
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
