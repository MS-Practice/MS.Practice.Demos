using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB;
using MongoDB.Attributes;

namespace MongoDBForMS
{
    class Program
    {
        public sealed class Customer {
            [MongoId]
            public string CustomerID  { get; set; }
            public string customerName { get; set; }
            public string ContactName { get; set; }
            public string Address { get; set; }
            public string PostalCode { get; set; }
            public string Tel { get; set; }
        }
        /// <summary>
        /// MongoDB链接字符串
        /// </summary>
        private const string _connectionString = "Server=217.0.0.1";
        private static readonly string _dbName = "MyDataBase";
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="customer"></param>
        public static void Insert(Customer customer) {
            customer.CustomerID = Guid.NewGuid().ToString("N");
            //创建一个连接
            using (MongoDBHelper mongo = new MongoDBHelper())
            {
                mongo.GetCollection<Customer>().Insert(customer);
                ////打开连接
                //mongo.Connect();
                ////切换到指定数据库
                //var db = mongo.GetDatabase(_dbName);
                ////根据类型获取相应的集合
                //var collection = db.GetCollection<Customer>();
                ////向集合中插入对象
                //collection.Insert(customer);
            }
        }
        public static void Delete(string customerId) {
            using (MongoDBHelper mm = new MongoDBHelper()) {
                mm.GetCollection<Customer>().Remove(x => x.CustomerID == customerId);
            }
        }
        public static void Update(Customer customer) {
            using (MongoDBHelper mm = new MongoDBHelper()) {
                mm.GetCollection<Customer>().Update(customer, (x => x.CustomerID == customer.CustomerID));
            }
        }
        public static void GetbyId(string customerId) {
            using (MongoDBHelper mm = new MongoDBHelper()) {
                mm.GetCollection<Customer>().FindOne<Customer>(x => x.CustomerID == customerId);
            }
        }
        static void Main(string[] args)
        {
            //新增数据
            Customer customer = new Customer
            {
                ContactName = "Mason",
                customerName = "Jackey",
                PostalCode = "20100110111",
                Tel = "18975152023",
                Address = "白石洲"
            };
            Insert(customer);
        }
    }
}
