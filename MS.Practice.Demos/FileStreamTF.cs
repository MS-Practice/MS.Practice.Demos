using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MS.Practice.Demos
{
    public class FileStreamTF
    {
    }
    public class UserInfo
    {
        private Int32 age = -1;
        private char level = 'A';
    }
    public class User
    {
        public Int32 id;
        public UserInfo user;
    }
    public class VIPUser : User
    {
        public bool isVip;
        public bool IsVipUser() {
            return isVip;
        }
    }
}
