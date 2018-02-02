using System;
using System.Collections.Generic;

namespace Core.Lib.Sharedd.Condition
{

    public class ResultMatchFalse<T, TNext> : IResultMatch<T, TNext>
    {

        private readonly T _value;

        public ResultMatchFalse(T value)
        {
            _value = value;
        }

        public TNext Or(Func<T, TNext> mapping)
            => mapping(_value);
    }
}
