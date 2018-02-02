
using Core.Lib.Sharedd.Helper;
namespace System
{
    public static class Helper
    {
        public static EmptyHelper Empty = new EmptyHelper();
        public static ActionHelper Action = new ActionHelper();
        public static FuncHelper Func = new FuncHelper();
        public static TaskHelper Task = new TaskHelper();
    }
}
