using PcInfoSerchProject.PcStatus.Modules;
using System.Threading;

namespace PcInfoSerchProject
{
    /// <summary>
    ///     PC監開始
    /// </summary>
    public  class StartObserv
    {
        /// <summary>
        ///  スナップショット開始　1秒間隔
        /// </summary>
        public StartObserv() {
            Thread t = new Thread(new ParameterizedThreadStart(SnapShotThread));
            t.Start(1);
        }

        /// <summary>
        ///     スナップショット開始　
        /// </summary>
        /// <param name="sec">スナップショット保存間隔：秒数</param>
        /// <example>
        ///     <code>
        ///         //１０秒間隔でスナップショットを収集するとき
        ///         StartObserv obSrv = new StartObserv(10);
        ///     </code>
        /// </example>
        public StartObserv(int sec)
        {
            Thread t = new Thread(new ParameterizedThreadStart(SnapShotThread));
            t.Start(sec);
        }

        private void SnapShotThread(Object sec) { 
            int snapSec = (int)sec;
            DateTime date = DateTime.Now;
            CpuObserv cpu = new CpuObserv();
            Thread cpuObserv = new Thread(new ParameterizedThreadStart(cpu.SnapShot));
            cpuObserv.Start(date);
        }
    }
}
