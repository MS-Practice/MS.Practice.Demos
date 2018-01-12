// The code in this file comes from Matt Warren's series of blog posts on how to build a LINQ provider
// http://blogs.msdn.com/mattwar/archive/2007/08/09/linq-building-an-iqueryable-provider-part-i.aspx
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpresstionTreeDemo.QueryableAbstract
{
    public class Query<T> :
    IQueryable<T>, IQueryable,
    IEnumerable<T>, IEnumerable,
    IOrderedQueryable<T>, IOrderedQueryable
    {
        private QueryProvider queryProvider;
        private Expression expression;

        public Query():this(new CnblogsQueryProvider())
        {

        }

        public Query(QueryProvider queryProvider)
        {
            this.queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
            this.queryProvider = queryProvider;
            this.expression = Expression.Constant(this);
        }

        public Query(QueryProvider provider, Expression expression)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException(nameof(expression));
            }
            this.queryProvider = provider;
            this.expression = expression;
        }

        Expression IQueryable.Expression => this.expression;

        Type IQueryable.ElementType => typeof(T);

        IQueryProvider IQueryable.Provider => this.queryProvider;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.queryProvider.Execute(this.expression)).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)this.queryProvider.Execute(this.expression)).GetEnumerator();
        }
    }
}