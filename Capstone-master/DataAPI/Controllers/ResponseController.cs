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

        [HttpPost("add")]
        public IActionResult AddResponse([FromBody] Response response)
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

        [HttpGet("responses")]
        public IActionResult GetResponses()
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

        [HttpPut("modify")]
        public IActionResult ModifyResponse([FromBody] Response response)
        {
            sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "ModiifyResponse";
            sqlCommand.Parameters.AddWithValue("@id", response.ID);
            sqlCommand.Parameters.AddWithValue("@question_one", response.QuestionOne);
            sqlCommand.Parameters.AddWithValue("@question_two", response.QuestionTwo);
            sqlCommand.Parameters.AddWithValue("@question_three", response.QuestionThree);

            if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) == 1)
            {
                return Ok("Response modified successfully");
            }
            else
            {
                return BadRequest("Error");
            }
        }

        [HttpDelete("clear")]
        public IActionResult DeleteResponses()
        {
            sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "DeleteResponses";

            if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) == 1)
            {
                return Ok("Responses deleted successfully");
            }
            else
            {
                return BadRequest("Error");
            }
        }
    }
}
