namespace Core.Lib.Shared.Tests
{
    public interface ITest
    {
        string Property { get; set; }

        void Action();
        void Action(string a);
        string Func(string a);
    }
}