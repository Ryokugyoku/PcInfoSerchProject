
using LibreHardwareMonitor.Hardware;
using PcInfoSerchProject.PcStatus.Modules.Property;

using System.Diagnostics;


namespace PcInfoSerchProject.PcStatus.Modules
{

    /// <summary>
    ///     Cpu監視用クラス
    /// </summary>
    class CpuObserv
    {
        /// <summary>
        ///     CPU状態のスナップショットを取得し、グローバルオブジェクトに格納する
        /// </summary>
        /// <param name="date">
        ///    キーとなる実行時間
        /// </param>
        public void SnapShot(Object date) {
            List<WmicCpuProperty> cpuPropertyList = getCpuUsagePerProcess();
            GlobalObject.CpuUsagePerProcessMap.Add((DateTime)date, cpuPropertyList);
            GlobalObject.NowWmicData = cpuPropertyList;
            Computer c = new Computer() { 
                IsCpuEnabled = true
            };
            HwMonitor hmonitor = new (c,(DateTime)date);
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
}
