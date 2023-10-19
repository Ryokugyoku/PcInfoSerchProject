using LibreHardwareMonitor.Hardware;
using PcInfoSerchProject.PcStatus.Modules.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcInfoSerchProject.PcStatus.Modules
{
    internal class GpuObserv
    {
        public GpuObserv() { 
        
        }

        public void SnapShot(Object? date)
        {
            date = date ?? DateTime.Now;
            Computer c = new Computer()
            {
                IsGpuEnabled = true
            };
            HwMonitor hmonitor = new(c, (DateTime)date);
        }
    }
}
