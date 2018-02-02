using System.Threading.Tasks;
using System;

namespace Core.Lib.Shared.Flow
{
    public interface IPhase<TFlow, TContext> where TFlow : IFlow<TContext>
    {

    }
    public interface IPhase<TContext> : IPhase<IFlow<TContext>,TContext>
    {
        FlowStep Step { get; }
        Task Next(TContext context, Func<Task> next);
    }
}
