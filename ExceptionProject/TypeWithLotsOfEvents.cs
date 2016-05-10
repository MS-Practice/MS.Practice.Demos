using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExceptionProject
{
    class TypeWithLotsOfEvents
    {
        protected EventHandlerSet eventSet =
            EventHandlerSet.Synchronized(new EventHandlerSet());
        //2.为Foo事件定义个成员
        //2.1 构造一个静态只读对象标志该事件，每个对象都有一个散列码用于在集合当中查找事件对应的委托链表
        protected static readonly Object fooEventKey = new Object();
        //2.2 定一个事件
        public class FooEventArgs : EventArgs { }
        //2.3 为事件定义委托链
        public delegate void FooEventHandler(object sender, FooEventArgs e);
        //2.4 为事件定义添加删除委托
        public event FooEventHandler Foo {
            add { eventSet.AddHandler(fooEventKey, value); }
            remove { eventSet.RemoveHandler(fooEventKey, value); }
        }
        //2.5 为事件定义一个受保护的虚方法
        protected virtual void OnFoo(FooEventArgs e) {
            eventSet.Fire(fooEventKey, this, e);
        }
        //2.6 定一个方法讲输入转化为期望的事件
        public void SimulateFoo() {
            OnFoo(new FooEventArgs());
        }

        //3 下面的步骤同上
        protected static readonly Object barEventKey = new Object();
        public class BarEventArgs : EventArgs { }
        public delegate void BarEventHandler(object sender, BarEventArgs e);
        public event BarEventHandler Bar {
            add { eventSet.AddHandler(barEventKey, value); }
            remove { eventSet.RemoveHandler(barEventKey, value); }
        }
        protected virtual void OnBar(BarEventArgs e) {
            eventSet.Fire(barEventKey, this, e);
        }
        public void SimulateBar() {
            OnBar(new BarEventArgs());
        }
    }
}
