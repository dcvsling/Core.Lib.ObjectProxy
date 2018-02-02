using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;
using Core.Lib.ObjectProxy.Builder;
using Core.Lib.ObjectProxy.Abstractions;

namespace Core.Lib.ObjectProxy.Internal
{
    internal class ProxyServiceProvider
    {
        private readonly Action<ServiceProxyOptions> _config;
        private readonly IServiceCollection _services;
        private IServiceProvider _inner;
        public ProxyServiceProvider(IServiceCollection services,Action<ServiceProxyOptions> config)
        {
            _config = config;
            _services = services;
        }

        public Type CreateProxy(ServiceDescriptor descriptor)
        {
            using (var scope = (_inner ?? Build()).CreateScope())
            {
                return scope.ServiceProvider.GetService<IProxyBuilder>().Build(descriptor);
            }
        }

        private IServiceProvider Build()
        {
            var services = new ServiceCollection();
            services.AddOptions()
                .AddScoped<IConfigureOptions<ServiceProxyOptions>, ServiceProxyConfigureOptions>()
                .AddScoped<ITypeProxyBuilder, TypeProxyBuilder>()
                .AddScoped<IPropertyProxyBuilder, PropertyProxyBuilder>()
                .AddScoped<IMethodProxyBuilder, MethodProxyBuilder>()
                .AddScoped<IILBuilder, ILBuilder>()
                .AddScoped<IConstructorProxyBuilder, ConstructorProxyBuilder>()
                .AddScoped<IProxyBuilder, ProxyBuilder>()
                .Configure(_config)
                .Add(_services.FirstOrDefault(x => x.ServiceType == typeof(IConfiguration)));
            _inner = services.BuildServiceProvider();
            return _inner;
        }
    }
}
