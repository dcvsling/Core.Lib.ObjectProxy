namespace Microsoft.Extensions.DependencyInjection
{
    public static class ChainExtensions
    {
        public static ChainBuilder AddChain(this IServiceCollection services)
            => new ChainBuilder(services).AddChainCore();
    }
}
