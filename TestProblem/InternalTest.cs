using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProblem
{
    internal sealed class InternalTest
    {
        public InternalTest() { }

        ~InternalTest()
        {

        }
        public static Boolean operator ==(InternalTest intest1, InternalTest intest2) { return true; }
        public static Boolean operator !=(InternalTest intest1, InternalTest intest2) { return false; }
        public static InternalTest operator +(InternalTest intest1, InternalTest intest2) { return null; }
        public String AProperty
        {
            get { return null; }
            set { }
        }
        //一个索引器
        public String this[Int32 x] {
            get { return null; }
            set { }
        }
        event EventHandler AnEvent;
    }
}
