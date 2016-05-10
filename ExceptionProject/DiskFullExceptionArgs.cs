using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExceptionProject
{
    internal class DiskFullExceptionArgs : ExceptionArgs
    {
        private readonly String m_diskpath; //在构造时设置私有字段

        public DiskFullExceptionArgs(String diskpath) { m_diskpath = diskpath; }
        
        //返回只读字段
        public String DiskPath { get { return m_diskpath; } }
        //重写Message属性来包含我们的字段（如果设置了的话）
        public override string Message
        {
            get
            {
                return (m_diskpath == null) ? base.Message : "DiskPath=" + m_diskpath;
            }
        }
    }
}
