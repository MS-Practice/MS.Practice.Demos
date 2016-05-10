using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace CharStringTextHandler
{
    internal class EncodingToUnicode
    {
        public static void ToUnicode()
        {
            String s = "Hi here";
            Encoding encodingUTF8 = Encoding.UTF8;
            Byte[] encodeBytes = encodingUTF8.GetBytes(s);
            Console.WriteLine("Encoding bytes:" + BitConverter.ToString(encodeBytes));

            String decodeString = encodingUTF8.GetString(encodeBytes);
            Console.WriteLine("Decoded string:" + decodeString);
        }
        public static void EncodingAllMethod() {
            foreach (EncodingInfo ei in Encoding.GetEncodings())
            {
                Encoding e = ei.GetEncoding();
                Console.WriteLine("{1} {0}" +
                     "\tCodePage={2}, WindowsCodePage={3}{0}" +
            "\tWebName={4}, HeaderName={5}, BodyName={6}{0}" +
            "\tIsBrowserDisplay={7}, IsBrowserSave={8}{0}" +
            "\tIsMailNewsDisplay={9}, IsMailNewsSave={10}{0}",
            Environment.NewLine,
            e.EncodingName, e.CodePage, e.WindowsCodePage, e.WebName, e.HeaderName, e.BodyName,
            e.IsBrowserDisplay, e.IsBrowserSave, e.IsMailNewsDisplay, e.IsMailNewsSave);
            }
        }
        public static void ToBase64CharArray() {
            Byte[] bytes = new Byte[10];
            new Random().NextBytes(bytes);
            //显示字节数
            Console.WriteLine(BitConverter.ToString(bytes));
            //将字节码解码成Base64字符串并显示字符串
            String s = Convert.ToBase64String(bytes);
            Console.WriteLine(s);
            //将Base-64字符串编码回字节，并显示字节
            bytes = Convert.FromBase64String(s);
            Console.WriteLine(BitConverter.ToString(bytes));
        }
        //public static void SecureString() {
        //    using (SecureStringClass ss = new SecureStringClass())
        //    {
        //        while (true)
        //        {
        //            ConsoleKeyInfo cki = Console.ReadKey(true);
        //            if (cki.Key == ConsoleKey.Enter) break;
        //            //将密码字符追加到SecureString中
        //            ss.AppendChar(cki.KeyChar);
        //            Console.Write("*");
        //        }
        //        Console.WriteLine();
        //        //密码已输入，出于演示的目的而显示他
        //        DisplaySecureString(ss);
        //    }
        //    //使用之后 SecureString是要被销毁的，内存中没有敏感数据
        //}        //以下代码不是安全的，因为要访问非托管代码
        private unsafe static void DisplaySecureString(SecureString ss)
        {
            Char* pc = null;
            try
            {
                //将SecureString解密到一个非托管内存缓冲区
                pc = (Char*)Marshal.SecureStringToCoTaskMemUnicode(ss);
                //访问包含已 解密SecureString的非托管内存缓冲区
                for (Int32 index = 0; pc[index] != 0; index++)
                {
                    Console.Write(pc[index]);
                }
            }
            finally
            {
                //确定清零并释放已解密的SecureString字符的非托管内存缓冲区
                if (pc != null)
                {
                    Marshal.ZeroFreeCoTaskMemUnicode((IntPtr)pc);
                }
            }
        }
    }
}
