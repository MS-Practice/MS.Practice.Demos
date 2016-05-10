using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace ExceptionProject
{
    

    public class MyClass:EventArgs
    {
        public event EventHandler MyEvent;

        public virtual void OnMailMsg(MyClass e)
        {
            //有对象登记事件
            if (MyEvent != null)
            {
                //如果有，则通知委托链上的所有对象
                MyEvent(this, e);
            }
        }
    }

    /// <summary>
    /// 将事件作为对象
    /// </summary>
    /// <typeparam name="TDelegate"></typeparam>
    public class DelegateEvent<TDelegate> {
        public Action<TDelegate> _add;
        public Action<TDelegate> _remove;

        public DelegateEvent(Action<TDelegate> add, Action<TDelegate> remove) {
            this.CheckeDelegateType();
            if (add == null) throw new ArgumentNullException("add");
            if (remove == null) throw new ArgumentNullException("remove");
            this._add = add;
            this._remove = remove;
        }

        public DelegateEvent(object obj, string eventName) {
            this.CheckeDelegateType();
            if (obj == null) throw new ArgumentNullException("obj");
            if (string.IsNullOrEmpty(eventName)) throw new ArgumentNullException("eventName");
            this.BindEvent(obj.GetType(), obj, eventName);
        }

        public DelegateEvent(Type type, string eventName) {
            this.CheckeDelegateType();
            if (type == null) throw new ArgumentNullException("type");
            if (String.IsNullOrEmpty(eventName)) throw new ArgumentNullException("eventName");

            this.BindEvent(type, null, eventName);
        }

        public DelegateEvent(Expression<Func<TDelegate>> eventExpression) {
            this.CheckeDelegateType();
            // () => obj.EventName
            if (eventExpression == null) throw new ArgumentNullException("eventExpr");

            // obj.EventName
            var memberExpr = eventExpression.Body as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentNullException("eventExpr", "Not an event");
            object instance = null;
            if (memberExpr.Expression != null) {
                try
                {
                    var instanceExpr = Expression.Lambda<Func<object>>(memberExpr.Expression);
                    instance = instanceExpr.Compile().Invoke();
                }
                catch (Exception ex)
                {
                    throw new ArgumentNullException("eventExpr is not an event", ex);
                }
            }
            this.BindEvent(memberExpr.Member.DeclaringType, instance, memberExpr.Member.Name);
        }

        private void BindEvent(Type type,object obj,string eventName){
            var eventInfo = type.GetEvent(eventName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | (obj == null ? BindingFlags.Static : BindingFlags.Instance));

            if(eventInfo==null)
                throw new ArgumentException(String.Format("Event {0} is missing in {1}",eventName, type.FullName));
            if(eventInfo.EventHandlerType !=typeof(TDelegate))
                throw new ArgumentException(String.Format("Type of event {0} in {1} is mismatched with {2}.",eventName, type.FullName, typeof(TDelegate).FullName));

            this._add = h => eventInfo.AddEventHandler(obj, (Delegate)(object)h);
            this._remove = h => eventInfo.RemoveEventHandler(obj, (Delegate)(object)h);
        }

        private void CheckeDelegateType() { 
            if(!typeof(Delegate).IsAssignableFrom(typeof(TDelegate)))
                throw new ArgumentNullException("TDelegate must be an Delegate type");
        }

        public DelegateEvent<TDelegate> AddHandler(TDelegate handler) {
            _add(handler);
            return this;
        }
        public DelegateEvent<TDelegate> RemoveHandler(TDelegate handler) {
            _remove(handler);
            return this;
        }
    }

    public static class EventFactory {
        public static DelegateEvent<T> Create<T>(Expression<Func<T>> eventExpr) {
            return new DelegateEvent<T>(eventExpr);
        }
    }
}
