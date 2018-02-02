using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Extensions.Options;
using Core.Lib.ObjectProxy.Abstractions;
using Core.Lib.ObjectProxy.Internal;

namespace Core.Lib.ObjectProxy.Builder
{
    internal class ConstructorProxyBuilder : IConstructorProxyBuilder
    {
        private readonly ServiceProxyOptions _options;
        private readonly IOptionsMonitorCache<IBuilderContainer> _cache;

        public ConstructorProxyBuilder(IOptions<ServiceProxyOptions> options,IOptionsMonitorCache<IBuilderContainer> cache)
        {
            _options = options.Value;
            _cache = cache;
        }

        public void Create(ServiceDescriptor descriptor, TypeBuilder builder)
            => CreateConstructor(descriptor, builder);

        private void CreateConstructor(ServiceDescriptor descriptor, TypeBuilder builder)
        {
            var serviceType = descriptor.ServiceType;
            var container = _cache.GetOrAdd(serviceType.FullName, () => new ProxyBuilderContainer());
            var constructor = builder.DefineConstructor(MethodAttributes.Public,
                      CallingConventions.Standard, new Type[] { descriptor.ImplementationType ?? serviceType });

            var il = constructor.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, container.DefindedFields[0]);
            il.Emit(OpCodes.Ret);
            container.DefindedConstructors.Add(constructor);
        }
    }
}
