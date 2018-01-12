using ExpresstionTreeDemo.CNBlog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpresstionTreeDemo.QueryableAbstract
{
    public class PostExpressionVisitor
    {
        private SearchCriteria _criteria;

        public SearchCriteria ProcessExpression(Expression expression)
        {
            _criteria = new SearchCriteria();
            Visit(expression);
            return _criteria;
        }

        private void Visit(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Add:
                    break;
                case ExpressionType.AddChecked:
                    break;
                case ExpressionType.And:
                    break;
                case ExpressionType.AndAlso:
                    VisitAndAlso((BinaryExpression)expression);
                    break;
                case ExpressionType.ArrayLength:
                    break;
                case ExpressionType.ArrayIndex:
                    break;
                case ExpressionType.Call:
                    VisitMethodCall((MethodCallExpression)expression);
                    break;
                case ExpressionType.Coalesce:
                    break;
                case ExpressionType.Conditional:
                    break;
                case ExpressionType.Constant:
                    break;
                case ExpressionType.Convert:
                    break;
                case ExpressionType.ConvertChecked:
                    break;
                case ExpressionType.Divide:
                    break;
                case ExpressionType.Equal:
                    VisitEqual((BinaryExpression)expression);
                    break;
                case ExpressionType.ExclusiveOr:
                    break;
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                    GreaterThanOrEqual((BinaryExpression)expression);
                    break;
                case ExpressionType.Invoke:
                    break;
                case ExpressionType.Lambda:
                    Visit(((LambdaExpression)expression).Body);
                    break;
                case ExpressionType.LeftShift:
                    break;
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                    VisitLessThanOrEqual((BinaryExpression)expression);
                    break;
                case ExpressionType.ListInit:
                    break;
                case ExpressionType.MemberAccess:
                    break;
                case ExpressionType.MemberInit:
                    break;
                case ExpressionType.Modulo:
                    break;
                case ExpressionType.Multiply:
                    break;
                case ExpressionType.MultiplyChecked:
                    break;
                case ExpressionType.Negate:
                    break;
                case ExpressionType.UnaryPlus:
                    break;
                case ExpressionType.NegateChecked:
                    break;
                case ExpressionType.New:
                    break;
                case ExpressionType.NewArrayInit:
                    break;
                case ExpressionType.NewArrayBounds:
                    break;
                case ExpressionType.Not:
                    break;
                case ExpressionType.NotEqual:
                    break;
                case ExpressionType.Or:
                    break;
                case ExpressionType.OrElse:
                    break;
                case ExpressionType.Parameter:
                    break;
                case ExpressionType.Power:
                    break;
                case ExpressionType.Quote:
                    break;
                case ExpressionType.RightShift:
                    break;
                case ExpressionType.Subtract:
                    break;
                case ExpressionType.SubtractChecked:
                    break;
                case ExpressionType.TypeAs:
                    break;
                case ExpressionType.TypeIs:
                    break;
                case ExpressionType.Assign:
                    break;
                case ExpressionType.Block:
                    break;
                case ExpressionType.DebugInfo:
                    break;
                case ExpressionType.Decrement:
                    break;
                case ExpressionType.Dynamic:
                    break;
                case ExpressionType.Default:
                    break;
                case ExpressionType.Extension:
                    break;
                case ExpressionType.Goto:
                    break;
                case ExpressionType.Increment:
                    break;
                case ExpressionType.Index:
                    break;
                case ExpressionType.Label:
                    break;
                case ExpressionType.RuntimeVariables:
                    break;
                case ExpressionType.Loop:
                    break;
                case ExpressionType.Switch:
                    break;
                case ExpressionType.Throw:
                    break;
                case ExpressionType.Try:
                    break;
                case ExpressionType.Unbox:
                    break;
                case ExpressionType.AddAssign:
                    break;
                case ExpressionType.AndAssign:
                    break;
                case ExpressionType.DivideAssign:
                    break;
                case ExpressionType.ExclusiveOrAssign:
                    break;
                case ExpressionType.LeftShiftAssign:
                    break;
                case ExpressionType.ModuloAssign:
                    break;
                case ExpressionType.MultiplyAssign:
                    break;
                case ExpressionType.OrAssign:
                    break;
                case ExpressionType.PowerAssign:
                    break;
                case ExpressionType.RightShiftAssign:
                    break;
                case ExpressionType.SubtractAssign:
                    break;
                case ExpressionType.AddAssignChecked:
                    break;
                case ExpressionType.MultiplyAssignChecked:
                    break;
                case ExpressionType.SubtractAssignChecked:
                    break;
                case ExpressionType.PreIncrementAssign:
                    break;
                case ExpressionType.PreDecrementAssign:
                    break;
                case ExpressionType.PostIncrementAssign:
                    break;
                case ExpressionType.PostDecrementAssign:
                    break;
                case ExpressionType.TypeEqual:
                    break;
                case ExpressionType.OnesComplement:
                    break;
                case ExpressionType.IsTrue:
                    break;
                case ExpressionType.IsFalse:
                    break;
                default:
                    break;
            }
        }

        private void VisitLessThanOrEqual(BinaryExpression expression)
        {
            // 处理 Diggs <= n  推荐人数
            if ((expression.Left.NodeType == ExpressionType.MemberAccess) &&
(((MemberExpression)expression.Left).Member.Name == "Diggs"))
            {
                if (expression.Right.NodeType == ExpressionType.Constant)
                    _criteria.MaxDiggs =
            (int)((ConstantExpression)expression.Right).Value;

                else if (expression.Right.NodeType == ExpressionType.MemberAccess)
                    _criteria.MaxDiggs =
      (int)GetMemberValue((MemberExpression)expression.Right);

                else
                    throw new NotSupportedException("Expression type not supported for Diggs: "
        + expression.Right.NodeType.ToString());
            }
            // 处理 Views<= n   访问人数
            else if ((expression.Left.NodeType == ExpressionType.MemberAccess) &&
       (((MemberExpression)expression.Left).Member.Name == "Views"))
            {
                if (expression.Right.NodeType == ExpressionType.Constant)
                    _criteria.MaxViews =
          (int)((ConstantExpression)expression.Right).Value;

                else if (expression.Right.NodeType == ExpressionType.MemberAccess)
                    _criteria.MaxViews =
          (int)GetMemberValue((MemberExpression)expression.Right);

                else
                    throw new NotSupportedException("Expression type not supported for Views: "
        + expression.Right.NodeType.ToString());
            }
            // 处理 comments <= n   评论数
            else if ((expression.Left.NodeType == ExpressionType.MemberAccess) &&
        (((MemberExpression)expression.Left).Member.Name == "Comments"))
            {
                if (expression.Right.NodeType == ExpressionType.Constant)
                    _criteria.MaxComments =
         (int)((ConstantExpression)expression.Right).Value;

                else if (expression.Right.NodeType == ExpressionType.MemberAccess)
                    _criteria.MaxComments =
           (int)GetMemberValue((MemberExpression)expression.Right);

                else
                    throw new NotSupportedException("Expression type not supported for Comments: "
         + expression.Right.NodeType.ToString());
            }

            // 处理发布时间 <= 
            else if ((expression.Left.NodeType == ExpressionType.MemberAccess) &&
       (((MemberExpression)expression.Left).Member.Name == "Published"))
            {
                if (expression.Right.NodeType == ExpressionType.Constant)
                    _criteria.End =
      (DateTime)((ConstantExpression)expression.Right).Value;

                else if (expression.Right.NodeType == ExpressionType.MemberAccess)
                    _criteria.End =
         (DateTime)GetMemberValue((MemberExpression)expression.Right);

                else
                    throw new NotSupportedException("Expression type not supported for Published: "
       + expression.Right.NodeType.ToString());
            }
        }

        private void GreaterThanOrEqual(BinaryExpression expression)
        {
            // 处理 Diggs >= n  推荐人数
            if ((expression.Left.NodeType == ExpressionType.MemberAccess) &&
       (((MemberExpression)expression.Left).Member.Name == "Diggs"))
            {
                if (expression.Right.NodeType == ExpressionType.Constant)
                    _criteria.MinDiggs =
        (int)((ConstantExpression)expression.Right).Value;

                else if (expression.Right.NodeType == ExpressionType.MemberAccess)
                    _criteria.MinDiggs =
            (int)GetMemberValue((MemberExpression)expression.Right);

                else
                    throw new NotSupportedException("Expression type not supported for Diggs:"
          + expression.Right.NodeType.ToString());
            }
            // 处理 Views>= n   访问人数
            else if ((expression.Left.NodeType == ExpressionType.MemberAccess) &&
        (((MemberExpression)expression.Left).Member.Name == "Views"))
            {
                if (expression.Right.NodeType == ExpressionType.Constant)
                    _criteria.MinViews =
       (int)((ConstantExpression)expression.Right).Value;

                else if (expression.Right.NodeType == ExpressionType.MemberAccess)
                    _criteria.MinViews =
         (int)GetMemberValue((MemberExpression)expression.Right);

                else
                    throw new NotSupportedException("Expression type not supported for Views: "
         + expression.Right.NodeType.ToString());
            }
            // 处理 comments >= n   评论数
            else if ((expression.Left.NodeType == ExpressionType.MemberAccess) &&
       (((MemberExpression)expression.Left).Member.Name == "Comments"))
            {
                if (expression.Right.NodeType == ExpressionType.Constant)
                    _criteria.MinComments =
        (int)((ConstantExpression)expression.Right).Value;

                else if (expression.Right.NodeType == ExpressionType.MemberAccess)
                    _criteria.MinComments =
        (int)GetMemberValue((MemberExpression)expression.Right);

                else
                    throw new NotSupportedException("Expression type not supported for Comments: "
         + expression.Right.NodeType.ToString());
            }
            // 处理 发布时间>=
            else if ((expression.Left.NodeType == ExpressionType.MemberAccess) &&
        (((MemberExpression)expression.Left).Member.Name == "Published"))
            {
                if (expression.Right.NodeType == ExpressionType.Constant)
                    _criteria.Start =
         (DateTime)((ConstantExpression)expression.Right).Value;

                else if (expression.Right.NodeType == ExpressionType.MemberAccess)
                    _criteria.Start =
          (DateTime)GetMemberValue((MemberExpression)expression.Right);

                else
                    throw new NotSupportedException("Expression type not supported for Published: "
         + expression.Right.NodeType.ToString());
            }
        }

        private void VisitEqual(BinaryExpression expression)
        {
            if ((expression.Left.NodeType == ExpressionType.MemberAccess) &&
                (((MemberExpression)expression.Left).Member.Name == "Author"))
            {
                if (expression.Right.NodeType == ExpressionType.Constant)
                {
                    _criteria.Author = ((ConstantExpression)expression.Right).Value.ToString();
                }
                else if (expression.Right.NodeType == ExpressionType.MemberAccess)
                {
                    _criteria.Author = GetMemberValue((MemberExpression)expression.Right).ToString();
                }
                else
                {
                    throw new NotSupportedException("Expression type not supported for author: " +
                     expression.Right.NodeType.ToString());
                }
            }
            Visit(expression);
        }

        private void VisitMethodCall(MethodCallExpression expression)
        {
            if (expression.Method.DeclaringType == typeof(Queryable) &&
                expression.Method.Name == "Where")
            {
                Visit(((UnaryExpression)(expression.Arguments[1])).Operand);
            }
            else if (expression.Method.DeclaringType == typeof(string) &&
                expression.Method.Name == "Contains")
            {
                if (expression.Object.NodeType == ExpressionType.MemberAccess)
                {
                    MemberExpression memberExpr = (MemberExpression)expression.Object;
                    if (memberExpr.Expression.Type == typeof(Post))
                    {
                        if (memberExpr.Member.Name == "Title")
                        {
                            Expression argument;
                            argument = expression.Arguments[0];
                            if (argument.NodeType == ExpressionType.Constant)
                            {
                                _criteria.Title = (String)((ConstantExpression)argument).Value;
                            }
                            else if (argument.NodeType == ExpressionType.MemberAccess)
                            {
                                _criteria.Title = (String)GetMemberValue((MemberExpression)argument);
                            }
                            else
                            {
                                throw new NotSupportedException("Expression type not supported: " + argument.NodeType.ToString());
                            }
                        }
                    }
                }
            }
        }

        private void VisitAndAlso(BinaryExpression andAlso)
        {
            Visit(andAlso.Left);
            Visit(andAlso.Right);
        }


        private object GetMemberValue(MemberExpression memberExpression)
        {
            MemberInfo memberInfo;
            Object obj;
            if (memberExpression == null)
                throw new ArgumentNullException(nameof(memberExpression));

            if (memberExpression.Expression is ConstantExpression)
                obj = ((ConstantExpression)memberExpression.Expression).Value;
            else if (memberExpression.Expression is MemberExpression)
                obj = GetMemberValue((MemberExpression)memberExpression.Expression);
            else
                throw new NotSupportedException("Expression type not supported: " + memberExpression.Expression.GetType().FullName);

            memberInfo = memberExpression.Member;
            if (memberInfo is PropertyInfo)
            {
                PropertyInfo property = (PropertyInfo)memberInfo;
                return property.GetValue(obj, null);
            }
            else if (memberInfo is FieldInfo)
            {
                FieldInfo field = (FieldInfo)memberInfo;
                return field.GetValue(obj);
            }
            else
            {
                throw new NotSupportedException("MemberInfo type not supported: " + memberInfo.GetType().FullName);
            }
        }
    }
}
