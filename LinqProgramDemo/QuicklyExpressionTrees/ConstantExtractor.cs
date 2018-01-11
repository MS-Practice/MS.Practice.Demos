using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LinqProgramDemo
{
    public class ConstantExtractor : ExpressionVisitor
    {
        private List<object> m_constants;

        public List<object> Extract(Expression exp)
        {
            this.m_constants = new List<object>();
            this.Visit(exp);
            return this.m_constants;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            this.m_constants.Add(c.Value);
            return c;
        }
    }
}
