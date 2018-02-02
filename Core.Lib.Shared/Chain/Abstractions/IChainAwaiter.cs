
using System;
using System.Threading.Tasks;

namespace Core.Lib.Chain
{
    public interface IChainAwaiter<T>
    {
        IChainAwaiter<TNext> Then<TNext>(Func<T, Task<TNext>> next);
        IChainAwaiter<TNext> Then<TNext>(Func<T, TNext> next);
        Task<T> Result { get; }
    }
}
