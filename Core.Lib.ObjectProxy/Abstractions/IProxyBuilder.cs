using Microsoft.Extensions.DependencyInjection;
using System;

namespace Core.Lib.ObjectProxy.Abstractions
{
    public interface IProxyBuilder
    {
        Type Build(ServiceDescriptor descriptor);
    }
}