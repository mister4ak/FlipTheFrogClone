using System;
using System.Collections.Generic;
using CodeBase.Data;

namespace CodeBase.Infrastructure.StateFactory
{
    public class StateMachine
    {
        private IExitableState _currentState;
   
        // private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type,List<Transition>>();
        // private List<Transition> _currentTransitions = new List<Transition>();
        //
        // private static List<Transition> EmptyTransitions = new List<Transition>(0);
        //
        // public void ChangeState()
        // {
        //     Transition transition = GetTransition();
        //     if (transition != null) 
        //         SetState(transition.To);
        //     else
        //         throw new NullReferenceException("transition is null");
        // }

        // public void SetState(IState state)
        // {
        //     if (state == _currentState)
        //         return;
        //
        //     _currentState?.OnExit();
        //     _currentState = state;
        //     
        //     state.OnEnter();
        // }

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

        // public void AddTransition(IState from, IState to, Func<bool> predicate)
        // {
        //     if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
        //     {
        //         transitions = new List<Transition>();
        //         _transitions[from.GetType()] = transitions;
        //     }
        //
        //     transitions.Add(new Transition(to, predicate));
        // }
        //
        // private class Transition
        // {
        //     public Func<bool> Condition {get; }
        //     public IState To { get; }
        //
        //     public Transition(IState to, Func<bool> condition)
        //     {
        //         To = to;
        //         Condition = condition;
        //     }
        // }
        //
        // private Transition GetTransition()
        // {
        //     foreach (var transition in _currentTransitions)
        //         if (transition.Condition())
        //             return transition;
        //
        //     return null;
        // }
    }

    
}