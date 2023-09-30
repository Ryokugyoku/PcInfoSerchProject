using PcInfoSerchProject.PcStatus.Modules.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcInfoSerchProject.PcStatus
{
    static class GlobalObject
    {
        private static  IDictionary<DateTime, List<Cpu>> cpuMap = new Dictionary<DateTime, List<Cpu>>();
        private static IDictionary<DateTime, List<WmicCpuProperty>> cpuUsagePerProcess = new Dictionary<DateTime, List<WmicCpuProperty>>();

        /// <summary>
        ///     キー日時　Value:Cpu
        /// </summary>
        public static IDictionary<DateTime, List<Cpu>> CpuMap {  get { return cpuMap; } set { cpuMap = value; } }
        /// <summary>
        ///  キー日時 value : WmicCpuProperty(プロセスごとの使用率)
        /// </summary>
        public static IDictionary<DateTime,List<WmicCpuProperty>> CpuUsagePerProcess { get { return cpuUsagePerProcess; } set { cpuUsagePerProcess = value; } }
    }
}
