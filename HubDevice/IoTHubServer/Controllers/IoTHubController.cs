using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SmallDB;

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

        // [HttpGet("{input:int}")]
        // public string Get(int input)
        // {
        //     return "This is integer";
        // }

        // [HttpGet("{input}")]
        // public string Get(string input)
        // {
        //     return input;
        // }
    }

    [ApiController]
    [Route("api/iothub/devices")]
    public class IoTDeviceController : ControllerBase
    {
        // [HttpGet]
        // public List<IoTDevice> Get()
        // {
        //     var list = new List<IoTDevice>();

        //     SmallDBService.LoadDB();

        //     foreach (DataRow row in SmallDBService.DBTable.Rows)
        //     {
        //         list.Add(new IoTDevice(row));
        //     }
            
        //     return list;
        // }

        [HttpGet]
        public List<IoTDevice> Get()
        {
            var list = new List<IoTDevice>();

            list.Add(new IoTDevice()
            {
                DeviceName = $"TestDevice",
                BTName = $"TestBT",
                MACAddress = "00:00:00:00:00:00",
                Path = "/Test/TestBT",
                SensorType = DeviceType.HTSensor,
                Status = DeviceStatus.Connected,
                StatusArgument = CreateRandomArg()
            });

            return list;
        }

        [HttpGet("status/info/{mac}")]
        public string Get(string mac)
        {
            if (!mac.Equals("00:00:00:00:00:00"))
            {
                return "-1;-1";
            }

            return CreateRandomArg();
        }

        private string CreateRandomMAC()
        {
            var r = new Random((int)DateTime.Now.Ticks);
            var sb = new StringBuilder();

            for (int i = 0; i < 6; ++i)
            {
                sb.Append(r.Next() % 10);
                sb.Append(r.Next() % 10);
                sb.Append((i == 5) ? "" : ":");
            }

            return sb.ToString();
        }

        private string CreateRandomArg()
        {
            var r = new Random((int)DateTime.Now.Ticks);

            return $"{r.Next() % 40};{r.Next() % 100}";
        }
    }
}
