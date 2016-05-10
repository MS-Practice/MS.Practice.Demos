using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgressBar.Commom
{
    public partial class Comment
    {
        public string Content { get; set; }

        public DateTime CreateTime { get; set; }
    }

    public partial class Comtent
    {
        private static List<Comment> s_comment = new List<Comment>
        {
            new Comment{
                CreateTime = DateTime.Parse("2015-09-15"),
                Content = "今天风和日丽！"
            },
            new Comment{
             CreateTime = DateTime.Parse("2015-09-16"), 
             Content = "今天阳光明媚！"
            },
            new Comment{
                CreateTime = DateTime.Parse("2015-09-17"), 
             Content = "今天小雨转晴！"
            }

        };

        public static List<Comment> GetComment(int pageSize, int pageIndex, out int totalCount) {
            totalCount = s_comment.Count;
            List<Comment> comments = new List<Comment>(pageSize);

            for (var i = pageSize * (pageIndex - 1); i < pageSize * pageIndex && i < s_comment.Count; i++) {
                comments.Add(s_comment[i]);
            }
            return comments;
        }

    }
}