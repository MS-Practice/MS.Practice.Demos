using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

/// <summary>
///Class1 的摘要说明
/// </summary>
public class Class1
{
	public Class1()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public string GetPath(string virtualPath)
    {
        return System.Web.Hosting.HostingEnvironment.MapPath(virtualPath);
    }
}