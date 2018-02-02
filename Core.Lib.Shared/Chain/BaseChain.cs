using System.Threading.Tasks;
namespace Core.Lib.Chain
{
    public class BaseChain
    {
        public IChain<T> StartBy<T>(T current)
            => new LazyChain<T>(current);

        public IChainAwaiter<T> StartBy<T>(Task<T> task)
            => new ChainAwaiter<T>(task);
    }
}
