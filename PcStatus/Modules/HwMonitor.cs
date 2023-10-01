using LibreHardwareMonitor.Hardware;
using PcInfoSerchProject.PcStatus.Modules.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcInfoSerchProject.PcStatus.Modules
{
    class HwMonitor
    {
        private List<double> PcVoltages = new List<double>();
        private List<double> PcTemperatures = new List<double>();
        //論理プロセッサーの使用率
        private List<double> PcAllCoreProcess = new List<double>();
        DateTime date;
        /// <summary>
        /// PCのデータを収集するための設定を行う
        /// 設定値
        ///     IsCpuEnabled : Cpuの監視をする
        ///     IsGpuEnabled : Gpuの監視をする
        ///     IsMemoryEnabled : Memoryの監視をする
        ///     IsMotherboardEnabled : Motherboardの監視をする
        ///     IsStorageEnabled : HDD/SSD の監視をする
        /// サンプルコード
        /// <example>
        ///     <code>
        ///         Computer c = new Computer(){
        ///         IsCpuEnabled = true,
        ///         IsGpuEnabled = true,
        ///         IsMemoryEnabled = true,
        ///         IsMotherboardEnabled = true,
        ///         IsNetworkEnabled = true,
        ///         IsStorageEnabled = true
        ///         }
        ///         
        ///         HwMonitor monitor = new HwMonitor(c);
        ///     </code>
        /// </example>
        /// </summary>
        /// <param name="c">収集したいPc情報を設定する</param>
        /// <param name="date">実行日時</param>

        public HwMonitor(Computer c, DateTime date)
        {
            this.date = date;
            c.Open();
            c.Accept(new UpdateVisitor());

            //foreach (IHardware hardware in c.Hardware)
            //{
            //    Debug.Write("Hardware:"+ hardware.Name);

            //    foreach (IHardware subhardware in hardware.SubHardware)
            //    {
            //        Console.WriteLine("\tSubhardware: " + subhardware.Name);

            //        foreach (ISensor sensor in subhardware.Sensors)
            //        {
            //            Debug.Write("\t\tSensor:"+sensor.Value + ", value:" + sensor.Name );
            //        }
            //    }

            //    foreach (isensor sensor in hardware.sensors)
            //    {
            //        debug.write("\t\tsensor:" + sensor.value + ", value:" + sensor.name);
            //    }
            //}
            StartObserv(c);
 
        }

        /// <summary>
        ///     各ハードウェアタイプごとに処理を適切に割り振るためのメソッド
        /// </summary>
        /// <param name="c">Computer</param>
        private void StartObserv(Computer c)
        {
            foreach (IHardware hardware in c.Hardware)
            {
                switch (hardware.HardwareType)
                {
                    case HardwareType.Motherboard:
                        break;

                    case HardwareType.GpuNvidia:
                        break;
                    case HardwareType.Cpu:
                        Cpu(hardware);
                        break;
                    case HardwareType.SuperIO:
                        break;
                    case HardwareType.GpuAmd:
                        break;
                    case HardwareType.GpuIntel:
                        break;
                    case HardwareType.Memory:
                        break;
                    case HardwareType.Storage:
                        break;
                    case HardwareType.Cooler:
                        break;
                    case HardwareType.Network:
                    default:
                        break;
                }
            }
        }

        /// <summary>
        ///  Cpu専用の情報を収集するメソッド
        /// </summary>
        /// <param name="h"></param>
        private void Cpu(IHardware h)
        {
            Cpu cpu = new Cpu();
            foreach (ISensor sensor in h.Sensors)
            {
                switch (sensor.SensorType)
                {
                    case SensorType.Voltage:
                        if (sensor.Name.Contains("#"))
                        {
                            PcVoltages.Add(sensor.Value.GetValueOrDefault());
                        }
                        else
                        {
                            cpu.PackageVoltage = sensor.Value.GetValueOrDefault();
                        }
                        break;
                    case SensorType.Temperature:
                        if (sensor.Name.Contains("Package"))
                        {
                            cpu.PackageTemp = sensor.Value.GetValueOrDefault();
                        }
                        else if (sensor.Name.Contains("Max"))
                        {
                            //Cpu全体の観測された温度
                            cpu.MaxPackageTemp = sensor.Value.GetValueOrDefault();
                        }
                        else if (sensor.Name.Contains("TjMax"))
                        {
                            //核物理コアの観測された温度
                        }
                        else if (sensor.Name.Contains("#"))
                        {
                            PcTemperatures.Add(sensor.Value.GetValueOrDefault());
                        }
                        break;
                    case SensorType.Clock:
                        break;
                    case SensorType.Power:
                        break;
                    case SensorType.Load:
                        if (sensor.Name.Contains("Total"))
                        {
                            cpu.TotalCpuUsage = sensor.Value.GetValueOrDefault();
                        }
                        else
                        {
                            PcAllCoreProcess.Add(sensor.Value.GetValueOrDefault());
                        }
                        break;
                }
            }
            GlobalObject.CpuMap.Add(date,cpu);
            GlobalObject.NowCpuData = cpu;
        }
    }

    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }
}
