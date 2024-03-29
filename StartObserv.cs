﻿using PcInfoSerchProject.PcStatus.Modules;

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
        ///     
        ///<example>
        /// １０秒間隔でスナップショットを収集するとき
        ///     <code>
        ///         StartObserv obSrv = new StartObserv(10);
        ///     </code>
        /// </example>
        /// </summary>
        /// <param name="sec">スナップショット保存間隔：秒数</param>
        public StartObserv(int sec)
        {
            Thread t = new Thread(new ParameterizedThreadStart(SnapShotThread));
            t.Start(sec);
        }

        /// <summary>
        /// SnapShot用のメソッド
        /// </summary>
        /// <param name="sec">
        ///     引数がNullの時デフォルトで1秒が設定される
        /// </param>
        private void SnapShotThread(Object? sec) {
            sec = sec ?? 1;
            int snapSec = (int)sec;
            for (;true;) {
                DateTime date = DateTime.Now;
                CpuObserv cpu = new CpuObserv();
                GpuObserv gpu = new GpuObserv();
                Task t1 = Task.Factory.StartNew(() => { cpu.SnapShot(date); });
                Task t2 = Task.Factory.StartNew(() => { gpu.SnapShot(date); });
                
                Task.WaitAll(t1,t2);
            }
        }
    }
}
