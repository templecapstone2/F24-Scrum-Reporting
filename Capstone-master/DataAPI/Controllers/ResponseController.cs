using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using DataAPI.Utilities;
using DataAPI.Models;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace DataAPI.Controllers
{
    [Route("api/[controller]")]
    public class ResponseController : Controller
    {
        private readonly DBConnect dbConnect;
        SqlCommand? sqlCommand;

        public ResponseController(DBConnect dbConnect)
        {
            this.dbConnect = dbConnect;
        }

        [HttpPost]
        public IActionResult AddResponse([FromBody] Response response)
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "AddResponse";
                sqlCommand.Parameters.AddWithValue("@question_one", response.QuestionOne);
                sqlCommand.Parameters.AddWithValue("@question_two", response.QuestionTwo);
                sqlCommand.Parameters.AddWithValue("@question_three", response.QuestionThree);
                sqlCommand.Parameters.AddWithValue("@date_submitted", response.DateSubmitted);
                sqlCommand.Parameters.AddWithValue("@scrum_id", response.ScrumID);
                sqlCommand.Parameters.AddWithValue("@user_id", response.UserID);

                if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) == 1)
                {
                    return Ok("Response added successfully");
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
        public IActionResult GetResponses()
        {
            try
            {
                List<Response> responses = new List<Response>();
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "GetResponses";
                DataSet ds = dbConnect.GetDataSetUsingCmdObj(sqlCommand);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Response response = new Response();
                        foreach (var property in response.GetType().GetProperties())
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
                                    property.SetValue(response, value);
                                }
                                else
                                {
                                    property.SetValue(response, null);
                                }
                            }
                        }
                        responses.Add(response);
                    }
                    return Ok(responses);
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
        public IActionResult ModifyResponse(int id, [FromBody] Response response)
        {
            try
            {
                if (id != response.ID)
                {
                    return BadRequest("ID in the URL does not match ID in the request body.");
                }

                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "ModifyResponse";
                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlCommand.Parameters.AddWithValue("@question_one", response.QuestionOne);
                sqlCommand.Parameters.AddWithValue("@question_two", response.QuestionTwo);
                sqlCommand.Parameters.AddWithValue("@question_three", response.QuestionThree);

                if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) == 1)
                {
                    return Ok("Response modified successfully");
                }
                else
                {
                    return NotFound("No response found with ID " + id);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteResponses()
        {
            try
            {
                sqlCommand = new SqlCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "DeleteResponses";

                if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) > 0)
                {
                    return Ok("Responses deleted successfully");
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
