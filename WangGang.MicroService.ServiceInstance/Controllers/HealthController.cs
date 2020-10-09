﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WangGang.MicroService.ServiceInstance.Controllers
{
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private IConfiguration _configuration;

        public HealthController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            Console.WriteLine($"This is HealthController  {this._configuration["Service:Port"]} Invoke");

            return Ok();
        }
    }
}
