
using LibreHardwareMonitor.Hardware;
using PcInfoSerchProject.PcStatus.Modules.Property;

using System.Diagnostics;


namespace PcInfoSerchProject.PcStatus.Modules
{
    
    internal class CpuObserv
    {
        private IDictionary<DateTime, List<Cpu>> cpuMap = new Dictionary<DateTime, List<Cpu>>();
        private IDictionary<DateTime, List<WmicCpuProperty>> cpuUsage = new Dictionary<DateTime, List<WmicCpuProperty>>();
        /// <summary>
        ///     コンストラクタ・処理なし
        /// </summary>
        public CpuObserv() { 
        
        }
        /// <summary>
        ///     CPU状態のスナップショットを取得し、グローバルオブジェクトに格納する
        /// </summary>
        /// <param name="date">
        ///    キーとなる実行時間
        /// </param>
        public void SnapShot(Object date) {
            List<WmicCpuProperty> cpuPropertyList = getCpuUsagePerProcess();
            GlobalObject.CpuUsagePerProcess.Add((DateTime)date, cpuPropertyList);
            HwMonitor hmonitor = new HwMonitor();
        }

        /// <summary>
        ///     プロセスごとのCPU使用率を取得する。_Totalに全体の使用率も格納されている
        /// </summary>
        /// <returns></returns>
        private List<WmicCpuProperty> getCpuUsagePerProcess() {
            List<WmicCpuProperty> cpuPropertyList = new List<WmicCpuProperty>();
            String output = getProcessRslt(0);
            String title = "";
            String value = "";
            WmicCpuProperty cpuProperty = new WmicCpuProperty();

            Boolean insertFlg = false;
            int count = 1;
            for (int i = 0; i < output.Length; i++)
            {
                char c = output[i];
                switch (c)
                {
                    case '\n':
                        if (count % 2 == 0)
                        {
                            insertFlg = true;
                        }
                        break;
                    case '\r':
                        break;
                    case '=':
                        count++;
                        break;
                    default:
                        if (count % 2 == 0 && count != 0)
                        {
                            value = value + c;
                        }
                        else
                        {
                            title = title + c;
                        }
                        break;
                }
                if (insertFlg && count % 6 == 0)
                {
                    cpuProperty = setCpuProperty(cpuProperty, title, value);
                    cpuPropertyList.Add(cpuProperty);
                    cpuProperty = new WmicCpuProperty();
                    value = "";
                    title = "";
                    insertFlg = false;

                    count++;

                }
                if (insertFlg)
                {
                    cpuProperty = setCpuProperty(cpuProperty, title, value);
                    value = "";
                    title = "";
                    insertFlg = false;
                    count++;
                }
            }

                return cpuPropertyList;
        }

        /// <summary>
        ///  値を変換してWmicCpuPropertyに格納する
        /// </summary>
        /// <param name="cpuProperty">値を格納するためのオブジェクト</param>
        /// <param name="title">プロセス名</param>
        /// <param name="value">値</param>
        /// <returns>引数に設定されたValueとを設定したオブジェクトを返す</returns>
        private WmicCpuProperty setCpuProperty(WmicCpuProperty cpuProperty, String title, String value)
        {

            switch (title)
            {
                case "IDProcess":
                    cpuProperty.IDProcess = int.Parse(value);
                    break;

                case "Name":
                    cpuProperty.Name = value;
                    break;

                case "PercentUserTime":
                    cpuProperty.PercentUserTime = int.Parse(value);
                    break;

                default:
                    break;
            }
            return cpuProperty;
        }

        /// <summary>
        ///     引数をもとにCMDコマンドを実行するためのプロセスを返す
        /// </summary>
        /// <param name="flg">
        ///     引数の値によって下記のようなCMDコマンドの実行結果を返す
        ///     0 = Cpu使用率　※各プロセスID事
        /// </param>
        /// <returns>
        ///     引数に応じてProcessを返す
        /// </returns>
        private String getProcessRslt(int flg)
        {
            ProcessStartInfo process = new ProcessStartInfo();
            process.FileName = "c:\\Windows\\System32\\cmd";
            process.Verb = "runas";
            switch (flg)
            {
                case 0:
                    process.Arguments = "/c WMIC PATH Win32_PerfFormattedData_PerfProc_Process WHERE \"PercentUserTime > 1\" GET Name,IDProcess,PercentUserTime /FORMAT:List";
                    break;

                case 1:

                    break;

                default:
                    break;
            }

            //シェル機能を利用しない
            process.RedirectStandardOutput = true;
            //cmdウィンドウを表示しない
            process.CreateNoWindow = true;
            //プロセスオブジェクトの作成と起動
            Process startProcess = Process.Start(process);
            StreamReader stream = startProcess.StandardOutput;
            startProcess.WaitForExit();
            return stream.ReadToEnd(); ;
        }
    }

    internal class HwMonitor {
        private List<double> voltages = new List<double>();
        private List<double> temperatures = new List<double>();
        private List<double> allCoreProcess = new List<double>();
        /// <summary>
        ///     HwMonitorから値を収集する
        /// </summary>
        public HwMonitor() {
            Computer c = new Computer()
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
                IsMotherboardEnabled = true,
                IsControllerEnabled = true,
                IsNetworkEnabled = true,
                IsStorageEnabled = true
            };
            c.Open();
            c.Accept(new UpdateVisitor());

            foreach (IHardware hardware in c.Hardware)
            {
                Debug.Write("Hardware:"+ hardware.Name);

                foreach (IHardware subhardware in hardware.SubHardware)
                {
                    Console.WriteLine("\tSubhardware: " + subhardware.Name);

                    foreach (ISensor sensor in subhardware.Sensors)
                    {
                        Debug.Write("\t\tSensor:"+sensor.Value + ", value:" + sensor.Name );
                    }
                }

                foreach (ISensor sensor in hardware.Sensors)
                {
                    Debug.Write("\t\tSensor:" + sensor.Value + ", value:" + sensor.Name);
                }
            }

            StartObserv(c);
        }

        /// <summary>
        ///     各ハードウェアタイプごとに処理を適切に割り振るためのメソッド
        /// </summary>
        /// <param name="c">Computer</param>
        private void StartObserv(Computer c) {
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
        private void Cpu(IHardware h) {


            Cpu cpu = new Cpu();
            foreach (IHardware subhardware in h.SubHardware) {

            }
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
