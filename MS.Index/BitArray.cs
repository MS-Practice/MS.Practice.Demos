using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MS.Index
{
    internal class BitArray
    {
        //一个用于保存位的私有数组
        private byte[] byteArray;
        private int numBits;

        //下面的构造器用来分配字节数组，并将所有的位设为0
        public BitArray(int numBits) { 
            //检测参数的有效性
            if (numBits <= 0)
                throw new ArgumentOutOfRangeException("numBits", "numBits must be > 0");
            //保存位的个数
            this.numBits = numBits;
            //为位数组分配字节
            byteArray = new byte[(numBits + 7) / 8];
        }

        //下面一个索引器
        public bool this[int bitPos] { 
            //get访问方式
            get { 
                //检测参数的有效性
                if (bitPos < 0 || bitPos >= numBits)
                    throw new ArgumentOutOfRangeException();
                //返回指定索引器上的位的状态
                return ((byteArray[bitPos / 8]) & (1 << (bitPos % 8))) != 0;
            }
            set {
                if (bitPos < 0 || bitPos >= numBits)
                    throw new ArgumentOutOfRangeException();
                if (value) { 
                    //将指定索引上的位数设为真值
                    byteArray[bitPos / 8] = (byte)(byteArray[bitPos / 8] | (1 << (bitPos % 8)));
                }
            }
        }
    }
}
