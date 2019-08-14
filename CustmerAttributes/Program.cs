using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
[assembly: CLSCompliant(true)]
namespace CustmerAttributes
{
    [Serializable]
    [DefaultMemberAttribute("Main")]
    [DebuggerDisplayAttribute("Swift", Name = "Marson", Target = typeof(Program))]
    public sealed class Program
    {
        [CLSCompliant(true)]
        [STAThread]
        static void Main(string[] args)
        {
            //显示应用于这个类型的Attribute集
            ShowAttributes(typeof(Program));
            MemberInfo[] members = typeof(Program).FindMembers(MemberTypes.Method | MemberTypes.Constructor
                , BindingFlags.DeclaredOnly | BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Static, Type.FilterName, "*");
            foreach (MemberInfo member in members)
            {
                //显示这个应用于这个成员的Attribute集
                ShowAttributes(member);
            }

            CanWriteCheck(new ChildAccount());
            CanWriteCheck(new AdultAccount());
            //没用应用AccountAttribute的类型
            CanWriteCheck(new Program());
        }
        private static void CanWriteCheck(Object obj)
        {
            //够一个Attribute的一个实例，并初始化
            //我们要显式查找的内容
            Attribute checking = new AccountsAttribute(Accounts.Checking);
            //构造应用于类型的attributes实例
            Attribute validAccount = Attribute.GetCustomAttribute(obj.GetType(),typeof(AccountsAttribute), false);
            //如果Attribute应用于账户，且账户名指定了“Checking”账户，表明该类型可以开支票
            if ((validAccount != null) && checking.Match(validAccount))
            {
                Console.WriteLine("{0} types can write checks.", obj.GetType());
            }
            else
            {
                Console.WriteLine("{0} types can Not write checks.", obj.GetType());
            }
        }

        private static void ShowAttributes(MemberInfo attributesTarget)
        {
            Attribute[] attributes = Attribute.GetCustomAttributes(attributesTarget);
            Console.WriteLine("Attributes applied to {0} : {1}", attributesTarget.Name, (attributes.Length == 0 ? "None" : String.Empty));
            foreach (var attribute in attributes)
            {
                //显示以应用与每个Attribute的类型
                Console.WriteLine(" {0}", attribute.GetType().ToString());
                if (attribute is DefaultMemberAttribute)
                    Console.WriteLine(" MemberName={0}", ((DefaultMemberAttribute)attribute).MemberName);
                if (attribute is ConditionalAttribute)
                    Console.WriteLine(" ConditionString={0}", ((ConditionalAttribute)attribute).ConditionString);
                if (attribute is CLSCompliantAttribute)
                    Console.WriteLine(" IsCompliant={0}", ((CLSCompliantAttribute)attribute).IsCompliant);
                DebuggerDisplayAttribute dda = attribute as DebuggerDisplayAttribute;
                if (dda != null)
                {
                    Console.WriteLine(" Value={0},Name={1},Target={2}", dda.Value, dda.Name, dda.Target);
                }
            }
            Console.WriteLine();
        }
        private static void ShowAttributes(MemberInfo attributesTarget, int kindValue)
        {
            IList<CustomAttributeData> attributes =
                CustomAttributeData.GetCustomAttributes(attributesTarget);
            Console.WriteLine("Attributes applied to {0} : {1}", attributesTarget.Name, (attributes.Count == 0 ? "None" : String.Empty));
            foreach (CustomAttributeData attribute in attributes) {
                //显示以应用与每个Attribute的类型
                Type t = attribute.Constructor.DeclaringType;
                Console.WriteLine(" {0}", t.ToString());
                Console.WriteLine(" Constructor called={0}", attribute.Constructor);

                IList<CustomAttributeTypedArgument> posArgs = attribute.ConstructorArguments;
                Console.WriteLine(" Positional arguments passed to constructor:" + ((posArgs.Count == 0) ? " None" : string.Empty));
                foreach (CustomAttributeTypedArgument pa in posArgs)
                {
                    Console.WriteLine(" Type={0},Value={1}", pa.ArgumentType, pa.Value);
                }
                IList<CustomAttributeNamedArgument> nameArgs = attribute.NamedArguments;
                Console.WriteLine(" Named arguments set after construction:" + ((nameArgs.Count == 0) ? " None" : string.Empty));
                foreach (CustomAttributeNamedArgument name in nameArgs)
                {
                    Console.WriteLine(" Name={0},Type={1},Value={2}", name.MemberInfo.Name, name.TypedValue.ArgumentType, name.TypedValue.Value);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void TestAttribute()
        {
        }
        [Conditional("Debug")]
        [Conditional("Release")]
        private void DoSomething() { }
    }

    internal enum Color { Red }
    internal sealed class SomeAttribute : Attribute
    {
        public SomeAttribute(String name, Object o, Type[] types)
        {
            //'name' 引用一个String
            //'0'引用一个合法的Object类型
            //'types'引用一个一维0基Type数组
        }
    }
    [Some("Marson", Color.Red, new Type[] { typeof(Math), typeof(Console) })]
    internal sealed class SomeType
    {

    }
    [Flags]
    internal enum Accounts
    {
        Savings = 0x0001,
        Checking = 0x0002,
        Brokerage = 0x0004
    }
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class AccountsAttribute : Attribute
    {
        private Accounts m_accounts;
        public AccountsAttribute(Accounts accounts)
        {
            m_accounts = accounts;
        }
        public override bool Match(Object obj)
        {
            //如果基类实现了Match，而且基类不是Attribute
            //就取消下面这行的注释
            //if(!base.Match(obj)) retun false;

            //由于'this'不为null，所以假如obj为null，
            //那么对象肯定不匹配
            //注意：如果你信任基类正确实现了Match 那么可以删除下面这行代码
            if (obj == null) return false;
            //如果对象属于不同的类型，肯定不匹配
            //注意：如果你信任基类正确实现了Match
            //那么下面行代码可以删除
            if (this.GetType() != obj.GetType()) return false;
            //将obj转型为我们的类型以访问字段
            //注意：转型不可能失败，因为两者类型是相同的
            AccountsAttribute other = (AccountsAttribute)obj;
            //比较字段，判断它们具有相同的值
            //这个例子判断'this'的账户是不是other的账户的一个子集
            if ((other.m_accounts & m_accounts) != m_accounts)
                return false;
            return true;    // 对象匹配
        }
        public override bool Equals(object obj)
        {
            //如果基类实现了Equals，而且基类不是Attribute
            //就取消下面这行的注释
            //if(!base.Equals(obj)) retun false;

            //由于'this'不为null，所以假如obj为null，
            //那么对象肯定不匹配
            //注意：如果你信任基类正确实现了Equals 那么可以删除下面这行代码
            if (obj == null) return false;

            //如果对象属于不同的类型，肯定不匹配
            //注意：如果你信任基类正确实现了Equals
            //那么下面行代码可以删除
            if (this.GetType() != obj.GetType()) return false;

            //将obj转型为我们的类型以访问字段
            //注意：转型不可能失败，因为两者类型是相同的
            AccountsAttribute other = (AccountsAttribute)obj;
            //比较字段，判断它们具有相同的值
            //这个例子判断'this'的账户是不是other的账户
            if (other.m_accounts != m_accounts)
                return false;
            return true;//对象相等
        }
    }
    [Accounts(Accounts.Savings)]
    internal sealed class ChildAccount { }
    [Accounts(Accounts.Checking | Accounts.Savings | Accounts.Brokerage)]
    internal sealed class AdultAccount { }
}
