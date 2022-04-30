using System;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Services.Input
{
    public class TouchInput: IInputService, ITickable
    {
        private Vector2 _inputPosition;
        private Touch _touch;
        public event Action<Vector2> Clicked;
        public event Action<Vector2> Hold;
        public event Action<Vector2> Released;

        public void Tick()
        {
            _touch = UnityEngine.Input.GetTouch(0);
            _inputPosition = _touch.position;
            
            if (_touch.phase == TouchPhase.Began)
                Clicked?.Invoke(_inputPosition);
            if (_touch.phase == TouchPhase.Moved)
                Hold?.Invoke(_inputPosition);
            if (_touch.phase == TouchPhase.Ended)
                Released?.Invoke(_inputPosition);
        }
    }
}