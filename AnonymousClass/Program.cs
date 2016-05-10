using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AnonymousClass
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var query = from num in numbers
                        select num * (from n in numbers
                                      where n % 2 == 0
                                      select n).Count();
            //LET关键字
            var query1 = from num in numbers
                         let evenNumbers = from n in numbers
                                           where n % 2 == 0
                                           select n
                         select num * evenNumbers.Count();

            String myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var obj = TimeSpan.FromDays(7);
            var query2 =
                from pathName in Directory.GetFiles(myDocuments)
                let LastTime = File.GetLastWriteTime(pathName)
                where LastTime > (DateTime.Now - TimeSpan.FromDays(7))
                orderby LastTime
                select new { Path = pathName, LastTime = LastTime };
            foreach (var file in query2)
            {
                Console.WriteLine("LastWriteTime={0},Path={1}", file.LastTime, file.Path);
            }
        }
    }
}
