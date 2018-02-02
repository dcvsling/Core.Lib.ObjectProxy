using Core.Lib.Sharedd.Optional;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Core.Lib.ObjectProxy.Internal;
using Core.Lib.Sharedd.Helper;

namespace Core.Lib.ObjectProxy
{
    public class ServiceProxyBuilder
    {
        private ProxyServiceProvider _provider;
        public ServiceProxyBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        internal ServiceProxyBuilder CreateProxyCore(Action<ServiceProxyOptions> config)
        {
            _provider = new ProxyServiceProvider(Services, config);
            return this;
        }

        public ServiceProxyBuilder AddService(Type serviceType)
        {
            var services = Services.Where(x => x.ServiceType == serviceType)
                .ToArray();
            Services.RemoveAll(serviceType);
            services.SelectMany(x => CreateDescriptor(x, _provider.CreateProxy(x)))
                .Each(Services.Add);
            return this;
        }

        private IOptional<ServiceDescriptor> CreateDescriptor(ServiceDescriptor descriptor, Type proxyType)
            => CreateDescriptByFactory(descriptor, proxyType);
        private IOptional<ServiceDescriptor> CreateDescriptByFactory(ServiceDescriptor descriptor, Type proxyType)
           => descriptor.Condition(x => x.ImplementationFactory != null)
               .IsTrue(x => CreateProxyByFactoryDescriptor(x, proxyType))
               .Or(x => CreateDescriptByInstance(x, proxyType));
        private IOptional<ServiceDescriptor> CreateDescriptByInstance(ServiceDescriptor descriptor, Type proxyType)
            => descriptor.Condition(x => x.ImplementationInstance != null)
                .IsTrue(x => CreateProxyByInstanceDescriptor(x, proxyType))
                .Or(x => CreateDescriptByType(x, proxyType));
        private IOptional<ServiceDescriptor> CreateDescriptByType(ServiceDescriptor descriptor, Type proxyType)
            => CreateProxyByTypeDescriptor(descriptor, proxyType);

        private IOptional<ServiceDescriptor> CreateProxyByTypeDescriptor(ServiceDescriptor descriptor,Type proxyType)
            => Optional.HasMany(
                ServiceDescriptor.Describe(descriptor.ServiceType, descriptor.ImplementationType, descriptor.Lifetime),
                ServiceDescriptor.Describe(
                    descriptor.ImplementationType,
                    descriptor.ImplementationType,
                    descriptor.Lifetime));

        private IOptional<ServiceDescriptor> CreateProxyByInstanceDescriptor(ServiceDescriptor descriptor, Type proxyType)
            => Optional.HasOne(
                ServiceDescriptor.Describe(
                descriptor.ServiceType,
                CreateProxyFactory(proxyType,p => descriptor.ImplementationInstance),
                descriptor.Lifetime));

        private IOptional<ServiceDescriptor> CreateProxyByFactoryDescriptor(ServiceDescriptor descriptor, Type proxyType)
            => Optional.HasOne(
                ServiceDescriptor.Describe(
                descriptor.ServiceType,
                CreateProxyFactory(proxyType, descriptor.ImplementationFactory),
                descriptor.Lifetime));

        private Func<IServiceProvider, object> CreateProxyFactory(Type proxyType,Func<IServiceProvider,object> arg)
            => p => ActivatorUtilities.CreateInstance(p, proxyType, arg(p));
    }
}
