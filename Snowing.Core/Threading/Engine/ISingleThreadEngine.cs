using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing.Threading.Engine
{
    /// <summary>
    /// 单线程引擎接口。
    /// </summary>
    public interface ISingleThreadEngine
    {
        /// <summary>
        /// 启动引擎。
        /// </summary>
        void Start();

        /// <summary>
        /// 停止引擎线程。
        /// </summary>
        void Stop();

        /// <summary>
        /// 是否正在运行。
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// 引擎周期性执行的时间间隔，以毫秒为单位。
        /// </summary>
        int DetectSpanInMs { get; set; }

        /// <summary>
        /// 当引擎停止时发生。如果是正常停止，则ExceptionEventArgs的Exception属性为EventArgs.Empty。
        /// </summary>
        event EventHandler<ExceptionEventArgs> OnEngineStopped;
    }
}
