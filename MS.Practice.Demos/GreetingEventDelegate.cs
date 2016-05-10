using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MS.Practice.Demos
{
    class GreetingEventDelegate
    {
        private int temperature; // 水温
        public delegate void BoilHandler(int param);    //声明委托
        public event BoilHandler boilEvent; //声明事件
        // 烧水
        public void BoilWater()
        {
            for (int i = 0; i <= 100; i++)
            {
                temperature = i;
                #region 方法2 事件对委托的封装
                if (temperature > 95)
                {
                    if (boilEvent != null)
                    {  //如果事件对象注册
                        boilEvent(temperature);
                    }
                } 
                #endregion



                #region 方法1
                //if (temperature > 95)
                //{
                //    MakeAlert(temperature);
                //    ShowMsg(temperature);
                //} 
                #endregion
            }
        }
        //警报器
        public class Alarm
        {
            public void MakeAlert(int param)
            {
                Console.WriteLine("Alarm：嘀嘀嘀，水已经 {0} 度了：", param);
            }
        }
        //显示器
        public class Display 
        {
            public static void ShowMsg(int param)
            {
                Console.WriteLine("Display：水快开了，当前温度：{0}度。", param);
            }
        }
        
        // 发出语音警报
        private void MakeAlert(int param)
        {
            Console.WriteLine("Alarm：嘀嘀嘀，水已经 {0} 度了：", param);
        }
        // 显示水温
        private void ShowMsg(int param)
        {
            Console.WriteLine("Display：水快开了，当前温度：{0}度。", param);
        }
    }

    class Programs {
        static void Main1() {
            //GreetingEventDelegate ged = new GreetingEventDelegate();
            //ged.BoilWater();

            GreetingEventDelegate ged = new GreetingEventDelegate();
            MS.Practice.Demos.GreetingEventDelegate.Alarm am = new GreetingEventDelegate.Alarm();
            ged.boilEvent += am.MakeAlert;  //注册方法
            ged.boilEvent += (new MS.Practice.Demos.GreetingEventDelegate.Alarm()).MakeAlert;   //给匿名对象注册方法
            ged.boilEvent += MS.Practice.Demos.GreetingEventDelegate.Display.ShowMsg;   //注册静态方法
            ged.BoilWater();
            Console.Read();
        }
    }
}
