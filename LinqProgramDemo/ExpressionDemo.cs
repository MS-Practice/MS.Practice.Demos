using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LinqProgramDemo
{
    public class ExpressionDemo
    {
        public Expression<Func<int, bool>> CreateExpressionTreeByApi()
        {
            //num => num < 5
            ParameterExpression numParam = Expression.Parameter(typeof(int), "num");
            ConstantExpression five = Expression.Constant(5, typeof(int));
            BinaryExpression numLessThanFive = Expression.LessThan(numParam, five);
            Expression<Func<int, bool>> lambda =
                Expression.Lambda<Func<int, bool>>(
                    numLessThanFive,
                    new ParameterExpression[] { numParam });
            return lambda;
        }

        public void CreateExpressionTreeForFactotriesOfNumber(int number)
        {
            //创建一个参数表达式
            ParameterExpression value = Expression.Parameter(typeof(int), "value");
            //创建一个表达式表示本地变量
            ParameterExpression result = Expression.Parameter(typeof(int), "result");
            //创建标签从循环跳到指定标签
            LabelTarget label = Expression.Label(typeof(int));
            //创建方法体
            BlockExpression block = Expression.Block(
                //添加本地变量
                new[] { result },
                //为本地变量赋值一个常量
                Expression.Assign(result, Expression.Constant(1)),
                //循环
                Expression.Loop(
                    //添加循环条件
                    Expression.IfThenElse(
                        //条件：value > 1
                        Expression.GreaterThan(value, Expression.Constant(1)),
                        //if true
                        Expression.MultiplyAssign(result, Expression.PostDecrementAssign(value)),
                        //if false
                        Expression.Break(label, result)
                        ),
                    label
                    )
                );

            int facotorial = Expression.Lambda<Func<int, int>>(block, value).Compile()(number);
            Console.WriteLine(facotorial);
        }

        public void DecomposedExpressionTrees()
        {
            //创建一个表达式树
            Expression<Func<int, bool>> exprTree = num => num < 5;
            //分解表达式
            ParameterExpression param = (ParameterExpression)exprTree.Parameters[0];
            //num < 5
            BinaryExpression operation = (BinaryExpression)exprTree.Body;
            ParameterExpression left = (ParameterExpression)operation.Left;
            ConstantExpression right = (ConstantExpression)operation.Right;

            Console.WriteLine("Decomposed expression: {0} => {1} {2} {3}",
                  param.Name, left.Name, operation.NodeType, right.Value);
        }

        public void ComplieExpressTrees()
        {
            //创建一个表达式树
            Expression<Func<int, bool>> expr = num => num < 5;
            //编译表达式树为委托
            Func<int, bool> result = expr.Compile();
            //调用委托并写结果到控制台
            Console.WriteLine(result(4));

            //也可以使用简单的语法来编译运行表达式树
            Console.WriteLine(expr.Compile()(4));
            //结果一样
        }
    }
}
