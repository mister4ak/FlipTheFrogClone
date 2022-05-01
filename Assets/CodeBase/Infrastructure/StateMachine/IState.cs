namespace CodeBase.Infrastructure.StateMachine
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