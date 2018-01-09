using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Diagnostics;

namespace LinqProgramDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Stopwatch sw = new Stopwatch();
            //for (int multiply = 0; multiply < 10; multiply++)
            //{
            //    DataTable dt = DBHelper.DBSqlHelper.GetTableSchema();
            //    for (int count = multiply * 10000; count < (multiply + 1) * 10000; count++)
            //    {
            //        DataRow r = dt.NewRow();
            //        r[0] = count;
            //        r[1] = string.Format("User-{0}", count * multiply);
            //        r[2] = string.Format("Pwd-{0}", count * multiply);
            //        dt.Rows.Add(r);
            //    }
            //    sw.Start();
            //    DBHelper.DBSqlHelper.InserDataByBulkToDB(dt);
            //    sw.Stop();
            //    Console.WriteLine(string.Format("Elapsed Time is {0} Milliseconds", sw.ElapsedMilliseconds));

            //    //int iteration = 10 * 1000;
            //    //string s = "";
            //    //CodeTimer.Time("String Contact", iteration, () => { s += "a"; });
            //    //StringBuilder sb = new StringBuilder();
            //    //CodeTimer.Time("StringBuilder", iteration, () => { sb.Append("a"); });
            //}
            //GetPrices();
            //dLinqDelegate();
            //ExpressionToLinq();
            ////AddLinqExpression();
            //byte a = (byte)130;
            //Console.WriteLine(a);

            Expression<Func<string, bool>> expr = name => name.Length > 10 && name.StartsWith("G");
            Console.WriteLine(expr);

            AndAlsoModifier treeModifier = new AndAlsoModifier();
            Expression modifierExpr = treeModifier.Modify(expr);

            Console.WriteLine(modifierExpr);

            string[] companies = {"Consolidated Messenger", "Alpine Ski House", "Southridge Video", "City Power & Light",
                    "Coho Winery", "Wide World Importers", "Graphic Design Institute", "Adventure Works",
                    "Humongous Insurance", "Woodgrove Bank", "Margie's Travel", "Northwind Traders",
                    "Blue Yonder Airlines", "Trey Research", "The Phone Company",
                    "Wingtip Toys", "Lucerne Publishing", "Fourth Coffee" };
            //转化IQueryable数据源
            IQueryable<string> queryableData = companies.AsQueryable();
            //编写表示谓词参数的表达式树
            ParameterExpression pe = Expression.Parameter(typeof(string), "company");
            //新建一个表达式树来表示 'company.ToLower() == "coho winery"' 的表达式
            Expression left = Expression.Call(pe, typeof(string).GetMethod("ToLower", Type.EmptyTypes));
            Expression right = Expression.Constant("coho winery", typeof(string));
            Expression e1 = Expression.Equal(left, right);
            //新建一个表达式树来表示 'company.Length > 16' 表达式
            left = Expression.Property(pe, typeof(string).GetProperty("Length"));
            right = Expression.Constant(16,typeof(int));
            Expression e2 = Expression.GreaterThan(left, right);
            //编译表达式树来生成一个表示'(company.ToLower() == "coho winery" || company.Length > 16)' 的表达式
            Expression predicateBody = Expression.OrElse(e1, e2);
            //新建一个表达式树来表示 'queryableData.Where(company => (company.ToLower() == "coho winery" || company.Length > 16))'
            MethodCallExpression whereCallExpresstion = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { queryableData.ElementType },
                queryableData.Expression,
                Expression.Lambda<Func<string, bool>>(predicateBody, new ParameterExpression[] { pe }));

            //排序 OrderBy(company => company)
            //新建一个表达式树来表示 'whereCallExpression.OrderBy(company => company)'
            MethodCallExpression orderCallExpresstion = Expression.Call(
                typeof(Queryable),
                "OrderBy",
                new Type[] { queryableData.ElementType, queryableData.ElementType },
                whereCallExpresstion,
                Expression.Lambda<Func<string, string>>(pe, new ParameterExpression[] { pe }));

            //新建一个可执行的查询表达式树
            IQueryable<string> result = queryableData.Provider.CreateQuery<string>(orderCallExpresstion);

            //枚举结果
            foreach (string company in companies)
                Console.WriteLine(company);


            Console.ReadLine();
        }
        public static void dLinqDelegate()
        {
            List<String> info = new List<string>() { "世界", "毛帅", "祝琴" };
            var inString = info.FindAll(delegate(string s)
            {
                return s.IndexOf("毛帅") >= 0;
            });
            var innewString = info.FindAll(p => p.IndexOf("毛帅") > -1);
            var q = (from o in info where o.Contains("毛帅") select o).ToArray();

            //        var fn = CompiledQuery.Compile(
            //(NorthwindDataContext db2, string name) =>
            //from c in db2.Person
            //where c.Name == name
            //select c);
            //        fn(db, "毛帅");
        }
        public static void ExpressionToLinq()
        {
            Expression<Func<int, bool>> filter = n => (n * 3) < 5;
            BinaryExpression lt = (BinaryExpression)filter.Body;    //(n * 3) < 5
            BinaryExpression mult = (BinaryExpression)lt.Left;  //n*3
            ParameterExpression en = (ParameterExpression)mult.Left;  //n
            ConstantExpression three = (ConstantExpression)mult.Right;  //3
            ConstantExpression five = (ConstantExpression)lt.Right;  //5
            var One = filter.Compile();
            Console.WriteLine("Result:{0},{1}", One(5), One(1));
            Console.WriteLine("({0}({1} {2} {3} {4}))", lt.NodeType, mult.NodeType, en.Name, three.Value, five.Value);
        }
        public static void AddLinqExpression()
        {
            NorthwindDataContext db = new NorthwindDataContext("");
            DataLoadOptions ds = new DataLoadOptions();
            var newPerson = new Person { ID = 1, Name = "Mason", Age = 24 };
            db.Person.InsertOnSubmit(newPerson);
            db.SubmitChanges();
            ds.LoadWith<Person>(p => p.Name);
        }
        private static void GetPrices()
        {
            BooksPrice bp = new BooksPrice
            {
                IBS = Guid.NewGuid().ToString(),
                Name = "C#高级编程",
                Price = 100.0
            };
            BooksPrice bp1 = new BooksPrice
            {
                IBS = Guid.NewGuid().ToString(),
                Name = "CLR via C#",
                Price = 95
            };
            BooksPrice bp2 = new BooksPrice
            {
                IBS = Guid.NewGuid().ToString(),
                Name = "你必须知道的.NET",
                Price = 79
            };
            BooksPrice bp3 = new BooksPrice
            {
                IBS = Guid.NewGuid().ToString(),
                Name = "MVC5.0高级编程",
                Price = 80
            };
            List<BooksPrice> list = new List<BooksPrice>{
                bp,bp1,bp2,bp3
            };
            //double Singleprice = list.OrderBy(item => item.IBS).Sum(item =>
            //{
            //    if (item.Name == "CLR via C#")
            //    {
            //        item.Price = 69;
            //    }
            //    return item.Price;
            //});
            list.OrderBy(item => item.IBS).Select(p =>p).Sum(p =>
            {
                if (p.Name == "CLR via C#")
                {
                    p.Price = 29.0;
                }
                return p.Price;
            });
        }
    }

    public class BooksPrice
    {
        public double Price { get; set; }
        public string Name { get; set; }
        public string IBS { get; set; }
    }

}
