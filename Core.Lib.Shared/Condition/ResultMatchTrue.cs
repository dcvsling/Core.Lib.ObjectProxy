using System;
using System.Collections.Generic;

namespace Core.Lib.Sharedd.Condition
{

    public class ResultMatchTrue<T, TNext> : IResultMatch<T, TNext>
    {
        private readonly TNext _next;

        public ResultMatchTrue(TNext next)
        {
            _next = next;
        }

        public TNext Or(Func<T, TNext> mapping)
            => _next;
    }
}
