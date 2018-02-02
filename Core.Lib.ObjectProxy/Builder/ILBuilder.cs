using System.Reflection.Emit;
using System.Reflection;
using System;
using Core.Lib.Sharedd.Helper;
using Core.Lib.ObjectProxy.Abstractions;

namespace Core.Lib.ObjectProxy.Builder
{
    internal class ILBuilder : IILBuilder
    {
        public void DefineIL(MethodInfo method, ILGenerator il,Action<ILGenerator> invoke)
        {
            il.Emit(OpCodes.Ldarg_0);
            invoke(il);
            method.Condition(m => m.ReturnType == typeof(void))
                .IsTrue(_ => (Action)(() => il.Emit(OpCodes.Nop)))
                .Or(_ => () => { })
                .Invoke();
            il.Emit(OpCodes.Ret);
        }
    }
}