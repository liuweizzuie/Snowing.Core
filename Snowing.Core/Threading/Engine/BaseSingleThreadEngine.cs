using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Snowing.Threading.Engine
{
    public abstract class BaseSingleThreadEngine : ISingleThreadEngine
    {
        private ManualResetEvent manualResetEvent4Running = new ManualResetEvent(true);  //read-locker. (读锁)
        private Mutex mtIsRunning = new Mutex(false);  //write-locker.(写锁)

        protected int totalSleepCount = 0;

        #region SleepTimeInMs
        /// <summary>
        /// 每次引擎休眠的最小单位，一般是个位数字毫秒。
        /// </summary>
        public int SleepTimeInMs { get; set; }
        #endregion

        #region DetectSpanInMs
        /// <summary>
        /// 每再次任务执行的间隔，一般从数十毫秒到秒之间。
        /// </summary>
        public int DetectSpanInMs { get; set; }
        #endregion

        #region IsRunning
        private volatile bool isStop = true;
        public bool IsRunning
        {
            get
            {
                this.manualResetEvent4Running.WaitOne();  //request for reading. (请求读)
                return !this.isStop;
            }
            protected set
            {
                mtIsRunning.WaitOne();                 //lock for writing, and lock for read-locker,too. (加写锁，同时为读锁加锁)
                this.manualResetEvent4Running.Reset(); //lock for reading. 加读锁
                this.isStop = !value;
                this.manualResetEvent4Running.Set();   //解读锁
                mtIsRunning.ReleaseMutex();            //解除写锁
            }
        }
        #endregion

        /// <summary>
        /// 引擎任务。
        /// </summary>
        /// <returns>用户决定是否继续执行：返回true，下次继续执行；返回false，下次引擎停止。</returns>
        protected abstract bool EngineTask();

        public void Start()
        {
            if (this.SleepTimeInMs <= 0) { throw new ArgumentException("SleepTimeInMs"); }
            if (this.DetectSpanInMs <= 0) { throw new ArgumentException("DetectSpanInMs"); }
            this.totalSleepCount = this.DetectSpanInMs / this.SleepTimeInMs;
            if (this.DetectSpanInMs < 0) { return; }

            if (this.IsRunning) { return; }
            this.IsRunning = true;

            Thread t = new Thread(new ThreadStart(this.Work));

            t.Name = this.GetType().Name;
            t.Start();

            OnStart();
        }

        /// <summary>
        /// 由子类重载，写写日志什么的。
        /// </summary>
        protected virtual void OnStart() { }

        public void Stop()
        {
            if (!this.IsRunning) { return; }
            this.IsRunning = false;
        }

        protected virtual void Work()
        {
            try
            {
                while (IsRunning)
                {
                    if (!EngineTask())
                    {
                        this.IsRunning = false;
                        break;
                    }

                    #region Sleep
                    for (int i = 0; i < this.totalSleepCount; i++)
                    {
                        if (!this.IsRunning) { break; }
                        Thread.Sleep(this.SleepTimeInMs);

                    }
                    #endregion
                }

                this.RaiseOnEngineStoppedEvent();
            }
            catch (Exception exp)
            {
                this.IsRunning = false;
                this.RaiseOnEngineStoppedEvent(exp);
            }
        }

        /// <summary>
        /// 使用默认参数的DetectSpanInMs = 200ms,SleepTimeInMs = 10ms获取BaseSingleThreadEngine的新实例。
        /// </summary>
        public BaseSingleThreadEngine() : this(200, 10) { }

        /// <summary>
        /// 构造一个新的单循环引擎
        /// </summary>
        /// <param name="detectSpanInMs">每两次任务执行的间隔</param>
        /// <param name="sleepTimeInMs">每次休眠的时间,决定了引擎的调度精度</param>
        public BaseSingleThreadEngine(int detectSpanInMs, int sleepTimeInMs)
        {
            if (detectSpanInMs <= 0) { throw new ArgumentException("detectSpanInMs must be greater than 0!"); }
            if (sleepTimeInMs <= 0) { throw new ArgumentException("sleepTimeInMs must be greater than 0!"); }

            this.DetectSpanInMs = detectSpanInMs;
            this.SleepTimeInMs = sleepTimeInMs;

            // this.engine_stopped_handler_ref = new WeakReference(new EventHandler<ExceptionEventArgs>((s, e) => { }));
        }

        #region OnEngineStopped
        /// <summary>
        /// OnEngineStopped 事件。
        /// </summary>
        public event EventHandler<ExceptionEventArgs> OnEngineStopped;  //{ add;remove;}

        /// <summary>
        /// 触发 OnEngineStopped 事件。
        /// </summary>
        protected void RaiseOnEngineStoppedEvent()
        {
            if (OnEngineStopped != null)
            {
                OnEngineStopped(this, new ExceptionEventArgs());
            }
        }

        /// <summary>
        /// 触发 OnEngineStopped 事件。
        /// </summary>
        /// <param name="exception">因为此异常而导致的停止。如果是正常停止，则为<see cref="EmptyException"/></param>
        protected void RaiseOnEngineStoppedEvent(Exception exception)
        {
            if (OnEngineStopped != null)
            {
                OnEngineStopped(this, new ExceptionEventArgs(exception));
            }
        }
        #endregion

        ~BaseSingleThreadEngine()  //不能在这里调用 release 方法。
        {
            if (this.IsRunning)
            {
                this.Stop();
            }
            this.mtIsRunning.Dispose();
            this.manualResetEvent4Running.Dispose();
        }
    }
}
