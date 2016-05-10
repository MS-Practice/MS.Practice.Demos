using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace EnumType
{
    public class EnumFlagFileter
    {
        //判断是否隐藏
        public static void IsHidden()
        {
            String file = Assembly.GetEntryAssembly().Location;
            FileAttributes attributes = File.GetAttributes(file);
            Console.WriteLine("Is {0} hidden? {1}", file, (attributes & FileAttributes.Hidden) != 0);
        }
        public static void Go()
        {
            Actions actions = Actions.Read | Actions.Delete;
            Console.WriteLine(actions);
            Console.WriteLine(actions.ToString());
            Actions a = (Actions)Enum.Parse(typeof(Actions), "28", false);
            Console.WriteLine(a.ToString());

            FileAttributes fa = FileAttributes.System;
            fa = fa.Set(FileAttributes.ReadOnly);
            fa = fa.Clear(FileAttributes.System);
            fa.ForEach(f => Console.WriteLine(f));
        }
    }
    [Flags]
    internal enum Actions
    {
        None = 0,
        Read = 0x0001,
        Write = 0x0002,
        ReadWrite = Actions.Read | Actions.Write,
        Delete = 0x0004,
        Query = 0x0008,
        Sync = 0x0010
    }
    internal static class FileAttributesExtensionMethods
    {
        public static Boolean IsSet(this FileAttributes flags, FileAttributes flagToTest)
        {
            if (flagToTest == 0)
            {
                throw new ArgumentOutOfRangeException("flagToTest", "值不能为0");
            }
            return (flags & flagToTest) == flagToTest;
        }
        public static Boolean IsClear(this FileAttributes flags, FileAttributes flagToTest)
        {
            if (flagToTest == 0)
                throw new ArgumentOutOfRangeException("flagToTest", "值不能为0");
            return !IsSet(flags, flagToTest);
        }
        public static Boolean AnyFlagsSet(this FileAttributes flags, FileAttributes testFlags)
        {
            return ((flags & testFlags) != 0);
        }
        public static FileAttributes Set(this FileAttributes flags, FileAttributes setFlags) {
            return flags | setFlags;
        }
        public static FileAttributes Clear(this FileAttributes flags, FileAttributes clearFlags)
        {
            return flags & ~clearFlags;
        }
        public static void ForEach(this FileAttributes flags, Action<FileAttributes> processFlag) {
            if (processFlag == null)
                throw new ArgumentNullException("processFlag 不能为null");
            for (UInt32 bit = 1; bit != 0; bit <<= 1) {
                UInt32 temp = (UInt32)flags & bit;
                if (temp != 0) processFlag((FileAttributes)temp);
            }
        }
    }
}
