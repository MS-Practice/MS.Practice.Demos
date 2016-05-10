using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace ExpressionProgram
{
    /// <summary>
    /// 收集目标常量
    /// </summary>
    public class ConstantExtractor : ExpressionVisitor
    {
        private List<object> m_constants;
        public List<object> Extract(Expression exp)
        {
            this.m_constants = new List<object>();
            this.Visit(exp);
            return this.m_constants;
        }
        //表达式树作为键 值存储表达式树编译的结果
        protected override Expression VisitConstant(ConstantExpression c)
        {
            this.m_constants.Add(c.Value);
            return c;
        }
    }
}
