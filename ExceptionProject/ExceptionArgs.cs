using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExceptionProject
{
    [Serializable]
    public abstract class ExceptionArgs
    {
        public virtual String Message { get { return String.Empty; } }
    }
}
