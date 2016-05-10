using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace CommomCore
{
    public class DynamicMethodExecutor
    {
        private Func<object, object[], object> m_execute;

        public DynamicMethodExecutor(MethodInfo methodInfo)
        {
            this.m_execute = this.GetExecuteDelegate(methodInfo);
        }

        public object Execute(object instance, object[] parameters)
        {
            return this.m_execute(instance, parameters);
        }

        private Func<object, object[], object> GetExecuteDelegate(MethodInfo methodInfo)
        {
            ParameterExpression instanceParameter = Expression.Parameter(typeof(object), "instance");
            ParameterExpression parametersParameter = Expression.Parameter(typeof(object[]), "parameters");
            //创建一个参数list
            List<Expression> parameterExpressions = new List<Expression>();
            ParameterInfo[] paramInfos = methodInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                //(Ti)parameters[i]
                BinaryExpression valueObj = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
                UnaryExpression valueCast = Expression.Convert(valueObj, paramInfos[i].ParameterType);
                parameterExpressions.Add(valueCast);
            }
            Expression instanceCast = methodInfo.IsStatic ? null :
                Expression.Convert(instanceParameter, methodInfo.ReflectedType);
            MethodCallExpression methodCall = Expression.Call(instanceCast, methodInfo, parameterExpressions);
            if (methodCall.Type == typeof(void))
            {
                Expression<Action<object, object[]>> lambda = Expression.Lambda<Action<object, object[]>>(methodCall, instanceParameter, parametersParameter);
                Action<object, object[]> execute = lambda.Compile();
                return (instance, parmeters) =>
                {
                    execute(instance, parmeters);
                    return null;
                };
            }
            else {
                UnaryExpression castMethodCall = Expression.Convert(methodCall, typeof(object));
                Expression<Func<object, object[], object>> lambda = Expression.Lambda<Func<object, object[], object>>(castMethodCall, instanceParameter, parametersParameter);
                return lambda.Compile();
            }
            
        }
    }
}
