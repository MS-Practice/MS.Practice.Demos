using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CommomCore
{
    public class LoadResource
    {
        /// <summary>
        /// 获取目标文件的绝对路径
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string GetResourceToStyle(string relativePath)
        {
            string returl = string.Empty;
            if (!string.IsNullOrWhiteSpace(relativePath))
                returl = HttpContext.Current.Server.MapPath(relativePath);
            return returl;
        }
        public static string GetResources(string path) {
            string returl = string.Empty;
            if (!string.IsNullOrWhiteSpace(path))
                returl = path + "?t=" + DateTime.Now.ToString("yyyyMMddssssss");
            return returl;
        }
    }
}
