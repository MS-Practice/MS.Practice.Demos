using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProblem
{
    internal interface IChangeBoxedPoint
    {
        void Change(Int32 x, Int32 y);
    }
    internal struct Point : IChangeBoxedPoint
    {
        public Int32 m_x, m_y;

        public Point(Int32 x, Int32 y)
        {
            this.m_x = x;
            this.m_y = y;
        }
        public void Change(Int32 x, Int32 y)
        {
            m_x = x; m_y = y;
        }
        public override string ToString()
        {
            return string.Format("{0},{1}", m_x, m_y);
        }
    }

}
