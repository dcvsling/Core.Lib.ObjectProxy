using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System;
using Microsoft.Extensions.Options;
using Core.Lib.ObjectProxy.Abstractions;
using Core.Lib.ObjectProxy.Internal;

namespace Core.Lib.ObjectProxy.Builder
{

    internal class MethodProxyBuilder : IMethodProxyBuilder
    {
        private readonly ServiceProxyOptions _options;
        private readonly IILBuilder _il;
        private readonly IOptionsMonitorCache<IBuilderContainer> _cache;

        public MethodProxyBuilder(IOptions<ServiceProxyOptions> options, IILBuilder il, IOptionsMonitorCache<IBuilderContainer> cache)
        {
            _options = options.Value;
            _il = il;
            _cache = cache;
        }

        public void Create(Type serviceType, TypeBuilder builder)
        {
            serviceType.GetInterfaces().Append(serviceType)
                .SelectMany(x => x.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                .Distinct()
                .Each(x => DefineMethod(serviceType, x, builder));
        }

        private void DefineMethod(Type serviceType, MethodInfo method, TypeBuilder builder)
        {
            var container = _cache.GetOrAdd(serviceType.FullName, () => new ProxyBuilderContainer());
            var result = builder.DefineMethod(
                method.Name,
                (MethodAttributes)(method.Attributes - MethodAttributes.Abstract) | MethodAttributes.Final,
                method.ReturnType,
                method.GetParameters().Select(x => x.ParameterType).ToArray());

            _il.DefineIL(
                method,
                result.GetILGenerator(),
                il => {
                    il.Emit(OpCodes.Ldfld, container.DefindedFields[0]);
                    Enumerable.Range(1, method.GetParameters().Length)
                        .Each(x => il.Emit(OpCodes.Ldarg_S, x));
                    il.EmitCall(OpCodes.Callvirt, method, null);
                });
            container.DefindedMethods.Add(result);
        }
    }
}