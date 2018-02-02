using System;

namespace Core.Lib.Sharedd.Condition
{
    public class ConditionFalse<T> : ICondition<T>
    {
        private readonly T _value;
        public ConditionFalse(T value)
        {
            _value = value;
        }
        public IResultMatch<T, TNext> IsTrue<TNext>(Func<T, TNext> mapping)
            => ResultMatch<T,TNext>.False(_value);

        public IResultMatch<T, TNext> IsFalse<TNext>(Func<T, TNext> mapping)
            => ResultMatch<T, TNext>.True(mapping(_value));

    }
}
