using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EventArgsClass
{
    #region Step1：定义事件接受者信息
    internal class NewMailEventArgs : EventArgs
    {
        private readonly string m_from, m_to, m_subject;
        public NewMailEventArgs(string from, string to, string subject)
        {
            m_from = from;
            m_to = to;
            m_subject = subject;
        }
        /// <summary>
        /// 发件人
        /// </summary>
        public string From { get { return m_from; } }
        /// <summary>
        /// 接受人
        /// </summary>
        public string To { get { return m_to; } }
        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject { get { return m_subject; } }
    } 
    #endregion

    #region Step2：定义事件成员
    internal class MailManager
    {
        public event EventHandler<NewMailEventArgs> NewMail;
        //第三步：定义个负责引发事件的方法，它通知已登记的对象
        //事件已经发生。如果类是密封的，这个方法要声明私有和非虚
        protected virtual void OnNewMail(NewMailEventArgs e)
        {
            //e.Raise(this, ref NewMail);
            //处于对线程的考虑，现在将委托字段的引用复制到一个临时字段中
            EventHandler<NewMailEventArgs> temp = Interlocked.CompareExchange(ref NewMail, null, null);
            //任何方法登记了对事件的关注，就通知他们
            if (temp != null) temp(this, e);
        }

        //第四步 定一个方法 将输入转化为期望事件
        public void SimulateNewMail(string from, string to, string subject)
        {
            NewMailEventArgs e = new NewMailEventArgs(from, to, subject);
            //调用虚方法通知对象已经发生
            //如果没有类型重写该方法，我们的对象将通知事件的所有登记对象
            OnNewMail(e);
        }
    } 
    #endregion
}
