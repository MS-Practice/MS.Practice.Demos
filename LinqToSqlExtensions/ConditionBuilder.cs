using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace LinqToSqlExtensions
{
    internal class ConditionBuilder : ExpressionVisitor
    {
        private List<object> arguments;
        private Stack<string> m_conditionParts;
        public string Condition { get; private set; }

        public object[] Arguments { get; private set; }
        public void Builder(Expression expression) {
            PartialEvaluator evaluator = new PartialEvaluator();
            Expression evaluatedExpression = evaluator.Eval(expression);
        }
    }
}
