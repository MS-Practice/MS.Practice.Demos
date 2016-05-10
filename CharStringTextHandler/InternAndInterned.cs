using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.ConstrainedExecution;
using System.Runtime.CompilerServices;

namespace CharStringTextHandler
{
    public class InternAndInterned
    {
        public static void InternString() {
            //string s1 = "Hello";
            //string s2 = string.Intern(s1);
            //Console.WriteLine(object.ReferenceEquals(s1, s2));
            //ThreeOperation();
            //Console.ReadKey();
        }

        public static Int32 NumTimesWordAppearsEquals(String word, String[] wordList)
        {
            Int32 count = 0;
            for (Int32 wordnum = 0; wordnum < wordList.Length; wordnum++)
            {
                if (word.Equals(wordList[wordnum], StringComparison.Ordinal))
                    count++;
            }
            return count;

        }
        public static Int32 NumTimesWordAppearIntern(String word, String[] wordList)
        { 
            //这个方法假定wordList中的所有数组元素引用已留用的字符串
            word = String.Intern(word);
            Int32 count = 0;
            for (Int32 wordnum = 0; wordnum < wordList.Length; wordnum++)
            {
                if (Object.ReferenceEquals(word, wordList[wordnum]))
                    count++;
            }
            return count;
        }

        public static void ThreeOperation() {
            List<String> list = GetList();
            int start = Environment.TickCount;
            for (var i = 0; i < list.Count; i++) {
                if (list[i] == null)
                {
                    list[i] = "ss";
                }
                else {
                    list[i] = list[i];
                }
            }
            int end = Environment.TickCount;
            Console.WriteLine("传统写法：" + (end - start) + "毫秒");
            int start1 = Environment.TickCount;
            for (var i = 0; i < list.Count; i++) {
                list[i] = list[i] == null ? "ss" : list[i];
            }
            int end1 = Environment.TickCount;
            Console.WriteLine("简写：" + (end - start)+"毫秒");
        }
        public static List<String> GetList() {
            List<string> stringList = new List<string>();
            int num = 30000000;
            Random rd = new Random();
            int number;  
            for (int i = 0; i < num; i++)
            {
                number = rd.Next(0, 100);
                stringList.Add("ss" + num);
            }
            return stringList;
        }
    }
}
