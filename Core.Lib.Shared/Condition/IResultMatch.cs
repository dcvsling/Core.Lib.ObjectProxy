using System;

namespace Core.Lib.Sharedd.Condition
{

    public interface IResultMatch<T,TNext>
    {
        TNext Or(Func<T,TNext> mapping);
    }
}
