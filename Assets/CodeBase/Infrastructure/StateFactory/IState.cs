using CodeBase.Data;

namespace CodeBase.Infrastructure.StateFactory
{
    public interface IState : IExitableState
    {
        void OnEnter();
    }

    public interface IExitableState
    {
        void OnExit();
    }
}