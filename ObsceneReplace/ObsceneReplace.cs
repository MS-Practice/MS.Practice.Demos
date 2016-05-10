using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ObsceneReplace
{
    /// <summary>
    /// 算法的关键，还是使用空间来换时间，使用了2个全局的BitArray, 长度均为Char.MaxValue。其中一个BitArray用来判断是否有某个char开头的脏字，另一个BitArray用来判断所有脏字中是否包含某个char。经过这两个BitArray，可以做出快速判断，之后就使用Hash Code来判断完整的脏字，通过预先获取的最大脏字长度优化遍历过程。
    /// </summary>
    public class ObsceneReplace
    {
        private Dictionary<string, object> hash = new Dictionary<string, object>();
        private BitArray firstCharCheck = new BitArray(Char.MaxValue);
        private BitArray allCharCheck = new BitArray(Char.MaxValue);
        private int maxLength = 0;
        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData() {
            string[] badwords = { "草", "日", "狗日" }; //初始化脏字 也可以用后台读取
            foreach (string word in badwords) {
                if (!hash.ContainsKey(word)) {
                    hash.Add(word,null);
                    maxLength = Math.Max(maxLength, word.Length);
                    firstCharCheck[word[0]] = true;

                    foreach (char c in word) {
                        allCharCheck[c] = true;
                    }
                }
            }
        }
        /// <summary>
        /// 判断脏字是否出现在一个字符串中的代码：
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool IsBadwords(string target) {
            int index = 0;
            int offset = 0;
            while (index < target.Length) {
                //判断目标字符串中的那些字含有脏字
                if (!firstCharCheck[target[index]])
                {
                    while (index < target.Length - 1 && !firstCharCheck[target[++index]]) ;
                }
                for (int j = 1; j <= Math.Min(maxLength, target.Length - index); j++)
                {
                    if (!allCharCheck[target[index + j - 1]])
                    {
                        break;
                    }
                    string sub = target.Substring(index, j);
                    if (hash.ContainsKey(sub)) {
                        return true;
                    }
                }
                index++;
            }
            return false;
        }


    }
}
