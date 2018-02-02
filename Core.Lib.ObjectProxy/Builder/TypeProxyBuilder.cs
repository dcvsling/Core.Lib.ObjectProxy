using Microsoft.Extensions.Options;
using System.Linq;
using System.Reflection;
using System;
using System.Reflection.Emit;
using Core.Lib.Sharedd.Helper;
using Core.Lib.ObjectProxy.Abstractions;

namespace Core.Lib.ObjectProxy.Builder
{

    internal class TypeProxyBuilder : ITypeProxyBuilder
    {
        private readonly ServiceProxyOptions _options;
        private IOptionsMonitorCache<TypeBuilder> _cache;
        public TypeProxyBuilder(
            IOptions<ServiceProxyOptions> options, 
            IOptionsMonitorCache<TypeBuilder> cache)
        {
            _options = options.Value;
            _cache = cache;
        }
        public TypeBuilder Create(Type serviceType)
            => _cache.GetOrAdd(
                    serviceType.FullName,
                    () => BuildTypeBuilder(serviceType));
        
        private TypeBuilder BuildTypeBuilder(Type serviceType)
        {
            var builder = (serviceType.IsInterface
                    ? (Func<Type, TypeBuilder>)CreateTypeByInterface
                    : CreateTypeByBaseType)
                .Invoke(serviceType);
            BuildGenericParameters(serviceType, builder);
            return builder;
        }

        private TypeBuilder CreateTypeByBaseType(Type serviceType)
            => CreateModuleBuilder(serviceType.Namespace)
                .DefineType(
                    serviceType.Name.FormatBy(_options.TypeNameFormat),
                    (TypeAttributes)(serviceType.Attributes - TypeAttributes.Abstract),
                    serviceType);
        
        

        private TypeBuilder CreateTypeByInterface(Type serviceType)
                => CreateModuleBuilder(serviceType.Namespace)
                      .DefineType(
                          serviceType.Name
                            .FormatBy(_options.TypeNameFormat)
                                .Condition(name => name.StartsWith("I"))
                                    .IsTrue(name => name.Substring(1, name.Length - 1))
                                    .Or(name => name),
                          (serviceType.Attributes - TypeAttributes.Abstract - TypeAttributes.Interface) | TypeAttributes.BeforeFieldInit,
                          null,
                          new Type[] { serviceType });
        private void BuildGenericParameters(Type serviceType, TypeBuilder builder)
            => (serviceType.IsGenericType
                ? (Action<Type, TypeBuilder>)CreateGenericParameters
                : (_, __) => { })
            .Invoke(serviceType, builder);

        private void CreateGenericParameters(Type serviceType, TypeBuilder builder)
            => builder.DefineGenericParameters(serviceType.GetGenericArguments().Select(x => x.Name).ToArray())
                    .Zip(
                    serviceType.GenericTypeArguments,
                    (b, s) => (b, s))
                    .Each(x =>
                    {
                        x.b.SetGenericParameterAttributes(x.s.GenericParameterAttributes);
                        x.b.SetBaseTypeConstraint(x.s.GetGenericParameterConstraints().FirstOrDefault(y => !y.IsInterface));
                        x.b.SetInterfaceConstraints(x.s.GetGenericParameterConstraints().Where(y => y.IsInterface).ToArray());
                    });
        private ModuleBuilder CreateModuleBuilder(string @namespace)
            => AssemblyBuilder.DefineDynamicAssembly(
                    new AssemblyName(
                        @namespace.FormatBy(_options.NamespaceFormat)),
                        AssemblyBuilderAccess.RunAndCollect)
                .DefineDynamicModule(@namespace
                    .FormatBy(_options.NamespaceFormat,_options.ModuleNameFormat));
        
    }
}
