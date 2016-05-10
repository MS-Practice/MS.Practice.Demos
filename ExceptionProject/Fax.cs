using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExceptionProject
{
    class Fax
    {
        //将MailManager对象传递给构造器
        public Fax(MailManager mm) { 
            //构造一个指向FaxMsg回调方法的MailMsgEventHandler
            //委托实例。然后登记MailManagerd的MailMsg事件
            mm.MailMsg +=new MailManager.MailMsgEventHandler(FaxMsg);
        }

        private void FaxMsg(object sender, MailManager.MailMsgEventArgs e) {
            Console.WriteLine("Faxing mail message:");
            Console.WriteLine("From: {0} \n To: {1} \n Subject: {2}\n Body: {3}\n", e.From, e.To, e.Subject, e.Body);
        }

        public void Unregister(MailManager mm) {
            MailManager.MailMsgEventHandler callback =
                new MailManager.MailMsgEventHandler(FaxMsg);
            //注销MailManager的MailMsg事件
            mm.MailMsg -= callback;
        }
    }
}
