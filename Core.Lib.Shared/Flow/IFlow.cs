using System.Threading.Tasks;
using System.Collections.Immutable;

namespace Core.Lib.Shared.Flow
{
    public interface IFlow<TContext>
    {
        IImmutableList<IPhase<TContext>> Phases { get; }
        Task Run(TContext context);
    }
}
