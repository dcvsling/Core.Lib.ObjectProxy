using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Lib.Sharedd.Optional
{
    public interface IOptional<T> : IEnumerable<T>
    {
        IOptional<T> HasDefault(Func<T> @case);
        IOptional<TNext> HasMany<TNext>(Func<T, IEnumerable<TNext>> @case);
        IOptional<T> Reduce(Func<T, T, T> reduce);
    }
}
