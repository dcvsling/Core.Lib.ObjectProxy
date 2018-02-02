using System;

namespace Core.Lib.Sharedd.Condition
{

    public abstract class ConditionResult<T>
    {
        public static ICondition<T> True(T value)
           => new ConditionTrue<T>(value);
        public static ICondition<T> False(T value)
            => new ConditionFalse<T>(value);
    }
}
