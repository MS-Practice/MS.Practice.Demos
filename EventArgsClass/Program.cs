using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventArgsClass
{
    class Program
    {
        static void Main(string[] args)
        {
            MailManager mm = new MailManager();
            //Fax fax = new Fax(mm);
            mm.SimulateNewMail("MS", "ZQ", "TEST");

            TypeWithLotsOfEvents twle = new TypeWithLotsOfEvents();
            //添加一个回调
            twle.Foo += HandlerFooEvent;
            twle.SimulateFoo();
        }
        private static void HandlerFooEvent(object sender, FooEventArgs e) {
            Console.WriteLine("Handling Foo Event here...");
        }
    }
}
