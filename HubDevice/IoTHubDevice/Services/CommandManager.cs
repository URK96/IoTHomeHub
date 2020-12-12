using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

using IoTHubDevice.Models;

namespace IoTHubDevice.Services
{
    public class CommandManager
    {
        private const string CMDPATH = "CMD";

        Process deviceP;

        private Timer commandTimer;

        public CommandManager()
        {
            deviceP = Process.GetCurrentProcess();

            commandTimer = new Timer(GetCommand, new AutoResetEvent(false), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));

            if (!Directory.Exists(CMDPATH))
            {
                Directory.CreateDirectory(CMDPATH);
            }
        }

        private async void GetCommand(object stateInfo)
        {
            try
            {
                commandTimer.Change(-1, -1);

                var cmdFiles = Directory.GetFiles(CMDPATH);

                foreach (var path in cmdFiles)
                {
                    var fi = new FileInfo(path);

                    string cmd = string.Empty;

                    using (var sr = fi.OpenText())
                    {
                        cmd = sr.ReadLine();
                    }

                    string[] cmdSplit = cmd.Split('_');

                    var targetDevice = (from device in AppEnvironment.deviceManager.PairedList
                                        where device.MACAddress.Equals(cmdSplit[0])
                                        select device).FirstOrDefault();

                    if (targetDevice.Status == DeviceStatus.Connected)
                    {
                        AppEnvironment.deviceManager.updateTimer.Stop();
                        await targetDevice.SendCommand(cmdSplit[1]);
                        AppEnvironment.deviceManager.updateTimer.Start();
                    }

                    fi.Delete();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                commandTimer.Change(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            }
        }
    }
}