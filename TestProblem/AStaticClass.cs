using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProblem
{
    public static class AStaticClass
    {
        public static void AStaticMethod(string name)
        {
            m_AStaticField = name;
        }
        public static String AStaticProperty
        {
            get { return m_AStaticField; }
            private set { m_AStaticField = value; }
        }

        private static string m_AStaticField { get; set; }
        public static event EventHandler AStaticEvent;
    }
}
