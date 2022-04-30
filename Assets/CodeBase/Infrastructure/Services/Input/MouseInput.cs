using System;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Services.Input
{
    public class MouseInput : IInputService, ITickable
    {
        public event Action<Vector2> Clicked;
        public event Action<Vector2> Hold;
        public event Action<Vector2> Released;
        
        private Vector2 _inputPosition;
        private TimeService _timeService;

        [Inject]
        private void Construct(TimeService timeService)
        {
            _timeService = timeService;
        }

        public void Tick()
        {
            if (_timeService.IsPaused)
                return;
            
            _inputPosition = UnityEngine.Input.mousePosition;
            
            if (UnityEngine.Input.GetMouseButtonDown(0))
                Clicked?.Invoke(_inputPosition);

            if (UnityEngine.Input.GetMouseButton(0))
                Hold?.Invoke(_inputPosition);

            if (UnityEngine.Input.GetMouseButtonUp(0))
                Released?.Invoke(_inputPosition);
        }
    }
}