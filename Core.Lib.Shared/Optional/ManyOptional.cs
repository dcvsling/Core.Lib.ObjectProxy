using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Lib.Sharedd.Optional
{

    public class ManyOptional<T> : Optional<T>
    {
        internal ManyOptional(IEnumerable<T> values)
        {
            Values = values;
        }

        protected override IEnumerable<T> Values { get; }

        public override IOptional<T> HasDefault(Func<T> @case)
            => this;

        public override IOptional<TNext> HasMany<TNext>(Func<T, IEnumerable<TNext>> @case)
            => HasMany(Values.SelectMany(@case));

        public override IOptional<T> Reduce(Func<T, T, T> reduce)
            => HasOne(Values.Aggregate(reduce));
    }
}
