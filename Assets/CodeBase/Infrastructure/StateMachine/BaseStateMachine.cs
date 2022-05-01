namespace CodeBase.Infrastructure.StateMachine
{
    public class BaseStateMachine
    {
        private IExitableState _currentState;
   
        public void SetState(IState state)
        {
            ChangeState(state);
            state.OnEnter();
        }

        private void ChangeState(IExitableState state)
        {
            if (state == _currentState)
                return;

            _currentState?.OnExit();
            _currentState = state;
        }
    }
}