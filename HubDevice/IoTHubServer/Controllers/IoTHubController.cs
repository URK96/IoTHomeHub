using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IoTHubServer.Controllers
{
    [ApiController]
    [Route("api/iothub")]
    public class IoTHubController : ControllerBase
    {
        private readonly ILogger<IoTHubController> _logger;

        public IoTHubController(ILogger<IoTHubController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "IoTHubController Default";
        }

        [HttpGet("{input:int}")]
        public string Get(int input)
        {
            return "This is integer";
        }

        [HttpGet("{input}")]
        public string Get(string input)
        {
            return input;
        }
    }
}
