using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace ExpressionProgram
{
    class ExpressionLambdaDemoT : ExpressionVisitor
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

    public class WeakTypeDelegateGenerator : ExpressionVisitor
    {
        private List<ParameterExpression> m_parameters;
        public Delegate Generate(Expression exp) {
            this.m_parameters = new List<ParameterExpression>();
            var body = this.Visit(exp);
            var lambda = Expression.Lambda(body, this.m_parameters.ToArray());
            return lambda.Compile();
        }

        protected override Expression VisitConstant(ConstantExpression v)
        {
            var p = Expression.Parameter(v.Type, "p" + this.m_parameters.Count);
            this.m_parameters.Add(p);
            return p;
        }
    }

    //public class CacheEvaluator : IEvaluator
    //{
    //    private static IExpressionCache<Delegate> s_cache = new HashedListCache<Delegate>();

    //    private WeakTypeDelegateGenerator m_delegateGenerator = new WeakTypeDelegateGenerator();
    //    private ConstantExtractor m_constantExtrator = new ConstantExtractor();

    //    private IExpressionCache<Delegate> m_cache;
    //    private Func<Expression, Delegate> m_creatorDelegate;

    //    public CacheEvaluator()
    //        : this(s_cache)
    //    { }

    //    public CacheEvaluator(IExpressionCache<Delegate> cache)
    //    {
    //        this.m_cache = cache;
    //        this.m_creatorDelegate = (key) => this.m_delegateGenerator.Generate(key);
    //    }

    //    public object Eval(Expression exp)
    //    {
    //        if (exp.NodeType == ExpressionType.Constant)
    //        {
    //            return ((ConstantExpression)exp).Value;
    //        }

    //        var parameters = this.m_constantExtrator.Extract(exp);
    //        var func = this.m_cache.Get(exp, this.m_creatorDelegate);
    //        return func.DynamicInvoke(parameters.ToArray());
    //    }
    //}

}
