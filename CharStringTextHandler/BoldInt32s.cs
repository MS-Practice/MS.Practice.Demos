using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CharStringTextHandler
{
    internal class BoldInt32s : IFormatProvider, ICustomFormatter
    {
        public Object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter)) return this;
            return Thread.CurrentThread.CurrentCulture.GetFormat(formatType);
        }
        public String Format(String format, Object arg, IFormatProvider formatProvider) {
            String s;
            IFormattable formattable = arg as IFormattable;
            if (formattable == null) s = arg.ToString();
            else s = formattable.ToString(format, formatProvider);
            if (arg.GetType() == typeof(Int32))
                return "<B>" + s + "</B>";
            return s;
        }
    }
}
