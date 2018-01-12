using ExpresstionTreeDemo.CNBlog;
using ExpresstionTreeDemo.QueryableAbstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpresstionTreeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            PostService ps = new PostService();
            var str = ps.SearchPost(new SearchCriteria
            {
                End = DateTime.Now,
                MinComments = 10
            });
            Console.WriteLine(str);


            var posts = new Query<Post>().Where(p => p.Comments > 9).GetEnumerator();
            Console.WriteLine(JsonConvert.SerializeObject(posts));

            Console.ReadLine();
        }
    }
}
