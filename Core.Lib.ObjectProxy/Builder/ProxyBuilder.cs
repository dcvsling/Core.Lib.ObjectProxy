using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using System.Reflection;
using System;
using System.Reflection.Emit;
using Core.Lib.ObjectProxy.Abstractions;
using Core.Lib.ObjectProxy.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Lib.ObjectProxy.Builder
{
    internal class ProxyBuilder : IProxyBuilder
    {
        private readonly ITypeProxyBuilder _type;
        private readonly IPropertyProxyBuilder _property;
        private readonly IMethodProxyBuilder _method;
        private readonly IOptionsMonitorCache<Type> _cache;
        private readonly IConstructorProxyBuilder _constructor;
        private readonly ServiceProxyOptions _options;
        private readonly IOptionsMonitorCache<IBuilderContainer> _container;

        public ProxyBuilder(
            ITypeProxyBuilder type,
            IPropertyProxyBuilder property,
            IMethodProxyBuilder method,
            IConstructorProxyBuilder constructor,
            IOptionsMonitorCache<Type> cache,
            IOptionsMonitorCache<IBuilderContainer> container,
            IOptions<ServiceProxyOptions> options)
        {
            _type = type;
            _method = method;
            _property = property;
            _cache = cache;
            _constructor = constructor;
            _options = options.Value;
            _container = container;
        }
        
        public Type Build(ServiceDescriptor descriptor)
            => _cache.GetOrAdd(
                    descriptor.ServiceType.FullName,
                    () => CreateType(descriptor));

        private Type CreateType(ServiceDescriptor descriptor)
        {
            var serviceType = descriptor.ServiceType;
            var builder = _type.Create(serviceType);
            _container.GetOrAdd(serviceType.FullName, () => new ProxyBuilderContainer())
                .DefindedFields.Add(builder.DefineField(_options.InputFieldName, serviceType, FieldAttributes.Private | FieldAttributes.InitOnly));

            CreateConstructor(descriptor, builder);
            CreateProperty(serviceType, builder);
            CreateMethod(serviceType, builder);
            return builder.CreateTypeInfo();
        }
        private void CreateProperty(Type serviceType, TypeBuilder builder)
            => _property.Create(serviceType, builder);
        private void CreateMethod(Type serviceType, TypeBuilder builder)
            => _method.Create(serviceType, builder);
        private void CreateConstructor(ServiceDescriptor descriptor, TypeBuilder builder)
            => _constructor.Create(descriptor, builder);
    }
}
