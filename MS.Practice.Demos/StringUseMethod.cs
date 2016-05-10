using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MS.Practice.Demos
{
    public class StringUseMethod
    {
        public static void Execute(object obj)
        {
            MarshalByRefType marshalByRefobj = obj as MarshalByRefType;
            marshalByRefobj.ExecuteWithStringLocked();
        }
    }
    [Serializable]
    public class MarshalByRefType
    {
        #region 私有字段
        private string _stringLockHelper;
        private object _objectLockHelper;
        #endregion

        #region 共有属性
        public string StringLockHelper
        {
            get
            {
                return _stringLockHelper;
            }
            set { _stringLockHelper = value; }
        }
        public object ObjectLockHelper
        {
            get { return _objectLockHelper; }
            set { _objectLockHelper = value; }
        }
        #endregion

        #region 共有方法
        public void ExecuteWithStringLocked()
        {
            lock (this._stringLockHelper) {
                Console.WriteLine("The operation with a string locked is executed\n\tAppDomain:\t{0}\n\tTime:\t\t{1}",
                  AppDomain.CurrentDomain.FriendlyName, DateTime.Now);
                Thread.Sleep(10000);
            }
        }

        public void ExecuteWithObjectLocked()
        {
            lock (this._objectLockHelper)
            {
                Console.WriteLine("The operation with a object locked is executed\n\tAppDomain:\t{0}\n\tTime:\t\t{1}",
                   AppDomain.CurrentDomain.FriendlyName, DateTime.Now);
                Thread.Sleep(10000);
            }
        }
        #endregion
    }
}
