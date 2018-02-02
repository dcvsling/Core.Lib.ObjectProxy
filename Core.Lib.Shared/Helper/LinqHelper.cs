using System.Collections.Generic;
namespace System.Linq
{
    public static class LinqHelper
    {
        public static void Each<T>(this IEnumerable<T> seq, Action<T> action)
        {
            foreach(var t in seq)
            {
                action(t);
            }
        }
        
        public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> seq, TAccumulate acumulate, Action<TAccumulate, TSource> action)
            => seq.Aggregate(acumulate,Reduce(action));

        public static Func<TSeed,TNext,TSeed> Reduce<TSeed,TNext>(Action<TSeed ,TNext> action)
            => (seed, next) =>
            {
                action(seed,next);
                return seed;
            };

        public static IEnumerable<T> Do<T>(this IEnumerable<T> seq,Action<T> action)
        {
            foreach(var t in seq)
            {
                action(t);
                yield return t;
            }
        }
    }
}
