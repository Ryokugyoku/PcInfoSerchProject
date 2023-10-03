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
    ///     
    /// </summary>
    public class Cpu
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
        /// <summary>
        ///  CPU全体の使用率
        /// </summary>
        private double totalUsage;
        /// <summary>
        ///  CPU全体の温度
        /// </summary>
        private double packageTemp;
        /// <summary>
        ///  CPU全体の使用電力
        /// </summary>
        private double packageVoltage;
        /// <summary>
        ///  最大温度
        /// </summary>
        private double maxPackageTemp;

        /// <summary>
        ///  物理コア事の電圧情報を持っているオブジェクトを格納/返す
        ///  格納される順番は、コアNo順になっている
        /// </summary>
        public List<double> Voltages { get { return voltages; } set { voltages = value; } }

        /// <summary>
        ///  物理コア事の温度を持っておりオブジェクトを格納/返す
        ///  格納される順番は、コアNo順になっている
        /// </summary>
        public List<double> Temperatures { get {  return temperatures; } set {  temperatures = value; } }

        /// <summary>
        ///  論理コア事の使用率を格納委している
        ///  格納される順番は論理コアNoの順になっている。
        ///  物理コアと数が異なるため注意が必要
        /// </summary>
        public List<double> AllCoreProcess { get { return allCoreProcess;  } set { allCoreProcess = value; }}

        /// <summary>
        ///  CPU全体の温度を格納/返す
        /// </summary>
        public double PackageTemp { get { return packageTemp; }  set { packageTemp = value; } }

        /// <summary>
        ///  CPU全体の電圧を格納/返す
        /// </summary>
        public double PackageVoltage { get {  return packageVoltage; } set { packageVoltage = value; } }

        /// <summary>
        ///   CPU全体の使用率
        /// </summary>
        public double TotalCpuUsage { get { return totalUsage; } set { totalUsage = value; } }

        /// <summary>
        ///  観測された最大のCPU温度を持っている
        /// </summary>
        public double MaxPackageTemp {  get { return maxPackageTemp; } set {  maxPackageTemp = value; } }

    }

    class WmicCpuProperty
    {
        private int idProcess;

        private String name;

        private int percentUserTime;

        public int IDProcess { get { return idProcess; } set { idProcess = value; } }

        public String Name { get { return name; } set { name = value; } }

        public int PercentUserTime { get { return percentUserTime; } set { percentUserTime = value; } }
    }
}
