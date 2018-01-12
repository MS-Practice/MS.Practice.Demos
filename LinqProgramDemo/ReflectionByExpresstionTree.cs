using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace LinqProgramDemo
{
    public static class ReflectionByExpresstionTreeExtension
    {
        public static Func<object> CreateInstanceDelegate(this Type type)
        {
            NewExpression newExp = Expression.New(type);
            Expression<Func<object>> lambdaExp = Expression.Lambda<Func<object>>(newExp, null);
            Func<object> func = lambdaExp.Compile();
            return func;
        }


        public static Func<object[],object> CreateInstanceDelegate(this Type type,Type[] parameterTypes)
        {
            //根据参数类型数组来获取构造函数
            var constructor = type.GetConstructor(parameterTypes);
            //创建lambda表达式的参数
            var lambdaParam = Expression.Parameter(typeof(object[]), "_args");
            //创建构造函数的参数表达式数组
            var constructorParam = BuildParameters(parameterTypes, lambdaParam);
            //创建构造函数表达式
            NewExpression newExp = Expression.New(constructor, constructorParam);
            //创建lambda表达式，返回构造函数
            Expression<Func<object[], object>> lambdaExp =
                Expression.Lambda<Func<object[], object>>(newExp, lambdaParam);
            Func<object[], object> func = lambdaExp.Compile();
            return func;
        }

        private static Expression[] BuildParameters(Type[] parameterTypes, ParameterExpression paramExp)
        {
            List<Expression> list = new List<Expression>();
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                //从参数表达式（参数是：object[]）中取出参数
                var arg = Expression.ArrayIndex(paramExp, Expression.Constant(i));
                //把参数转化成指定类型
                var argCast = Expression.Convert(arg, parameterTypes[i]);

                list.Add(argCast);
            }
            return list.ToArray();
        }
    }
}
