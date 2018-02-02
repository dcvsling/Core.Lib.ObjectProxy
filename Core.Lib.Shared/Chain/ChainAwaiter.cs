using System.Threading.Tasks;

namespace Core.Lib.Chain
{

    internal class ChainAwaiter<T> : IChainAwaiter<T>
    {
        public ChainAwaiter(Task<T> task)
        {
            Result = task;
        }
        public Task<T> Result { get; }

        public IChainAwaiter<TNext> Then<TNext>(System.Func<T, Task<TNext>> next)
            => new ChainAwaiter<TNext>(Result.ContinueWith(t => next(t.Result)).Unwrap());

        public IChainAwaiter<TNext> Then<TNext>(System.Func<T, TNext> next)
            => new ChainAwaiter<TNext>(Result.ContinueWith(t => next(t.Result)));
    }
}
