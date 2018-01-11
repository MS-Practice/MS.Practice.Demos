using LinqProgramDemo.Solutions;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Threading;

namespace LinqProgramDemo
{
    public class PrefixTreeCache<T> : IExpressionCache<T> where T : class
    {
        private Hashtable m_storage = new Hashtable();
        private ReaderWriterLockSlim m_rwLock = new ReaderWriterLockSlim();

        public T Get(Expression key, Func<Expression, T> creator)
        {
            T value;

            this.m_rwLock.EnterReadLock();
            try
            {
                value = this.Get(key);
                if (value != null) return value;
            }
            finally
            {
                this.m_rwLock.ExitReadLock();
            }

            this.m_rwLock.EnterWriteLock();
            try
            {
                value = this.Get(key);
                if (value != null) return value;

                value = creator(key);
                this.Set(key, value);
                return value;
            }
            finally
            {
                this.m_rwLock.ExitWriteLock();
            }
        }

        public void Set(Expression key, T value)
        {
            var visitor = new PrefixTreeVisitor(m_storage, false);
            var storage = visitor.Accept(key);
            storage[typeof(T)] = value;
        }

        public T Get(Expression key)
        {
            var visitor = new PrefixTreeVisitor(this.m_storage, true);
            var storage = visitor.Accept(key);
            return storage == null ? null : (T)storage[typeof(T)];
        }
    }
}
