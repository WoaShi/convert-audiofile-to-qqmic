using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioToMicWPF.Services
{
    internal class FindDevicesServices
    {
        public ObservableCollection<Devices> outputDevices { get; set; }
        public FindDevicesServices()
        {
            outputDevices = new ObservableCollection<Devices>();
            for (int n = 0; n < WaveOut.DeviceCount; n++)
            {
                var caps = WaveOut.GetCapabilities(n);
                outputDevices.Add(new Devices()
                {
                    DeviceID = n,
                    DeviceName = caps.ProductName
                });
            }
        }

    }

    class Devices
    {
        public int DeviceID { get; set; }

        public string DeviceName { get; set; }
    }
}
