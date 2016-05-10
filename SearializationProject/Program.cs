using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Data;
using System.Security.Permissions;
using System.Runtime.Serialization.Formatters.Soap;
using System.Reflection;

namespace SearializationProject
{
    class Program
    {
        static void Main(string[] args)
        {
            FieldInfo fi = typeof(DateTime).GetField("dateData", BindingFlags.NonPublic | BindingFlags.Instance);
            String name = fi.GetValue(DateTime.Now).ToString();
            //DataTable dt = new DataTable();
            ////创建一个对象图，以便序列化到流中
            //var objectGraph = new List<String> { "Marson", "Christina", "Micrady" };
            //Stream stream = SerializeToMemory(objectGraph);
            //stream.Position = 0;
            //objectGraph = null;
            ////范序列化对象，证明他/她能工作
            //objectGraph = (List<String>)DeserializeFromMemory(stream);

            Circle cc = new Circle(10);
            Stream stream = SerializeToMemory(cc);
            stream.Position = 0;
            cc = null;
            //范序列化对象，证明他/她能工作
            cc = (Circle)DeserializeFromMemory(stream);
        }

        private static Object DeserializeFromMemory(Stream stream)
        {
            //构造一个序列化格式化器来做所有的工作
            BinaryFormatter formatter = new BinaryFormatter();
            //告诉格式化器从流中范序列化对象
            return formatter.Deserialize(stream);
        }

        private static Stream SerializeToMemory<T>(T objectGraph)
        {
            //构造一个流来容纳序列化的对象
            MemoryStream stream = new MemoryStream();
            //构造一个序列化格式化器，它负责所有的工作
            BinaryFormatter formatter = new BinaryFormatter();
            //告诉格式化器，讲对象序列化到一个流中
            formatter.Serialize(stream, objectGraph);
            //将序列化好的对象返回给调用者
            return stream;
        }

        private static Object DeepClone(Object original)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Context = new StreamingContext(StreamingContextStates.Clone);
                formatter.Serialize(stream, original);
                stream.Position = 0;
                return formatter.Deserialize(stream);
            }
        }

        private static void SingletonSerializationTest()
        {
            //创建一个数组，其中多个元素引用到Singleton对象中
            Singleton[] al = { Singleton.GetSingleton(), Singleton.GetSingleton() };
            Console.WriteLine("Do both elements refer to the same object? " + (al[0] == al[1]));    //“True”

            using (var stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, al);
                stream.Position = 0;
                Singleton[] a2 = (Singleton[])formatter.Deserialize(stream);
                //证明它的工作与预期一样
                Console.WriteLine("Do both elements refer to the same object? " + (a2[0] == a2[1]));    //True
                Console.WriteLine("Do all elements refer to the same object? " + (al[0] == a2[0]));    //True

            }
        }
    }
    [Serializable]
    internal class Circle
    {
        private Double m_radius;
        [NonSerialized]
        private Double m_area;
        public Circle(Double radius)
        {
            m_radius = radius;
            m_area = Math.PI * m_radius * m_radius;
        }
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            m_area = Math.PI * m_radius * m_radius;
        }
    }
    //每个AppDomain应该只有这一个类型的实例
    [Serializable]
    internal sealed class Singleton : ISerializable
    {
        //这是该类型的一个实例
        private static readonly Singleton s_theOneObject = new Singleton();
        //这些是实例字段
        public String Name = "Marson";
        public DateTime Date = DateTime.Now;
        //私有构造  允许这个类型构造单实例
        private Singleton() { }
        //该方法返回对单实例的引用
        public static Singleton GetSingleton() { return s_theOneObject; }
        //序列化一个Singleton时，所调用的方法
        //我建议这里显式调用接口方法实现（EIMI）
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.SetType(typeof(SingletonSerializationHelper));
            //不需要添加其他值
        }
    }
    [Serializable]
    internal sealed class SingletonSerializationHelper : IObjectReference
    {
        //这个方法在对像（没有字段）反序列化调用
        public Object GetRealObject(StreamingContext context)
        {
            return Singleton.GetSingleton();
        }
    }

    internal sealed class UniversalToLocalTimeSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            //将DateTime从本地时间转化为UTC
            info.AddValue("Date", ((DateTime)obj).ToUniversalTime().ToString("u"));
        }
        public Object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            //将Datetime从UTC转回本地时间
            return DateTime.ParseExact(info.GetString("Date"), "u", null).ToLocalTime();
        }
        private static void SerializationSurrogateDemo()
        {
            using (var stream = new MemoryStream())
            {
                //构造所需的格式化器
                IFormatter formatter = new SoapFormatter();
                //构造一个SurrogateSelector(代理选择器)对象
                SurrogateSelector ss = new SurrogateSelector();
                //告诉代理选择器为DateTime对象使用我们的代理
                ISerializationSurrogate utcToLocalTimeSurrogate = new UniversalToLocalTimeSerializationSurrogate();
#if GetSurrogateForCyclicalReference
         utcToLocalTimeSurrogate = FormatterServices.GetSurrogateForCyclicalReference(utcToLocalTimeSurrogate); 
#endif
                ss.AddSurrogate(typeof(DateTime), formatter.Context, utcToLocalTimeSurrogate);
                //注意：AddSurrogate可多次调用来登记多个代理
                //告诉格式化器使用代理选择器
                formatter.SurrogateSelector = ss;
                //创建一个DateTimel来代表机器上的本地时间，并序列化它
                DateTime localTimeBeforeSerialize = DateTime.Now;
                formatter.Serialize(stream, localTimeBeforeSerialize);
                //stream讲universal时间作为一个字符串显示，证明能工作
                stream.Position = 0;
                Console.WriteLine(new StreamReader(stream).ReadToEnd());
                //反序列化universal时间字符串，并且把它转换本地的DateTime
                DateTime localTimeAfterDeserialize = (DateTime)formatter.Deserialize(stream);
                //证明能正常工作
                Console.WriteLine("LocalTimeBeforeSerialize={0}", localTimeBeforeSerialize);
                Console.WriteLine("LocalTimeAfterSerialize={0}", localTimeAfterDeserialize);
            }
        }
    }

    internal sealed class UniversalToLocalTimeSerializationSurrogate1 : ISerializationSurrogate
    {
        public void GetObjectData(Object obj, SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Date", ((DateTime)obj).ToUniversalTime().ToString("u"));
        }
        public Object SetObjectData(Object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            DateTime dt = DateTime.ParseExact(info.GetString("Date"), "u", null).ToLocalTime();
#if GetSurrogateForCyclicalReference
            //使用GetSurrogateForCyclicalReference时,你必须直接修改的obj,返回null或obj
            //所以,我修改传递给SetObjectData的一个经过装箱的DateTime对象
            FieldInfo fi = typeof(DateTime).GetField("dateData", BindingFlags.NonPublic | BindingFlags.Instance);
            fi.SetValue(obj, fi.GetValue(dt));
            return null;
#else
            return dt;
#endif

        }
    }
}
