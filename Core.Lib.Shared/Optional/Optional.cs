using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Lib.Sharedd.Optional
{
    public abstract class Optional
    {
        public static IOptional<T> HasNone<T>() => new NoneOptional<T>();
        public static IOptional<T> HasOne<T>(T value) => new OneOptional<T>(value);
        public static IOptional<T> HasMany<T>(IEnumerable<T> values) => new ManyOptional<T>(values);
        public static IOptional<T> HasMany<T>(params T[] values) => HasMany(values.AsEnumerable());
    }
}
