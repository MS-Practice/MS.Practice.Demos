using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test._360doo.com
{
    public interface IInvokePAInterface<TPaInterface>
    {
        /// <summary>
        /// 调用平安接口
        /// </summary>
        TPaInterface CallPAInterfaceMethod(string url);
    }
}
