using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExpresstionTreeDemo.CNBlog
{
    public class PostService
    {
        private static DateTime _lastModified = DateTime.Now;
        private static volatile object _obj = new object();
        private static readonly string _serviceUrl;
        private static List<Post> m_list;

        static PostService()
        {
            _serviceUrl = "http://wcf.open.cnblogs.com/blog/sitehome/recent/100000";
            // 初始加载
            LoadPostFromCnblogs();
        }

        private static void LoadPostFromCnblogs()
        {
            lock (_obj)
            {
                m_list = new List<Post>();
                var document = XDocument.Load(_serviceUrl);

                var elements = document.Root.Elements();
                var result = from entry in elements
                             where entry.HasElements == true
                             select new Post
                             {
                                 Id = Convert.ToInt32(entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "id").Value),

                                 Title = entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "title").Value,

                                 Published = Convert.ToDateTime(entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "published").Value),

                                 Diggs = Convert.ToInt32(entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "diggs").Value),

                                 Views = Convert.ToInt32(entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "views").Value),

                                 Comments = Convert.ToInt32(entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "comments").Value),

                                 Summary = entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "summary").Value,

                                 Href = entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "link")
                                     .Attribute("href").Value,

                                 Author = entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "author")
                                     .Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "name").Value
                             };

                m_list.AddRange(result);
                _lastModified = DateTime.Now;
            }
        }

        public static IEnumerable<Post> Posts
        {
            get
            {
                // 一分钟这后再次去博客园取最新的数据
                if (DateTime.Now.AddSeconds(60) > _lastModified)
                {
                    LoadPostFromCnblogs();
                }
                return m_list;
            }
        }

        public string SearchPost(SearchCriteria criteria = null)
        {
            var result = PostService.Posts;
            if (criteria != null)
            {
                if (!string.IsNullOrEmpty(criteria.Title))
                    result = result.Where(
                        p => p.Title.IndexOf(criteria.Title, StringComparison.OrdinalIgnoreCase) >= 0);

                if (!string.IsNullOrEmpty(criteria.Author))
                    result = result.Where(p => p.Author.IndexOf(criteria.Author, StringComparison.OrdinalIgnoreCase) >= 0);

                if (criteria.Start.HasValue)
                    result = result.Where(p => p.Published >= criteria.Start.Value);

                if (criteria.End.HasValue)
                    result = result.Where(p => p.Published <= criteria.End.Value);

                if (criteria.MinComments > 0)
                    result = result.Where(p => p.Comments >= criteria.MinComments);

                if (criteria.MinDiggs > 0)
                    result = result.Where(p => p.Diggs >= criteria.MinDiggs);

                if (criteria.MinViews > 0)
                    result = result.Where(p => p.Diggs >= criteria.MinViews);

                if (criteria.MaxComments > 0)
                    result = result.Where(p => p.Comments <= criteria.MaxComments);

                if (criteria.MaxDiggs > 0)
                    result = result.Where(p => p.Diggs <= criteria.MaxDiggs);

                if (criteria.MaxViews > 0)
                    result = result.Where(p => p.Diggs <= criteria.MaxViews);
            }
            return JsonConvert.SerializeObject(result);
        }

        private void GetContentFromXml()
        {
            var document = XDocument.Load("http://wcf.open.cnblogs.com/blog/sitehome/recent/100000");
            var elements = document.Root.Elements();

            var result = from entry in elements
                         where entry.HasElements == true
                         select new Post
                         {
                             Id = Convert.ToInt32(entry.Elements()
                        .SingleOrDefault(x => x.Name.LocalName == "id").Value),

                             Title = entry.Elements()
                        .SingleOrDefault(x => x.Name.LocalName == "title").Value,

                             Published = Convert.ToDateTime(entry.Elements()
                        .SingleOrDefault(x => x.Name.LocalName == "published").Value),

                             Diggs = Convert.ToInt32(entry.Elements()
                        .SingleOrDefault(x => x.Name.LocalName == "diggs").Value),

                             Views = Convert.ToInt32(entry.Elements()
                        .SingleOrDefault(x => x.Name.LocalName == "views").Value),

                             Comments = Convert.ToInt32(entry.Elements()
                        .SingleOrDefault(x => x.Name.LocalName == "comments").Value),

                             Summary = entry.Elements()
                        .SingleOrDefault(x => x.Name.LocalName == "summary").Value,

                             Href = entry.Elements()
                        .SingleOrDefault(x => x.Name.LocalName == "link")
                        .Attribute("href").Value,

                             Author = entry.Elements()
                        .SingleOrDefault(x => x.Name.LocalName == "author")
                        .Elements()
                        .SingleOrDefault(x => x.Name.LocalName == "name").Value
                         };

            m_list.AddRange(result);
        }
    }
}
