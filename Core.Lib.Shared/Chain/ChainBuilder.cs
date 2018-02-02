using Core.Lib.Chain;
namespace Microsoft.Extensions.DependencyInjection
{
    public class ChainBuilder
    {
        public ChainBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        internal ChainBuilder AddChainCore()
        {
            Services.AddTransient<BaseChain>();
            return this;
        }
    }
}
