using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Collections;

namespace ExpressionProgram
{
    public class SimpleKeyBuilder : ExpressionVisitor
    {
        public string Build(Expression exp)
        {
            this.m_builder = new StringBuilder();
            this.Visit(exp);
            return this.Key;
        }
        public string Key { get { return this.m_builder.ToString(); } }
        private StringBuilder m_builder;

        protected virtual SimpleKeyBuilder Accept(bool value)
        {
            this.m_builder.Append(value ? "1|" : "0|");
            return this;
        }

        protected virtual SimpleKeyBuilder Accept(Type type) {
            this.m_builder.Append(type == null ? "null" : type.FullName).Append("|");
            return this;
        }

        protected virtual SimpleKeyBuilder Accept(MemberInfo member)
        {
            if (member == null) {
                this.m_builder.Append("null|");
                return this;
            }
            return this.Accept(member.DeclaringType).Accept(member.Name);
        }

        protected virtual SimpleKeyBuilder Accept(object value)
        {
            this.m_builder.Append(value == null ? "null" : value.ToString()).Append("|");
            return this;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            this.Accept(node.IsLifted).Accept(node.IsLiftedToNull).Accept(node.Method);
            return base.VisitBinary(node);
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            this.Accept(c.Value);
            return base.VisitConstant(c);
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            this.Accept(m.Member);
            return base.VisitMember(m);
        }
    }

    public class SimpleKeyCache<T> : IExpressionCache<T> where T : class
    {
        private ReaderWriterLockSlim m_rwlock = new ReaderWriterLockSlim();
        private Dictionary<string, T> m_storge = new Dictionary<string, T>();

        public T Get(Expression key, Func<Expression, T> creator) {
            T value;
            string cachekey = new SimpleKeyBuilder().Build(key);
            this.m_rwlock.EnterReadLock();
            try
            {
                if (this.m_storge.TryGetValue(cachekey, out value))
                    return value;
            }
            finally
            {
                this.m_rwlock.ExitReadLock();
            }
            this.m_rwlock.EnterWriteLock();
            try
            {
                if (this.m_storge.TryGetValue(cachekey, out value))
                    return value;
                value = creator(key);
                this.m_storge.Add(cachekey, value);
                return value;
            }
            finally {
                this.m_rwlock.ExitWriteLock();
            }
        }
    }

    public class PrefixTreeVisitor : ExpressionVisitor
    {
        private Hashtable m_storage;
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

        private static readonly object s_nullkey = new object();

        protected virtual bool LookUp(object key) {
            if (this.CurrentStorage == null) return true;
            key = key ?? s_nullkey;
            Hashtable next = this.CurrentStorage[key] as Hashtable;
            if (next == null && !this.StopLookingUpWhenMissed)
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
                NodeType = exp.NodeType,
                Type = exp.Type
            };

            if (this.LookUp(key)) return exp;
            return base.Visit(exp);
        }
        protected override Expression VisitBinary(BinaryExpression b)
        {
            var key = new
            {
                IsLifted = b.IsLifted,
                IsLiftedToNull = b.IsLiftedToNull,
                Method = b.Method
            };

            if (this.LookUp(key)) return b;
            return base.VisitBinary(b);
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (this.LookUp(c.Value)) return c;
            return base.VisitConstant(c);
        }

        //protected override Expression VisitMemberAccess(MemberExpression m)
        //{
        //    if (this.LookUp(m.Member)) return m;
        //    return base.VisitMemberAccess(m);
        //}

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (this.LookUp(m.Method)) return m;
            return base.VisitMethodCall(m);
        }

    }
}
