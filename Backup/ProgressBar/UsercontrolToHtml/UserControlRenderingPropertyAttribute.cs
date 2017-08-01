using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProgressBar.Commom.Enum;

namespace ProgressBar.UsercontrolToHtml
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class UserControlRenderingPropertyAttribute : Attribute
    {
        public string Key { get; set; }
        public UserControlRenderingPropertySource Source { get; set; }
    }
}