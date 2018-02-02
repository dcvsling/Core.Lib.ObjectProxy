using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Lib.Sharedd.Optional
{

    public abstract class Optional<T> : Optional, IOptional<T>
    {
        protected abstract IEnumerable<T> Values { get; }

        public IEnumerator<T> GetEnumerator()
            => Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => Values.GetEnumerator();
        public abstract IOptional<T> HasDefault(Func<T> @case);
        public abstract IOptional<TNext> HasMany<TNext>(Func<T, IEnumerable<TNext>> @case);
        public abstract IOptional<T> Reduce(Func<T, T, T> reduce);
    }
}
