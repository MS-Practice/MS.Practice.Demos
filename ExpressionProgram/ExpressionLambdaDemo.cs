using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionProgram
{
    class ExpressionLambdaDemo
    {
        static object GeneralHandler(params object[] args)
        {
            Console.WriteLine("您的事件发生了说");
            return null;
        }
        public static void AttachGeneralHandler(object target, EventInfo targetEvent)
        {
            //获得事件响应程序的委托类型
            var delegateType = targetEvent.EventHandlerType;
            //这个委托的Invoke方法有我们所需的签名信息
            MethodInfo invokeMethod = delegateType.GetMethod("Invoke");
            //按照这个委托制作所需要的参数
            ParameterInfo[] parameters = invokeMethod.GetParameters();
            ParameterExpression[] paramsExp = new ParameterExpression[parameters.Length];
            Expression[] argsArrayExp = new Expression[parameters.Length];
            //参数一个个转成object类型。有些本身即是object，管他呢……
            for (int i = 0; i < parameters.Length; i++)
            {
                paramsExp[i] = Expression.Parameter(parameters[i].ParameterType, parameters[i].Name);
                argsArrayExp[i] = Expression.Convert(paramsExp[i], typeof(Object));
            }
            //调用我们的GeneralHandler
            MethodInfo executeMethod = typeof(ExpressionLambdaDemo).GetMethod(
                "GeneralHandler", BindingFlags.Static | BindingFlags.NonPublic);

            Expression lambdaBodyExp =
                Expression.Call(null, executeMethod,Expression.NewArrayInit(typeof(Object), argsArrayExp));
            //如果有返回值，那么将返回值转换成委托要求的类型
            //如果没有返回值就这样搁那里就成了
            if (!invokeMethod.ReturnType.Equals(typeof(void)))
            {
                //这是有返回值的情况
                lambdaBodyExp = Expression.Convert(lambdaBodyExp, invokeMethod.ReturnType);
            }
            //组装到一起
            LambdaExpression dynamicDelegateExp = Expression.Lambda(delegateType, lambdaBodyExp, paramsExp);

            //我们创建的Expression是这样的一个函数：
            //(委托的参数们) => GeneralHandler(new object[] { 委托的参数们 })

            //编译
            Delegate dynamiceDelegate = dynamicDelegateExp.Compile();

            //完成!
            targetEvent.AddEventHandler(target, dynamiceDelegate);

        }
        public void ExressionLamdaDemo()
        {
            ExpressionLambdaDemo.AttachGeneralHandler("Button1", "Button1".GetType().GetEvent("click"));
            //ParameterExpression pi = Expression.Parameter(typeof(int), "i");
            //var fexp =
            //    Expression.Lambda(
            //        Expression.Add(pi, Expression.Constant(1))
            //        , pi);
            //var f = fexp.Compile();
            //Console.WriteLine(f(3));
        }
    }
}
