using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LinqProgramDemo.Solutions
{
    public class PrefixTreeVisitor : ExpressionVisitor
    {
        private Hashtable m_storage;
        private static readonly object s_nullKey = new object();

        public Hashtable CurrentStorage { get; private set; }

        public bool StopLookingUpWhenMissed { get; private set; }

        public PrefixTreeVisitor(Hashtable storage, bool stopLookingUpWhenMissed)
        {
            this.m_storage = storage;
            this.StopLookingUpWhenMissed = stopLookingUpWhenMissed;
        }

        public Hashtable Accept(Expression exp)
        {
            this.CurrentStorage = this.m_storage;
            this.Visit(exp);
            return this.CurrentStorage;
        }

        protected virtual bool LookUp(object key)
        {
            if (this.CurrentStorage == null) return true;

            key = key ?? s_nullKey;
            Hashtable next = this.CurrentStorage[key] as Hashtable;
            if(next == null && !this.StopLookingUpWhenMissed)
            {
                next = new Hashtable();
                this.CurrentStorage[key] = next;
            }

            this.CurrentStorage = next;
            return next == null;
        }

        public override Expression Visit(Expression exp)
        {
            if (exp == null) return exp;

            var key = new
            {
                exp.NodeType,
                exp.Type
            };

            if (this.LookUp(key)) return exp;
            return base.Visit(exp);
        }
        protected override Expression VisitBinary(BinaryExpression b)
        {
            var key = new
            {
                b.IsLifted,
                b.IsLiftedToNull,
                b.Method
            };

            if (this.LookUp(key)) return b;
            return base.VisitBinary(b);
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (this.LookUp(c.Value)) return c;
            return base.VisitConstant(c);
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (this.LookUp(m.Member)) return m;
            return base.VisitMember(m);
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (this.LookUp(m.Method)) return m;
            return base.VisitMethodCall(m);
        }
    }
}
