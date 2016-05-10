using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExceptionProject
{
    class MailManager
    {
        //在MailManager内部定义MailMsgEventArgs类型
        public class MailMsgEventArgs : EventArgs { 
            //1、传递给事件接受者的类型定义信息
            public MailMsgEventArgs(string from, string to, string subject, string body) {
                this.From = from;
                this.To = to;
                this.Subject = subject;
                this.Body = body;
            }
            public readonly string From, To, Subject, Body;
        }

        //2. 下面的委托类型定义了接受者必须实现的回调方法原型
        public delegate void MailMsgEventHandler(object sender, MailMsgEventArgs e);

        //3.事先成员
        public event MailMsgEventHandler MailMsg;

        ////3.1 显示定义一个私有委托链表字段
        //private MailMsgEventHandler mailMsgEventHandlerDelegate;
        ////3.2 显示定义事件及访问器的方法
        //public event MailMsgEventHandler MailMsg
        //{
        //    add {
        //        mailMsgEventHandlerDelegate = (MailMsgEventHandler)Delegate.Combine(mailMsgEventHandlerDelegate, value);
        //    }
        //    remove {
        //        mailMsgEventHandlerDelegate = (MailMsgEventHandler)Delegate.Remove(mailMsgEventHandlerDelegate, value);
        //    }
        //}


        //4.下面的受保护的虚方法负责通知事件的登记对象
        protected virtual void OnMailMsg(MailMsgEventArgs e) { 
            //有对象登记事件
            if (MailMsg != null) {
                //如果有，则通知委托链上的所有对象
                MailMsg(this, e);
            }
        }

        //5.下面的方法将输入转化为期望的事件，该方法在新的电子邮件到达时被调用
        public void SimulateArrivingMsg(string from, string to, string subject, string body) { 
            //构造一个对象保存希望传递给通知接受者的信息
            MailMsgEventArgs e =
                new MailMsgEventArgs(from, to, subject, body);
            //调用虚方法通知对象事件已经发生
            //如果派生类没有重写该虚方法
            //对象将通知所有登记的事件侦听者
            OnMailMsg(e);
        }
    }
}
