using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace Core.Lib.ObjectProxy.Internal
{

    public class ServiceProxyConfigureOptions : IConfigureOptions<ServiceProxyOptions>
    {
        private readonly IConfiguration _config;

        public ServiceProxyConfigureOptions(IConfiguration config)
        {
            _config = config;
        }
        public void Configure(ServiceProxyOptions options)
        {
            _config.GetSection("serviceproxy").Bind(options);
        }
    }
}
