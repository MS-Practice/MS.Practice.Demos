using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterfaceDemo
{
    internal class SomeValueType1 : IComparable
    {
        private Int32 m_x;
        public SomeValueType1(Int32 x) { m_x = x; }
        public Int32 CompareTo(Object other)
        {
            return (m_x - ((SomeValueType1)other).m_x);
        }
    }
    internal struct SomeValueType : IComparable
    {
        private Int32 m_x;
        public SomeValueType(Int32 x) { m_x = x; }
        public Int32 CompareTo(SomeValueType other)
        {
            return (m_x - other.m_x);
        }
        Int32 IComparable.CompareTo(Object other)
        {
            return CompareTo((SomeValueType)other);
        }
    }
}
