using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LinqProgramDemo
{
    public interface IExpressionCache<T> where T:class
    {
        T Get(Expression key, Func<Expression, T> creator);
    }
}
