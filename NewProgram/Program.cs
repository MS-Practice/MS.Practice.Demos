using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommomCore;
//using WriteLine = System.Console;

namespace NewProgram
{
    //using DRead = System.Reflection.Assembly;
    class Program
    {
        static void Main(string[] args)
        {
            //Number num = new Number();
            //num.ShowNumber();   //120
            //IntNumber intNum = new IntNumber();
            //intNum.ShowNumber();        //Base number is 120 New number is 456
            //intNum.ShowInfo();  //Derived class--- 还是基类的bass class---

            //Number number = new IntNumber();
            //number.ShowInfo();  //调用IntNumber下的ShowInfo  new隐藏父类的ShowInfo方法
            //number.ShowNumber();    //因为子类重写 肯定是子类的ShowNumber方法
            ////using用来释放对象资源
            //Console.ReadLine();
            int iteration = 10 * 1000;
            string s = "";
            CodeTimer.Time("String Contact", iteration, () => { s += "a"; });
            StringBuilder sb = new StringBuilder();
            CodeTimer.Time("StringBuilder", iteration, () => { sb.Append("a"); });
        }
    }
}
