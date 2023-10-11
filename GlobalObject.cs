using PcInfoSerchProject.PcStatus.Modules.Property;

namespace PcInfoSerchProject
{
    /// <summary>
    ///    主にPCで収集したデータを一時的に保存する
    /// </summary>
    
    public static class GlobalObject
    {
        /// <summary>
        ///     最新の監視情報を持っている
        /// </summary>
        private static Cpu nowCpuData = new();
        /// <summary>
        ///  最新のプロセスごとの使用率を格納している
        /// </summary>
        private static List<WmicCpuProperty> nowWmicData = new();
        /// <summary>
        ///    キー　実行日時　Value Cpu
        /// </summary>
        private static IDictionary<DateTime, Cpu> cpuMap = new Dictionary<DateTime, Cpu>();
        /// <summary>
        ///  キー　実行日時 Value WmicCpuProperty /サービスごとのCPU利用率 
        /// </summary>
        private static IDictionary<DateTime, List<WmicCpuProperty>> cpuUsagePerProcessMap = new Dictionary<DateTime, List<WmicCpuProperty>>();

        /// <summary>
        ///     キー日時　Value:Cpu
        /// </summary>
        public static IDictionary<DateTime, Cpu> CpuMap { get { return cpuMap; } set { cpuMap = value; } }
        /// <summary>
        ///  キー日時 value : WmicCpuProperty(プロセスごとの使用率)
        /// </summary>
        public static IDictionary<DateTime, List<WmicCpuProperty>> CpuUsagePerProcessMap { get { return cpuUsagePerProcessMap; } set { cpuUsagePerProcessMap = value; } }

        /// <summary>
        ///     最新の監視情報を格納/取得する
        /// </summary>
        public static Cpu NowCpuData { get { return nowCpuData;} set { nowCpuData = value; } } 

        /// <summary>
        ///     最新のプロセスごとの使用率を格納/取得する
        /// </summary>
        public static List<WmicCpuProperty> NowWmicData { get { return nowWmicData; } set { lock (nowWmicData) { nowWmicData = value; } } }
    }
}
