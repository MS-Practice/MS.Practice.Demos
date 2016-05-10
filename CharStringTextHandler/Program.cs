using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Security;
using System.Runtime.InteropServices;

namespace CharStringTextHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            //IFormatProvider ifp = null;
            //ICustomFormatter cf = null;
            //cf = ifp.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter;
            //Double d;
            //d = char.GetNumericValue('\u0033'); //'\u0033'为数字3
            //Console.WriteLine(d.ToString());
            //d = char.GetNumericValue('\u00bc'); //'\u00bc'是普通分数四分之一'1/4'
            //Console.WriteLine(d.ToString());
            //d = char.GetNumericValue('A');
            //Console.WriteLine(d.ToString());
            //OneTimeCompare();
            //ComparingStringsForSorting();

            //InternAndInterned.InternString();
            //String s = "a\u0304\u0308bc\u0327";
            //StringInfoStaticClass.SubstringByTextElements(s);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(new BoldInt32s(), "{0} {1} {2:M}", "Marson", 123, DateTime.Now);
            Console.WriteLine(sb);

#if ENCODING
            EncodingToUnicode.ToUnicode();
            EncodingToUnicode.EncodingAllMethod();
            EncodingToUnicode.ToBase64CharArray();

            using (SecureString ss = new SecureString())
            {
                Console.Write("请输入你的密码：");
                while (true)
                {
                    ConsoleKeyInfo cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Enter) break;
                    //将密码字符追加到SecureString中
                    ss.AppendChar(cki.KeyChar);
                    Console.Write("*");
                }
                Console.WriteLine();
                //密码已输入，出于演示的目的而显示它
                DisplaySecureString(ss);
            } 
#endif
            Console.ReadKey();
        }

        //以下代码不是安全的，因为要访问非托管代码
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

        static void OneTimeCompare() {
            string s1 = "Strasse";
            string s2 = "Straße";
            Boolean eq;
            //Compare返回非0值
            eq = String.Compare(s1, s2, StringComparison.Ordinal) == 0;
            Console.WriteLine("Ordinal comparison: '{0}' {2} '{1}'", s1, s2, eq ? "==" : "!=");
            //正在向德国（DE）说德语（de）的人群
            //正确比较字符串
            CultureInfo ci = new CultureInfo("de-DE");
            //Compare 返回零值
            eq = string.Compare(s1, s2, true, ci) == 0;
            Console.WriteLine("CultureInfo comparison: '{0}'  {2}  '{1}'", s1, s2, eq ? "==" : "!=");
        }
        static void ComparingStringsForSorting() {
            string output = string.Empty;
            string[] symbool = new string[] { "<", "=", ">" };
            Int32 x;
            CultureInfo ci;
            //一下代码演示不同的语言文化中字符串比较方式也不相同
            String s1 = "coté";
            String s2 = "côte";
            //为法国法语排序字符串
            ci = new CultureInfo("fr-FR");
            x = Math.Sign(ci.CompareInfo.Compare(s1, s2));
            output += string.Format("{0} Compare: {1} {3} {2}", ci.Name, s1, s2, symbool[x + 1]);
            output += Environment.NewLine;
            //为日本日语排序字符串
            ci = new CultureInfo("ja-JP");
            x = Math.Sign(ci.CompareInfo.Compare(s1, s2));
            output += string.Format("{0} Compare: {1} {3} {2}", ci.Name, s1, s2, symbool[x + 1]);
            output += Environment.NewLine;
            //为当前线程的当前语言文化字符串排序
            ci = Thread.CurrentThread.CurrentCulture;
            x = Math.Sign(ci.CompareInfo.Compare(s1, s2));
            output += string.Format("{0} Compare: {1} {3} {2}", ci.Name, s1, s2, symbool[x + 1]);
            output += Environment.NewLine + Environment.NewLine;

            //以下代码演示如何将CompareInfo.Compare的高级选项应用于两个日语字符串
            //一个字符串代表用平假名写成的单词"shinkansen"(新干线，日本)
            //高速铁路名；另一个字符串代表用片假名写成的同一个单词
            s1 = "しんかんせん";// ("\u3057\u3093\u304B\u3093\u305b\u3093")
            s2 = "シンカンセン";// ("\u30b7\u30f3\u30ab\u30f3\u30bb\u30f3")
            ci = new CultureInfo("ja-JP");
            x = Math.Sign(String.Compare(s1, s2, true, ci));
            output += String.Format("Simple {0} Compare: {1} {3} {2}",
               ci.Name, s1, s2, symbool[x + 1]);
            output += Environment.NewLine;
            //以下是忽略日语假名的比较结果
            CompareInfo compareInfo = CompareInfo.GetCompareInfo("ja-JP");
            x = Math.Sign(compareInfo.Compare(s1, s2, CompareOptions.IgnoreKanaType));
            output += String.Format("Simple {0} Compare: {1} {3} {2}",
               ci.Name, s1, s2, symbool[x + 1]);
            MessageBox.Show(output, "Comparing Strings For Sorting");

            string FolderName="C:";
            while (true)
            {
                d.BeginInvoke(FolderName, ShowFolderSize, FolderName);
                
            }
        }
        public static void ShowFolderSize(IAsyncResult result)
        {
            Int64 size = d.EndInvoke(result);
            Console.WriteLine("\n文件夹{0}的容量为：{1}字节\n", (String)result.AsyncState, size);
        }
        private static Int64 CalculateFolderSize(string FolderName)
        {
            if (Directory.Exists(FolderName) == false)
            {
                throw new DirectoryNotFoundException("文件不存在");
            }
            DirectoryInfo rootDir = new DirectoryInfo(FolderName);
            //Get all subfolders
            DirectoryInfo[] childDirs = rootDir.GetDirectories();
            //Get all files of current folder
            FileInfo[] files = rootDir.GetFiles();
            Int64 totalSize = 0;
            //sum every file size
            foreach (FileInfo file in files)
            {
                totalSize += file.Length;

            }
            //sum every folder
            foreach (DirectoryInfo dir in childDirs)
            {
                totalSize += CalculateFolderSize(dir.FullName);
            }
            return totalSize;

        }
        public delegate Int64 CalculateFolderSizeDelegate(String folderName);
        private static CalculateFolderSizeDelegate d = CalculateFolderSize;

    }
}
