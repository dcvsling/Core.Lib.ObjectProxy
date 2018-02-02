using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Lib.Sharedd.Optional
{

    public class OneOptional<T> : Optional<T>
    {
        private readonly T _value;

        internal OneOptional(T value)
        {
            _value = value;
        }

        protected override IEnumerable<T> Values => Enumerable.Empty<T>().Append(_value);

        public override IOptional<T> HasDefault(Func<T> @case)
            => this;

        public override IOptional<TNext> HasMany<TNext>(Func<T, IEnumerable<TNext>> @case)
            => HasMany(@case(_value));

        public override IOptional<T> Reduce(Func<T, T, T> reduce)
            => this;
    }
}
