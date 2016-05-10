using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EventArgsClass
{
    public static class EventArgExtensions
    {
        public static void Raise<TEventArgs>(this TEventArgs e, Object sender, ref EventHandler<TEventArgs> eventDelegate) where TEventArgs : EventArgs
        { 
            //出于对线程的安全考虑，健在将一个委托字段的引用复制到另一个临时字段中
            EventHandler<TEventArgs> temp = Interlocked.CompareExchange(ref eventDelegate, null, null);
            //任何方法等级了我们事件的关注，就通知它们
            if (temp != null) temp(sender,e);
        }
    }
}
