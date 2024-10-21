using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Capstone.Models;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace DataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScrumController : Controller
    {
        private readonly DBConnect dbConnect;
        SqlCommand? sqlCommand;

        public ScrumController(DBConnect dbConnect)
        {
            this.dbConnect = dbConnect;
        }

        [HttpPost]
        public IActionResult AddScrum([FromBody] Scrum scrum)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "AddScrum";
                sqlCommand.Parameters.AddWithValue("@name", scrum.Name);
                sqlCommand.Parameters.AddWithValue("@date_due", scrum.DateDue);
                sqlCommand.Parameters.AddWithValue("@is_active", scrum.IsActive);

                if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) == 1)
                {
                    return Ok("Scrum added successfully");
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
        public IActionResult GetScrums()
        {
            try
            {
                List<Scrum> scrums = new List<Scrum>();
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "GetScrums";
                DataSet ds = dbConnect.GetDataSetUsingCmdObj(sqlCommand);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Scrum scrum = new Scrum();
                        foreach (var property in scrum.GetType().GetProperties())
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
                                    property.SetValue(scrum, value);
                                }
                                else
                                {
                                    property.SetValue(scrum, null);
                                }
                            }
                        }
                        scrums.Add(scrum);
                    }
                    return Ok(scrums);
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

        [HttpPut("{id:int}")]
        public IActionResult ModifyScrum(int id, [FromBody] Scrum scrum)
        {
            try
            {
                if (id != scrum.ID)
                {
                    return BadRequest("ID in the URL does not match ID in the request body.");
                }

                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "ModifyScrum";
                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlCommand.Parameters.AddWithValue("@name", scrum.Name);
                sqlCommand.Parameters.AddWithValue("@date_due", scrum.DateDue);
                sqlCommand.Parameters.AddWithValue("@is_active", scrum.IsActive);

                if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) == 1)
                {
                    return Ok("Scrum modified successfully");
                }
                else
                {
                    return NotFound("No scrum found with ID " + id);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteScrum(int id)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "DeleteScrum";
                sqlCommand.Parameters.AddWithValue("@id", id);

                if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) > 0)
                {
                    return Ok("Scrum with ID " + id + " deleted successfully");
                }
                else
                {
                    return NotFound("No scrum found with ID " + id);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
