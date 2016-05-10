using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileLoader.InterfaceClass;

namespace FileLoader
{
    public class Tax : TTax
    {
        public decimal Calculate(Func<decimal> rateProvider, decimal value)
        {
            var rate = rateProvider.Invoke();
            return rate * value;
        }
    }
}
