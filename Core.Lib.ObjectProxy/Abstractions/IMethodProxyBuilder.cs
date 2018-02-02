using System;
using System.Reflection.Emit;

namespace Core.Lib.ObjectProxy.Abstractions
{
    public interface IMethodProxyBuilder
    {
        void Create(Type serviceType, TypeBuilder builder);
    }
}