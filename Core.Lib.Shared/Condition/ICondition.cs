using System;

namespace Core.Lib.Sharedd.Condition
{

    public interface ICondition<T>
    {
        IResultMatch<T, TNext> IsTrue<TNext>(Func<T, TNext> mapping);
        IResultMatch<T, TNext> IsFalse<TNext>(Func<T, TNext> mapping);
    }
}
