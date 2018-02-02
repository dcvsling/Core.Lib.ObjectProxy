using System.Diagnostics;
using System.IO;

namespace Core.Lib.Shared.Tests
{

    public class Test : ITest
    {
        private readonly TextWriter _writer;
        
        public Test(TextWriter writer)
        {
            _writer = writer;
        }
        public string Property { get; set; }
        public void Action()
        {
            _writer.Write("Ok");
        }
        public void Action(string a)
        {
            _writer.Write(a);
        }

        public string Func(string a)
            => a;
    }

    
}
