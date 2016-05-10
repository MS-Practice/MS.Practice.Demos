using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace Test._360doo.com
{
    internal class CallMyInterface<TCityNameList> : IInvokePAInterface<TCityNameList>
    {
        public TCityNameList CallPAInterfaceMethod(string url)
        {
            string resultJson = HttpRequestGet(url);
            TCityNameList tclass = JsonConvert.DeserializeObject<TCityNameList>(resultJson);
            return tclass;
        }
        private static string HttpRequestGet(string sendUrl, string method = "POST")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sendUrl);
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
        public static string PostDataToUrl(String data, String url, String encode)
        {
            Encoding ecode = Encoding.GetEncoding(encode);
            Byte[] datas = ecode.GetBytes(data);
            return PostDataToUrl(datas, url);
        }
        private static string PostDataToUrl(Byte[] data, String url)
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //设置头部报文
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.Method = "POST";
            //填充数据
            myHttpWebRequest.ContentLength = data.Length;
            Stream requestStream = myHttpWebRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            //发送
            Stream responseStream;
            try
            {
                responseStream = myHttpWebRequest.GetResponse().GetResponseStream();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //读取服务器返回信息
            string responseString = string.Empty;
            using (StreamReader streamReader = new StreamReader(responseStream,Encoding.UTF8))
            {
                responseString = streamReader.ReadToEnd();
            }
            responseStream.Close();
            return responseString;
        }
    }
}
