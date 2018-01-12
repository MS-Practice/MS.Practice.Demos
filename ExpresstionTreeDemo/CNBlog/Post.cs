using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpresstionTreeDemo.CNBlog
{
    public class Post
    {
        // Id
        public int Id { get; set; }

        // 标题
        public string Title { get; set; }

        // 发布时间
        public DateTime Published { get; set; }

        // 推荐数据
        public int Diggs { get; set; }

        // 访问人数
        public int Views { get; set; }

        // 评论数据
        public int Comments { get; set; }

        // 作者
        public string Author { get; set; }

        // 博客链接
        public string Href { get; set; }

        // 摘要
        public string Summary { get; set; }
    }


    public class SearchCriteria
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public int MinDiggs { get; set; }
        public int MaxDiggs { get; set; }
        public int MinViews { get; set; }
        public int MaxViews { get; set; }
        public int MinComments { get; set; }
        public int MaxComments { get; set; }
    }
}
