using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressionProgram
{
    public class SquaresOfPositive
    {
        public List<int> GetSquaresOfPositive(List<string> strList)
        {
            List<int> intList = new List<int>();
            foreach (var s in strList) intList.Add(Int32.Parse(s));
            List<int> evenList = new List<int>();
            foreach (int i in intList)
            {
                if (i % 2 == 0) evenList.Add(i);
            }
            List<int> squareList = new List<int>();
            foreach (int i in evenList) squareList.Add(i * i);
            squareList.Sort();
            return squareList;
        }

        public List<int> GetSquaresOfPositiveByLambda(List<string> strList)
        {
            return strList
                .Select(s => Int32.Parse(s))
                .Where(i => i % 2 == 0)
                .Select(i => i * i)
                .OrderBy(i => i)
                .ToList();
        }

        public Dictionary<char, List<string>> GetIndex(IEnumerable<string> keywords)
        {
            // 定义字典
            var result = new Dictionary<char, List<string>>();
            // 填充字典
            foreach (var kw in keywords)
            {
                var firstChar = kw[0];
                List<string> groupKeywords;

                if (!result.TryGetValue(firstChar, out groupKeywords))
                {
                    groupKeywords = new List<string>();
                    result.Add(firstChar, groupKeywords);
                }

                groupKeywords.Add(kw);
            }
            // 为每个分组排序
            foreach (var groupKeywords in result.Values)
            {
                groupKeywords.Sort();
            }
            return result;
        }
        public Dictionary<char, List<string>> GetIndexByLambda(IEnumerable<string> keywords)
        {
            return keywords.GroupBy(k => k[0])   //按照首字母分组
                .ToDictionary(      //构造字典
                    g => g.Key,   //以每组的key作为键
                    g => g.OrderBy(k => k).ToList());   //对每组排序生成列表
        }
    }
}
