using System;
using System.Threading.Tasks;

namespace Core.Lib.Chain
{
    public class LazyChain<T> : IChain<T>
    {
        private readonly Func<T> _func;
        private readonly T _current;

        public LazyChain(T current) : this(() => current)
        {
            _current = current;
        }
        public LazyChain(Func<T> func)
        {
            _func = func;
        }

        public T Result => _func();

        public IChain<TNext> Then<TNext>(Func<T, TNext> next)
            => new LazyChain<TNext>(() => next(_func()));

        public IChainAwaiter<TNext> Then<TNext>(Func<T, Task<TNext>> next)
            => new ChainAwaiter<TNext>(next(_func()));
    }
}
