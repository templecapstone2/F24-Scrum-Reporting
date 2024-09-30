using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using DataAPI.Utilities;
using DataAPI.Models;
using System.Diagnostics;

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

        [HttpPost("add")]
        public IActionResult AddScrum([FromBody] Scrum scrum)
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

        [HttpGet("scrums")]
        public IActionResult GetScrums()
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
                        if (row.Table.Columns.Contains(property.Name))
                        {
                            object value = row[property.Name];

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

        [HttpPut("modify")]
        public IActionResult ModifyScrum([FromBody] Scrum scrum)
        {
            sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "ModifyScrum";
            sqlCommand.Parameters.AddWithValue("@id", scrum.ID);
            sqlCommand.Parameters.AddWithValue("@name", scrum.Name);
            sqlCommand.Parameters.AddWithValue("@date_due", scrum.DateDue);
            sqlCommand.Parameters.AddWithValue("@is_active", scrum.IsActive);
            //idk if this will work or I want to pass ID separately and use it as a url parameter
            if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) == 1)
            {
                return Ok("Scrum modified successfully");
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteScrum(int id)
        {
            sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "DeleteScrum";
            sqlCommand.Parameters.AddWithValue("@id", id);

            if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) == 1)
            {
                return Ok("Scrum with ID " + id + " deleted successfully");
            }
            else
            {
                return BadRequest("Error");
            }
        }
    }
}
