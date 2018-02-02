
using Core.Lib.ObjectProxy;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ServiceProxyBuilder AddServiceProxy(
            this IServiceCollection services, 
            Action<ServiceProxyOptions> config = default)
            => new ServiceProxyBuilder(services)
                .CreateProxyCore(config ?? Helper.Action.Empty<ServiceProxyOptions>());

        public static ServiceProxyBuilder AddService<TService>(this ServiceProxyBuilder builder)
            where TService : class
            => builder.AddService(typeof(TService));
    }
}
