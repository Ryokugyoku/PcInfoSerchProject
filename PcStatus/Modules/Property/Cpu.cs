using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PcInfoSerchProject.PcStatus.Modules.Property
{
    /// <summary>
    ///     HWMonitorから受け取ったCPU情報を格納するための構造
    /// </summary>
    internal class Cpu
    {
        /// <summary>
        /// 　物理コアの番号順に並んでいる電圧
        /// </summary>
        private List<double> voltages = new List<double>();
        /// <summary>
        ///  物理コアの番号順に並んでいる　温度
        /// </summary>
        private List<double> temperatures = new List<double>();
        /// <summary>
        ///  論理コアの番号順に並んでいるCPU使用率
        /// </summary>
        private List<double> allCoreProcess = new List<double>();
        private double totalUsage;
        private double packageTemp;
        private double packageVoltage;


        public List<double> Voltages { get { return voltages; } set { voltages = value; } }
        public List<double> Temperatures { get {  return temperatures; } set {  temperatures = value; } }
        public List<double> AllCoreProcess { get { return allCoreProcess;  } set { allCoreProcess = value; }}
        public double Toltages { get { return totalUsage; } set { totalUsage = value; } }
        public double PackageTemp { get { return packageTemp; }  set { packageTemp = value; } }
        public double PackageVoltage { get {  return packageVoltage; } set { packageVoltage = value; } }

    }

    internal class WmicCpuProperty
    {
        private int idProcess;

        private String name;

        private int percentUserTime;

        public int IDProcess { get { return idProcess; } set { idProcess = value; } }

        public String Name { get { return name; } set { name = value; } }

        public int PercentUserTime { get { return percentUserTime; } set { percentUserTime = value; } }
    }
}
