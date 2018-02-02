using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection.Emit;

namespace Core.Lib.ObjectProxy.Abstractions
{
    public interface IConstructorProxyBuilder
    {
        void Create(ServiceDescriptor descriptor, TypeBuilder builder);
    }
}