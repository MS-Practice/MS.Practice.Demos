using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace CharStringTextHandler
{
    public class SecureStringClass
    {
        public unsafe static void DisplaySecureString(SecureString ss)
        {
            Char* pc = null;
            try
            {
                //将SecureString解密到一个非托管内存缓存区中
                pc = (Char*)Marshal.SecureStringToCoTaskMemUnicode(ss);
                //访问包含已解密SecureString的非托管内存缓冲区
                for (Int32 index = 0; pc[index] != 0; index++)
                {
                    Console.WriteLine(pc[index]);
                }
            }
            finally { 
                //确定清零并释放包含已经解密SecureString字符的非托管内存缓冲区
                if (pc != null) {
                    Marshal.ZeroFreeCoTaskMemUnicode((IntPtr)pc);
                }
            }
        }
    }
}
