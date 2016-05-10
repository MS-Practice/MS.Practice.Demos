#define MACRO1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            Expression<Func<int, int>> exp1 = i => 1 + 2;
            Expression<Func<long, long>> exp2 = i => 1 + 2;
            //表达式树的ToString方法是丢失信息的。例如，如果表达式树中涉及方法调用，那么ToString也只会包含方法名，而无法表现出方法所属的类，以及它的返回值。如果要把一个表达式树完整地生成字符串，自然要用到ExpressionVisitor
            Console.WriteLine(exp1.ToString());
            Console.WriteLine(exp2.ToString());
            Expression<Func<double, double, double, double, double>> myExp = (a, b, m, n) => m * a * a + n * b * b;
            var calc = new BinaryExpressionCalculator(myExp);
            Console.WriteLine(calc.Calcutor(1, 2, 3, 4));
            Console.WriteLine(calc.Calcutor(1, 2, 3, 4));
            Expression<Func<DateTime>> expr = () => DateTime.Now.AddDays(1);
            Func<DateTime> tomorrow = expr.Compile();
            Console.WriteLine(tomorrow());
            Console.WriteLine(Eval(expr.Body));

            int[,] array = { { 1, 3, 8, 7, 6 }, { 14, 17, 22, 23, 25 }, { 4, 9, 11, 5, 13 }, { 16, 18, 20, 21, 2 }, { 10, 12, 19, 15, 24 } };
            var query = from i in array.Cast<int>() orderby i select i;
            foreach (var i in query)
            {
                Console.WriteLine(i);
            }

#if (MACRO1)
            Console.WriteLine("MACRO1 is defined");
#elif(MACRO2)
            Console.WriteLine("MACRO2 is defined");
#else
            Console.WriteLine("MACRO1 and MACRO2 are both defined");
#endif


        }

        //计算一个不明确的lambda表达式  要通过eval方法
        public static object Eval(Expression exp)
        {
            LambdaExpression lambda = Expression.Lambda(exp);
            Delegate func = lambda.Compile();
            return func.DynamicInvoke(null);
        }

        private void ParameterExpressionLambda()
        {
            ParameterExpression expA = Expression.Parameter(typeof(double), "a");   //参数a
            MethodCallExpression expCall = Expression.Call(null, typeof(Math).GetMethod("sin", System.Reflection.BindingFlags.Static | BindingFlags.Public), expA); //相当于Math.Sin(a)
            LambdaExpression exp = Expression.Lambda(expCall, expA);    //a=>Math.Sin(a)
            //表达式树字面量
            Expression<Func<double, double>> exp1 = a => Math.Sin(a);//这个就相当的于上面手动构建的lambda表达式树是一致的
        }
    }

    class BinaryExpressionCalculator
    {
        Dictionary<ParameterExpression, double> m_argDic;
        LambdaExpression m_exp;
        public BinaryExpressionCalculator(LambdaExpression exp)
        {
            this.m_exp = exp;
        }

        public string Calcutor(params double[] args)
        {
            //从ExpressionExpression中提取参数，和传输的实参对应起来。
            //生成的字典可以方便我们在后面查询参数的值
            m_argDic = new Dictionary<ParameterExpression, double>();

            for (int i = 0; i < m_exp.Parameters.Count; i++)
            {
                m_argDic[m_exp.Parameters[i]] = args[i];
            }
            //提取树根
            Expression rootExp = m_exp.Body as Expression;
            //计算
            return InternalPrefix(rootExp);
        }

        private double InternalCalc(Expression exp)
        {
            ConstantExpression cexp = exp as ConstantExpression;
            if (cexp != null) return (double)cexp.Value;
            ParameterExpression pexp = exp as ParameterExpression;
            if (pexp != null)
            {
                return m_argDic[pexp];
            }
            BinaryExpression bexp = exp as BinaryExpression;
            if (bexp == null) throw new ArgumentException("不支持表达式的类型", "exp");
            switch (exp.NodeType)
            {
                case ExpressionType.Add:
                    return InternalCalc(bexp.Left) + InternalCalc(bexp.Right);
                case ExpressionType.Divide:
                    return InternalCalc(bexp.Left) / InternalCalc(bexp.Right);
                case ExpressionType.Multiply:
                    return InternalCalc(bexp.Left) * InternalCalc(bexp.Right);
                case ExpressionType.Subtract:
                    return InternalCalc(bexp.Left) - InternalCalc(bexp.Right);
                default:
                    throw new ArgumentException("不支持表达式的类型", "exp");
            }
        }

        private string InternalPrefix(Expression exp)
        {
            ConstantExpression cexp = exp as ConstantExpression;
            if (cexp != null) return cexp.Value.ToString();
            ParameterExpression pexp = exp as ParameterExpression;
            if (pexp != null) return pexp.Name;
            BinaryExpression bexp = exp as BinaryExpression;
            if (bexp == null) throw new ArgumentException("不支持表达式的类型", "exp");
            switch (bexp.NodeType)
            {
                case ExpressionType.Add:
                    return "+ " + InternalPrefix(bexp.Left) + " " + InternalPrefix(bexp.Right);
                case ExpressionType.Divide:
                    return "- " + InternalPrefix(bexp.Left) + " " + InternalPrefix(bexp.Right);
                case ExpressionType.Multiply:
                    return "* " + InternalPrefix(bexp.Left) + " " + InternalPrefix(bexp.Right);
                case ExpressionType.Subtract:
                    return "/ " + InternalPrefix(bexp.Left) + " " + InternalPrefix(bexp.Right);
                default:
                    throw new ArgumentException("不支持表达式的类型", "exp");
            }
        }
    }
}
