using System;

namespace Core.Lib.Sharedd.Helper
{

    public class FuncHelper
    {
        public Func<T> Throw<T>(Exception ex) => () => throw ex;
        public Func<T> Result<T>(T t) => () => t;
        public T Create<T>(T t) => t;
        public Func<T> Factory<T>(Func<T> factory = default)
            where T : class
            => factory ?? Activator.CreateInstance<T>;
    }
}
