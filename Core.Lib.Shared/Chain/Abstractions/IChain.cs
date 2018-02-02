using System;
using System.Threading.Tasks;

namespace Core.Lib.Chain
{
    public interface IChain<T>
    {
        T Result { get; }
        IChain<TNext> Then<TNext>(Func<T, TNext> next);
        IChainAwaiter<TNext> Then<TNext>(Func<T, Task<TNext>> next);
    }
}
