using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test._360doo.com.ServiceReference1;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Test._360doo.com
{
    internal class Cart
    {
        public String ID { get; set; }
    }
    class Program
    {
        private static readonly string m_access_token = AccessToken.access_token;
        private static string interfaceULR = string.Empty;
        private static string resultJson = string.Empty;
        static IInvokePAInterface<GetInterfaceCityName> cmf = new CallMyInterface<GetInterfaceCityName>();
        static void Main(string[] args)
        {
            BetterPhone bp = new BetterPhone();
            bp.Dial();

#if DEFINE_CONTROL
            YieldDemo.Demo();
            CreateParentMenuByParentID("0");
            //GetInterfaceCityName gicn = cmf.CallPAInterfaceMethod("http://gw2demo.pahaoche.com/wghttp/gateway/carModels?token=" + m_access_token);http://gw2test.pahaoche.com/wghttp/gateway/ stores?token=39930e30-6f7b-4e3b-8b5f-f4316af95310
            //GetInterfaceCityName gicn = cmf.CallPAInterfaceMethod("http://gw2demo.pahaoche.com/wghttp/gateway/carModels?token=" + m_access_token);
            //Console.ReadKey();

            List<Cart> carts = new List<Cart>{
                  new Cart{ ID="1"},
                  new Cart{ ID="2"},
                  new Cart{ ID="3"},
                  new Cart{ ID="4"}
            };
            carts = carts.Where(p => (p.ID = "123").Length > -1).ToList();
            carts.ForEach((c) =>
            {
                Console.WriteLine(c.ID);
            });
            IFormatProvider iformat = new System.Globalization.CultureInfo("zh-CN", true);

            DateTime dtnow = DateTime.UtcNow.Date;
            string datetime = "19871212";
            //得出现在用户的生日与现在时间的时间间隔
            DateTime selectTime = DateTime.ParseExact(datetime, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            //最小值
            DateTime minTime = dtnow.AddYears(-27);
            //最大值
            DateTime maxTime = dtnow.AddYears(-45);
            //用户的生日早于最小时间并且晚于最大时间
            if (!(DateTime.Compare(selectTime, minTime) < 0) || !(DateTime.Compare(selectTime, maxTime) > 0))
            {
                //不符合条件
                Console.WriteLine("不符合条件 年龄大于45或者小于27");
            } 
#endif
            Console.ReadLine();
        }
        static void GetAnObject(out Object o) {
            o = new String('x', 100);
        }
        /// <summary>
        /// 获得门店详情
        /// </summary>
        private static void GetStoreList()
        {
            GetInterfaceCityName gicn = cmf.CallPAInterfaceMethod("http://gw2test.pahaoche.com/wghttp/gateway/ stores?token=" + m_access_token);
            int storeNumber = 0;
            StringBuilder storeMessage = new StringBuilder();
            if (gicn != null && gicn.StoreList != null)
            {
                //获得门店信息
                //获得门店数量
                storeNumber = gicn.StoreList.Count;
                //获得门店详细信息
                foreach (var store in gicn.StoreList)
                {
                    storeMessage.Append("门店信息：" + store.Title + " 门店地址：" + store.Content + Environment.NewLine);
                }
            }
        }
        /// <summary>
        /// 获得二手车报价
        /// </summary>
        private static decimal GetEstimatesUsedCarPrice(string trimId, string mileage)
        {
            decimal price = 0M;
            GetInterfaceCityName gicn = cmf.CallPAInterfaceMethod("http://gw2demo.pahaoche.com/wghttp/gateway/wapValuation?token=" + m_access_token + string.Format("&trimId={0}&mileage={1}&condition=良好", trimId, mileage));
            if (gicn != null && gicn.Data != null)
            {
                if (decimal.TryParse(gicn.Data.ExpPrice, out price)) return price;
            }
            return price;
        }
        /// <summary>
        /// 品牌接口
        /// </summary>
        private static void GetCarModels()
        {
            interfaceULR = "http://gw2demo.pahaoche.com/wghttp/gateway/carModels?token=" + m_access_token;
            resultJson = HttpRequestGet(interfaceULR);
        }
        /// <summary>
        /// 获取城市列表接口
        /// </summary>
        private static string GetCityNames()
        {
            interfaceULR = "http://gw2test.pahaoche.com/wghttp/gateway/chengshi?token=" + m_access_token;
            resultJson = HttpRequestGet(interfaceULR);
            return resultJson;
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
        private static readonly List<MenuMessage> m_menuList = new List<MenuMessage>{
            new MenuMessage(){ Id="1",Name="父级菜单01",ParentID="0"},
            new MenuMessage(){ Id="2",Name="子级菜单01",ParentID="1"},
            new MenuMessage(){ Id="3",Name="父级菜单02",ParentID="0"},
            new MenuMessage(){ Id="4",Name="子级菜单03",ParentID="3"},
            new MenuMessage(){ Id="5",Name="子级菜单04",ParentID="4"},
            new MenuMessage(){ Id="6",Name="父级菜单03",ParentID="0"},
            new MenuMessage(){ Id="7",Name="子级菜单02",ParentID="2"}
        };
        private static void CreateParentMenuByParentID(String ParentId)
        {
            //StringBuilder menuString = new StringBuilder();
            //查出所有父级菜单
            var info = m_menuList.Where(m => m.ParentID == ParentId);
            Console.WriteLine("构建的菜单:");
            //foreach (var item in info)
            //{
            //    Console.WriteLine("---{0}   {1}", item.Name, item.ParentID);
            //    CreatChildMenuByChildID(item.Id);
            //}
            Parallel.ForEach(m_menuList, item =>
            {
                Console.WriteLine("---{0}   {1}", item.Name, item.ParentID);
                CreatChildMenuByChildID(item.Id);
            });
        }
        private static void CreatChildMenuByChildID(String ChildID) {
            var lists = m_menuList.Where(m => m.ParentID == ChildID);
            foreach (var item in lists)
            {
                Console.WriteLine("------{0}    {1}", item.Name, item.ParentID);
                CreatChildMenuByChildID(item.Id);
            }
        }
    }
    internal sealed class MenuMessage
    {
        private String m_name;

        public String Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        private String m_id;

        public String Id
        {
            get { return m_id; }
            set { m_id = value; }
        }
        private String m_ParentID;


        public String ParentID
        {
            get { return m_ParentID; }
            set { m_ParentID = value; }
        }
    }
    internal static class YieldDemo {
        public static void Demo() {
            int i = 1;
            foreach (var fib in Fib())
            {
                Console.WriteLine(i + ": " + fib);
                if (i++ > 10)
                {
                    break;
                }
            }
        }
        private static IEnumerable<int> Fib()
        {
            int i = 1;
            int j = 1;
            yield return i;
            yield return j;
            while (true)
            {
                var k = i + j;
                yield return k;
                i = j;
                j = k;
            }
        }
    }

    internal class SomeType{
        public Int32 val;
    }

    class Phone {
        public void Dial() {
            Console.WriteLine("Phone.Dial");
            EstablishConnection();
        }

        protected virtual void EstablishConnection()
        {
            Console.WriteLine("Phone.EstablishConnection");
        }
    }
    class BetterPhone : Phone {
        new public void Dial()
        {
            Console.WriteLine("BetterPhone.Dial");
            EstablishConnection();
            base.Dial();
        }
        new protected virtual void EstablishConnection() {
            Console.WriteLine("BetterPhone.EstablishConnection");
        }
    }
}
