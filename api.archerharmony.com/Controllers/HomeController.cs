﻿using Microsoft.AspNetCore.Mvc;

namespace api.archerharmony.com.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Test Home");
        }
    }
}
