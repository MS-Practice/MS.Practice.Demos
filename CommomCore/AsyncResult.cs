using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CommomCore
{
    public class AsyncResult<T> : IAsyncResult
    {
        public AsyncResult(object asyncState)
        {
            this.AsyncState = asyncState;
            this.IsCompleted = true;
            this.AsyncWaitHandle = new ManualResetEvent(false);
        }
        public object AsyncState { get; private set; }

        public bool CompletedSynchronously { get { return false; } }

        public bool IsCompleted { get; private set; }


        /// <summary>
        /// 摘要:
        /// 操作完成，并通知其他等待的线程，让其继续访问
        /// </summary>
        public void Complete() {
            this.IsCompleted = true;
            (this.AsyncWaitHandle as ManualResetEvent).Set();
        }
        public T Result { get; set; }
        public Exception Exception { get; set; }


        public WaitHandle AsyncWaitHandle
        {
            get;
            private set;
        }
    }
}
