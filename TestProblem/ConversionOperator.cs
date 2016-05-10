using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProblem
{
    internal sealed class ConversionOperator
    {
        public static void Go() {
            Rational r1 = 5;
            Rational r2 = 2.5f;
            Int32 x = (Int32)r1;
            Single s = (Single)r2;
        }
    }
    public sealed class Rational {
        public Rational(Int32 num) { }
        public Rational(Single num) { }
        public Int32 ToInt32() { return 0; }
        public Single ToSingle() { /* ... */ return 0f; }
        public static implicit operator Rational(Int32 num)
        {
            return new Rational(num);
        }
        public static implicit operator Rational(Single num)
        {
            return new Rational(num);
        }
        public static explicit operator Int32(Rational r)
        {
            return r.ToInt32();
        }
        public static explicit operator Single(Rational r)
        {
            return r.ToSingle() ;
        }
    }
}
