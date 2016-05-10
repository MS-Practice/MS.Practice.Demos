using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EventArgsClass
{
    public sealed class EventKey : Object { }
    public sealed class EventSet
    {
        //私有字典用于维护EventKey ->Delegate映射
        private readonly Dictionary<EventKey, Delegate> m_events =
            new Dictionary<EventKey, Delegate>();
        //添加一个EventKey->Delegate映射（如果EventKey不存在）
        //或者将一个委托与一个现有的EventKey合并
        public void Add(EventKey eventKey, Delegate handler)
        {
            Monitor.Enter(m_events);
            Delegate d;
            m_events.TryGetValue(eventKey, out d);
            m_events[eventKey] = Delegate.Combine(d, handler);
            Monitor.Exit(m_events);
        }
        //从EventKey中删除一个委托，如果删除了最后一个字典委托则清除字典映射
        public void Remove(EventKey eventKey, Delegate handler)
        {
            Monitor.Enter(m_events);
            //调用TryGetValue，确保在尝试从集合中删除一个不存在的EventKey时
            //不会抛出一个异常
            Delegate d;
            if (m_events.TryGetValue(eventKey, out d)) {
                d = Delegate.Remove(d, handler);
                //如果还有委托，就设置新的头部地址，否则删除EventKey
                if (d != null) m_events[eventKey] = d;
                else m_events.Remove(eventKey);
            }
            Monitor.Exit(m_events);
        }
        //为指定的EventKey引发事件
        public void Raise(EventKey eventKey, Object sender, EventArgs e) { 
            //如果EventKey不在集合中，不抛出一个异常
            Delegate d;
            Monitor.Enter(m_events);
            m_events.TryGetValue(eventKey, out d);
            Monitor.Exit(m_events);
            if (d != null)
            { 
                //由于字典可能包含不同类型的委托
                //所以无法在编译的时候构造一个类型安全的委托调用
                //因此，我们调用委托类下的DynamicInvoke方法
                //以一个对象数组的形式传递回调方法的参数
                //在内部，DynamicInvoke会向调用的回调方法查证参数的类型安全性，并调用方法
                //如果存在不匹配的情况，DynamicInvoke会抛出一个异常
                d.DynamicInvoke(new object[] { sender, e });
            }
        }
    }
}
