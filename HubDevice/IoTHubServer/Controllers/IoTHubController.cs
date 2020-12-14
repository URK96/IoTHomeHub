using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using SmallDB;

using DBConstant = SmallDB.Constant.IoTDeviceDBConstant;

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
    }

    [ApiController]
    [Route("api/iothub/devices/command")]
    public class IoTCommandController : ControllerBase
    {
        [HttpGet("{command}")]
        public string Get(string command)
        {
            try
            {
                if (!Directory.Exists("CMD"))
                {
                    Directory.CreateDirectory("CMD");
                }
                
                var dt = DateTime.Now;
                var fileName = $"{dt.Year}{dt.Month}{dt.Day}{dt.Hour}{dt.Minute}{dt.Second}";

                var fi = new FileInfo(Path.Combine("CMD", fileName));

                using (var sw = fi.CreateText())
                {
                    sw.WriteLine(command);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return "x~x";
            }

            return "OK";
        }
    }

    [ApiController]
    [Route("api/iothub/devices")]
    public class IoTDeviceController : ControllerBase
    {
        [HttpGet]
        public List<IoTDevice> Get()
        {
            var list = new List<IoTDevice>();

            SmallDBService.LoadDB();

            foreach (DataRow dr in SmallDBService.DBTable.Rows)
            {
                list.Add(new IoTDevice(dr));
            }

            return list;
        }

        [HttpGet("status/info/{mac}")]
        public string Get(string mac)
        {
            // if (!mac.Equals("00:00:00:00:00:00"))
            // {
            //     return "-1;-1";
            // }

            // return CreateRandomArg();

            SmallDBService.LoadDB();

            var device = from DataRow dr in SmallDBService.DBTable.Rows 
                        where (dr[DBConstant.MAC_ADDRESS] as string).Equals(mac) 
                        select new IoTDevice(dr);

            return device.FirstOrDefault().StatusArgument;
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
