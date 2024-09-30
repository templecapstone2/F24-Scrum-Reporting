﻿using Microsoft.AspNetCore.Mvc;
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
        //do I want to add a team controller?
        private readonly DBConnect dbConnect;
        SqlCommand? sqlCommand;

        public UserController(IConfiguration configuration)
        {
            dbConnect = new DBConnect(configuration);
        }

        [HttpPost("team/add")]
        public IActionResult AddTeam([FromBody] Team team)
        {
            sqlCommand = new SqlCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "AddResponse";
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

        [HttpGet("teams")]
        public IActionResult GetTeams()
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
                        if (row.Table.Columns.Contains(property.Name))
                        {
                            object value = row[property.Name];

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

        [HttpDelete("team/delete/{id:int}")]
        public IActionResult DeleteTeam(int id)
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
                return BadRequest("Error");
            }
        }
    }
}
