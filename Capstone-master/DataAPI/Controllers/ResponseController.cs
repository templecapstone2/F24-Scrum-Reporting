using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using DataAPI.Utilities;
using DataAPI.Models;
using System.Diagnostics;

namespace DataAPI.Controllers
{
    [Route("api/[controller]")]
    public class ResponseController : Controller
    {
        private readonly DBConnect dbConnect;
        SqlCommand? sqlCommand;

        public ResponseController(IConfiguration configuration)
        {
            dbConnect = new DBConnect(configuration);
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

        [HttpPut("modify")]
        public IActionResult ModifyResponse([FromBody] Response response)
        {
            sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "AddResponse";
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

        [HttpDelete("{id:int}")]
        public IActionResult DeleteResponse(int id)
        {
            sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "DeleteResponse";
            sqlCommand.Parameters.AddWithValue("@id", id);

            if (dbConnect.DoUpdateUsingCmdObj(sqlCommand) == 1)
            {
                return Ok("Response with ID " + id + " deleted successfully");
            }
            else
            {
                return BadRequest("Error");
            }
        }
    }
}
