using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System;

namespace Core.Lib.Sharedd.Helper
{
    using Condition;
    using System.Collections.Generic;

    public static class ConditionHelper
    {
        public static ICondition<T> Condition<T>(this T dict, Func<T, bool> condition)
            => condition(dict) ? ConditionResult<T>.True(dict) : ConditionResult<T>.False(dict);

        public static IEnumerable<ICondition<T>> ConditionSelect<T>(this IEnumerable<T> seq, Func<T, bool> condition)
            => seq.Select(x => x.Condition(condition));

        public static IEnumerable<IResultMatch<T, TNext>> IsTrue<T, TNext>(this IEnumerable<ICondition<T>> conditions, Func<T, TNext> @true)
            => conditions.Select(x => x.IsTrue(@true));

        public static IEnumerable<IResultMatch<T, TNext>> IsFalse<T, TNext>(this IEnumerable<ICondition<T>> conditions, Func<T, TNext> @false)
            => conditions.Select(x => x.IsFalse(@false));

        public static IEnumerable<TNext> Or<T, TNext>(this IEnumerable<IResultMatch<T, TNext>> conditions, Func<T, TNext> @or)
            => conditions.Select(x => x.Or(@or));
    }
}
