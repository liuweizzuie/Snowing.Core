using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing
{
    /// <summary>
    /// 为 Exception 事件提供数据。
    /// </summary>
    public class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// 获取Exception。
        /// </summary>
        public Exception Exception { get; protected set; }

        /// <summary>
        /// 获取 ExceptionEventArgs 的新实例。
        /// </summary>
        public ExceptionEventArgs(Exception exception)
        {
            this.Exception = exception;
        }

        public ExceptionEventArgs()
        {
            this.Exception = new EmptyException();
        }
    }

    /// <summary>
    /// 空异常，即没有异常的异常。
    /// </summary>
    public class EmptyException : Exception
    {
        public EmptyException() : base(string.Empty) { }
    }
}
