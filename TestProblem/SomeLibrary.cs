using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: CLSCompliant(true)]
namespace TestProblem
{
    public class SomeLibrary
    {
        public sealed class SomeLibraryType
        {
            //public UInt32 Abc() { return 0; }
            //public void abc() { }
            private UInt32 ABC() { return 0; }
        }
    }
}
