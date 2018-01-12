using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpresstionTreeDemo
{
    public class FastBuilderExpresstionTree
    {
        private ExpressionTreeCache m_cache;
        public FastBuilderExpresstionTree()
        {
            m_cache = new ExpressionTreeCache();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance">要求实例化的对象的类型</param>
        /// <param name="parameterTypes">构造函数参数类型</param>
        /// <returns></returns>
        private Func<object[], object> GetInstanceBuilderDelegate(Type instanceType, Type[] parameterTypes)
        {
            //获取缓存实例
            Func<object[], object> func;
            if (m_cache.TryGetValue(instanceType, out func))
            {
                return func;
            }
            var constructor = instanceType.GetConstructor(parameterTypes);
            ParameterExpression parameterArrayExpression = Expression.Parameter(typeof(object[]), "_args");
            List<Expression> parameterExpressionList = new List<Expression>();
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                //遍历数组参数
                var parameterArg = Expression.ArrayIndex(parameterArrayExpression, Expression.Constant(i));
                //给参数赋类型
                var parameterType = Expression.Convert(parameterArg, parameterTypes[i]);
                parameterExpressionList.Add(parameterArg);
            }
            //构造函数
            var body = Expression.New(constructor, parameterExpressionList);
            var lambda = Expression.Lambda<Func<object[], object>>(body, parameterArrayExpression);
            return lambda.Compile();
        }

        public object GetInstance(Type instance, object[] args)
        {
            var creator = GetInstanceBuilderDelegate(instance, args.Select(arg => arg.GetType()).ToArray());
            return creator(args);
        }

        public class ExpressionTreeCache
        {
            Dictionary<object, Func<object[], object>> m_cache = new Dictionary<object, Func<object[], object>>();

            public void Add(object key, Func<object[], object> value)
            {
                m_cache.Add(key, value);
            }

            public bool TryGetValue(object key, out Func<object[], object> value)
            {
                return m_cache.TryGetValue(key, out value);
            }

            private string CacheKeyBuild(Type type, Type[] args)
            {
                var key = type.FullName;
                Array.ForEach(args, arg =>
                {
                    key += "." + arg.Name;
                });
                return key;
            }
        }
    }
}
