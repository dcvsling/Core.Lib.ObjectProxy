using System.Threading.Tasks;
using System.Collections.Immutable;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Lib.Shared.Flow
{
    public class Flow<TContext> : IFlow<TContext>
    {
        public Flow(IEnumerable<IPhase<TContext>> phases)
        {
            phases = phases.OrderBy(x => x.Step);
        }
        public IImmutableList<IPhase<TContext>> Phases { get; }
        public Task Run(TContext context)
            => Phases.Reverse()
                .Aggregate(EndPhase, Reduce)
                .Invoke(context);
        private Func<TContext, Task> EndPhase => _ => Task.CompletedTask;
        private Func<TContext, Task> Reduce(Func<TContext, Task> seed, IPhase<TContext> phase)
            => ctx => phase.Next(ctx, () => seed(ctx));
    }
}
