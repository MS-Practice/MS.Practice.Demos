using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventArgsClass
{
    internal sealed class Fax
    {
        public Fax(MailManager mm) { 
            //构造一个委托实例，绑定一个FaxMsg回调方法(注册绑定事件)
            mm.NewMail +=FaxMsg;
        }
        /// <summary>
        /// 新电子邮件到达，MailManager调用这个方法
        /// </summary>
        /// <param name="sender">表示MailManager对象，便于将信息传回给它</param>
        /// <param name="e">NewMailEventArgs对象想传给我们的附加事件信息</param>
        private void FaxMsg(Object sender, NewMailEventArgs e)
        {
            Console.WriteLine("Faxing Mail message:");
            Console.WriteLine("From={0},To={1},Subject={2}", e.From, e.To, e.Subject);
        }
        public void Unregister(MailManager mm) { 
            //想MailManager的NewMail事件注销自己对这个事件的关注
            mm.NewMail -= FaxMsg;
        }
    }
}
