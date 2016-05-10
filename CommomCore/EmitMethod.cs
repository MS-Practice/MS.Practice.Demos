using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace CommomCore
{
    class EmitMethod
    {
    }
    public class Parser<T, TResult> {
        public static readonly Func<T, TResult> Func = Emit();

        private static Func<T, TResult> Emit()
        {
            var method = new DynamicMethod(String.Empty, typeof(TResult), new[] { typeof(T) });
            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ret);
            return (Func<T, TResult>)method.CreateDelegate(typeof(Func<T, TResult>));
        }
    }
    public class TicksToDateTimeCaller {
        private static DateTime TicksToDateTime(long ticks) {
            return new DateTime(ticks);
        }
        public TResult Call<T, TResult>(T arg) {
            return Parser<DateTime, TResult>.Func(TicksToDateTime(Parser<T, long>.Func(arg)));
        }
    }
}
