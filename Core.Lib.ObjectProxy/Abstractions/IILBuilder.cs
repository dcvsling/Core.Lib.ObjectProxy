using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Core.Lib.ObjectProxy.Abstractions
{
    public interface IILBuilder
    {
        void DefineIL(MethodInfo method, ILGenerator il, Action<ILGenerator> invoke);
    }
}