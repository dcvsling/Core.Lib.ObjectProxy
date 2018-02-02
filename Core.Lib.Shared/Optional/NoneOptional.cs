using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Lib.Sharedd.Optional
{

    public class NoneOptional<T> : Optional<T>
    {
        internal NoneOptional()
        {
        }

        protected override IEnumerable<T> Values => Enumerable.Empty<T>();

        public override IOptional<T> HasDefault(Func<T> @case)
            => HasOne(@case());

        public override IOptional<TNext> HasMany<TNext>(Func<T, IEnumerable<TNext>> @case)
            => HasNone<TNext>();

        public override IOptional<T> Reduce(Func<T, T, T> reduce)
            => this;
    }
}
