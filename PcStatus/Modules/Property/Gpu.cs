using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcInfoSerchProject.PcStatus.Modules.Property
{
    /// <summary>
    ///     
    /// </summary>
    public class Gpu
    {
        public String GpuName { get; set; } = "";

        public double PackageTemp {  get; set; }

        public double MaxPackageTemp { get; set; }

        public double TotalGpuUsage {  get; set; }

        public double PackageVoltage {  get; set; }
    }
}
