using ExpresstionTreeDemo.CNBlog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpresstionTreeDemo.QueryableAbstract
{
    class CnblogsQueryProvider : QueryProvider
    {
        public override object Execute(Expression expression)
        {
            string url = GetQueryText(expression);
            IEnumerable<Post> results = PostHelper.PerformWebQuery(url);
            return results;
        }

        public override string GetQueryText(Expression expression)
        {
            SearchCriteria criteria;

            criteria = new PostExpressionVisitor().ProcessExpression(expression);

            string url = PostHelper.BuildUrl(criteria);
            return url;
        }
    }
}
