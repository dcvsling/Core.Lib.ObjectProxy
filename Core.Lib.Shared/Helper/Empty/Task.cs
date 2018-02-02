using System;
using System.Threading.Tasks;

namespace Core.Lib.Sharedd.Helper
{
    public class TaskHelper
    {
        public Task Action(System.Action action) => Task.Run(action);
        public Task<T> Func<T>(System.Func<T> func) => Task.Run(func);
    }
}
