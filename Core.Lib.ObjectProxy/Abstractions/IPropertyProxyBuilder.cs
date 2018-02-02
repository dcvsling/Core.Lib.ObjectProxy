using System;
using System.Reflection.Emit;

namespace Core.Lib.ObjectProxy.Abstractions
{
    public interface IPropertyProxyBuilder
    {
        void Create(Type serviceType, TypeBuilder builder);
    }
}