﻿using System;

namespace Core.Lib.Sharedd.Condition
{

    public class ConditionTrue<T> : ICondition<T>
    {
        private readonly T _value;
        public ConditionTrue(T value)
        {
            _value = value;
        }

        public IResultMatch<T, TNext> IsTrue<TNext>(Func<T, TNext> mapping)
            => ResultMatch<T, TNext>.True(mapping(_value));

        public IResultMatch<T, TNext> IsFalse<TNext>(Func<T, TNext> mapping)
            => ResultMatch<T, TNext>.False(_value);
    }
}
