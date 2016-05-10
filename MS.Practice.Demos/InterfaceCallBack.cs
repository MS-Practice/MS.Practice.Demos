using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommomCore;

namespace MS.Practice.Demos
{
    public interface InterfaceCallBack
    {
        void DoSomething();
    }
    /// <summary>
    /// 使用接口实现回调函数
    /// </summary>
    public class  MyBaseClass : InterfaceCallBack
    {
        public void DoSomething() {
            Console.WriteLine("Call interface method."); 
        }

#if DEFILE_INTERFACE
        public static void Main(string[] arg)
        {
            //int iteration = 10 * 1000;
            //string s = "";
            //CodeTimer.Time("String Contact", iteration, () => { s += "a"; });
            //StringBuilder sb = new StringBuilder();
            //CodeTimer.Time("StringBuilder", iteration, () => { sb.Append("a"); });
            //MyClassWithCallBack mc = new MyClassWithCallBack();
            //InterfaceCallBack ic = new MyBaseClass();

            //mc.AddCallback(ic);
            //mc.DoSomething();
            //mc.RemoveCallback();
        } 
#endif
    }

    public class MyClassWithCallBack {
        private InterfaceCallBack _myInterface;
        public void AddCallback(InterfaceCallBack myInterface)
        {
            _myInterface = myInterface;
        }
        public void RemoveCallback()
        {
            _myInterface = null;
        }
        public void DoSomething()
        {
            if (_myInterface != null)
                _myInterface.DoSomething();
        } 
    }
}
