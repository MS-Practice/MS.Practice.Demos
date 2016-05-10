using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventArgsClass
{
    public class FooEventArgs : EventArgs
    {
    }
    public class TypeWithLotsOfEvents
    {
        private readonly EventSet m_eventSet = new EventSet();
        //protected类型使派生类访问集合
        protected EventSet EventSet { get { return m_eventSet; } }

        #region 用于支持Foo事件的代码
        protected static readonly EventKey s_fooEventKey = new EventKey();
        //定义事件的访问方法，用于在集合中增删委托
        public event EventHandler<FooEventArgs> Foo
        {
            add { m_eventSet.Add(s_fooEventKey, value); }
            remove { m_eventSet.Remove(s_fooEventKey, value); }
        }
        //为这个事件定义受保护的虚方法
        protected virtual void OnFoo(FooEventArgs e)
        {
            m_eventSet.Raise(s_fooEventKey, this, e);
        }
        public void SimulateFoo()
        {
            OnFoo(new FooEventArgs());
        }
        #endregion

    }
}
