using System;
using System.Reflection.Emit;

namespace Core.Lib.ObjectProxy.Abstractions
{
    public interface ITypeProxyBuilder
    {
        TypeBuilder Create(Type serviceType);
    }
}