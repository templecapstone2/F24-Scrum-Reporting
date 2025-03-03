﻿using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using DataAPI.Utilities;
using DataAPI.Models;
using System.Diagnostics;

namespace DataAPI.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly DBConnect dbConnect;
        SqlCommand? sqlCommand;

        public LoginController(DBConnect dbConnect)
        {
            this.dbConnect = dbConnect;
        }
    }
}
