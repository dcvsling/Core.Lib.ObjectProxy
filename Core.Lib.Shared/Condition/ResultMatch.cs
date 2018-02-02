namespace Core.Lib.Sharedd.Condition
{

    public abstract class ResultMatch<T, TNext>
    {
        public static IResultMatch<T, TNext> True(TNext next) => new ResultMatchTrue<T, TNext>(next);
        public static IResultMatch<T, TNext> False(T value) => new ResultMatchFalse<T, TNext>(value);
    }
}
