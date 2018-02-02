namespace Core.Lib.Sharedd.Helper
{
    using System;
    public class ActionHelper
    {
        public Action<T> Empty<T>() => _ => { };
        public Action<Exception> Throw => ex => throw ex;
        public T Create<T>(T t) => t;
    }
}
