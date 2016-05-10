using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.CompilerServices;

namespace ExceptionProject
{
    public class EventHandlerSet : IDisposable
    {
        //用于保存“事件键/委托值”对的私有散列表
        private Hashtable events = new Hashtable();

        public virtual Delegate this[Object eventKey] { 
            //对象不在集合中，返回null
            get {
                return (Delegate)events[eventKey];
            }
            set {
                events[eventKey] = value;
            }
        }

        //在指定的散列表对应的委托链上添加组合一个实例委托
        public virtual void AddHandler(object eventKey, Delegate handler) {
            events[eventKey] = Delegate.Combine((Delegate)events[eventKey], handler);
        }

        //在指定的散列表对应的委托链上删除一个实例委托
        public virtual void RemoveHandler(object eventKey, Delegate handler) {
            events[eventKey] = Delegate.Remove((Delegate)events[eventKey], handler);
        }

        //在指定的散列表对应的委托链上触发事件
        public virtual void Fire(object eventKey, object sender, EventArgs e) {
            Delegate d = (Delegate)events[eventKey];
            if (d != null) {
                //d.DynamicInvoke(sender, e);
                d.DynamicInvoke(new object[] { sender, e });
            }
        }

        public void Dispose()
        {
            events = null;
        }

        //下面的方法是对插入EventHandlerSet对象的线程安全的封装
        public static EventHandlerSet Synchronized(EventHandlerSet eventHandlerSet) {
            if (eventHandlerSet != null)
                throw new ArgumentNullException("eventHandlerSet");
            return new SynchronizedEventHandlerSet(eventHandlerSet);
        }

        private class SynchronizedEventHandlerSet : EventHandlerSet {
            //引用非线程安全的对象
            private EventHandlerSet eventHandlerSet;
            public SynchronizedEventHandlerSet(EventHandlerSet eventHandlerSet)
            {
                this.eventHandlerSet = eventHandlerSet;
                Dispose();
            }

            //线程安全的索引器
            public override Delegate this[object eventKey]
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                get {
                    return eventHandlerSet[eventKey];
                }
                [MethodImpl(MethodImplOptions.Synchronized)]
                set {
                    eventHandlerSet[eventKey] = value;
                }
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void AddHandler(object eventKey, Delegate handler)
            {
                eventHandlerSet.AddHandler(eventKey, handler);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void RemoveHandler(object eventKey, Delegate handler)
            {
                eventHandlerSet.RemoveHandler(eventKey, handler);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Fire(object eventKey, object sender, EventArgs e)
            {
                eventHandlerSet.Fire(eventKey, sender, e);
            }
        }
    }
}
