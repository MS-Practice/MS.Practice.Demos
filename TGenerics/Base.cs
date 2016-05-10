using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGenerics
{
    internal class Base : IDisposable
    {
        public void Dispose() {
            Console.WriteLine("Base's Dispose");
        }
    }
    internal class Derived : Base, IDisposable {
        new public void Dispose() {
            Console.WriteLine("Derived's Dispose");
            base.Dispose();
        }
    }
    internal sealed class SimpleType : IDisposable {
        public void Dispose() { Console.WriteLine("Dispose"); }
        void IDisposable.Dispose() { Console.WriteLine("IDisoposable Dispose"); }
    }
}
