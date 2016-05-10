using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace MS.Practice.Demos
{
    public class EmitReflector
    {
        public static void EmitMethod()
        {
            //创建一个程序集
            var asmName = new AssemblyName("EmitTest");
            var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, System.Reflection.Emit.AssemblyBuilderAccess.RunAndSave);
            //添加程序集模块
            var mdlBldr = asmBuilder.DefineDynamicModule("Main", "Main.dll");
            //定义类型
            var typeBldr = mdlBldr.DefineType("Hello", TypeAttributes.Public);
            //定义类成员
            var methodBldr = typeBldr.DefineMethod("SayHello", MethodAttributes.Public, null, null);
            //执行方法
            var il = methodBldr.GetILGenerator();   //获取IL生成器
            il.Emit(OpCodes.Ldstr, "Hello World");
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            il.Emit(OpCodes.Ret);
            //完成上面的步骤，一个类型好像就已经完成了。事实上却还没有，最后我们还必须显示的调用CreateType来完成类型的创建。
            typeBldr.CreateType();
            asmBuilder.Save("Main.dll");
        }
    }
}
