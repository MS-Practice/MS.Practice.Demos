using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace FileLoader
{
    /// <summary>
    /// 实体转Xml，Xml转实体类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XmlHelper<T> where T : new()
    {
        #region 实体转XML字符串
        /// <summary>
        /// 对象实例转成xml
        /// </summary>
        /// <param name="item">对象示例</param>
        /// <returns></returns>
        public static string EntityToXml(T item)
        {
            IList<T> items = new List<T>();
            items.Add(item);
            return EntityToXml(items);
        }
        /// <summary>
        /// 对象实例集转成xml
        /// </summary>
        /// <param name="items">对象实例集</param>
        /// <returns></returns>
        private static string EntityToXml(IList<T> items)
        {
            //创建XmlDocument文档
            XmlDocument doc = new XmlDocument();
            //创建Xml跟节点
            XmlElement root = doc.CreateElement(typeof(T).Name + "root");
            //添加跟元素的子节点
            foreach (var item in items)
            {
                EntityToXml(doc, root, item);
            }
            //向XmlDocument文档添加根节点
            doc.AppendChild(root);
            return doc.InnerXml;
        }
        /// <summary>
        /// 目标文档在根节点下生成Xml节点
        /// </summary>
        /// <param name="doc">目标文档</param>
        /// <param name="root">目标文档的根节点</param>
        /// <param name="item">节点信息</param>
        private static void EntityToXml(XmlDocument doc, XmlElement root, T item)
        {
            //创建元素
            XmlElement xmlItem = doc.CreateElement(typeof(T).Name);
            //对象的属性值
            System.Reflection.PropertyInfo[] propertyInfos = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var property in propertyInfos)
            {
                if (property != null)
                {
                    //对象属性的名称
                    string name = property.Name;
                    //对象属性值
                    string value = string.Empty;
                    if (property.GetValue(item, null) != null)
                        value = property.GetValue(item, null).ToString();
                    xmlItem.SetAttribute(name, value);
                }
            }
            //向根节点添加节点
            root.AppendChild(xmlItem);
        } 
        #endregion

        /// <summary>
        /// 序列化对象为XML字符串
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serializer(Type type, T obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string ret = sr.ReadToEnd();
            return ret;
        }
    }
}
