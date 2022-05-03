using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace CodeBase.Infrastructure.Services.Input
{
    public class MouseInput : IInputService, IInitializable
    {
        private Vector2 _inputPosition;
        public event Action<Vector2> Clicked;
        public event Action<Vector2> Hold;
        public event Action<Vector2> Released;

        private PlayerInput _inputMap;
        private InputAction _position;

        public void Initialize()
        {
            _inputMap = new PlayerInput();
            _inputMap.Enable();

            //_position = _inputMap.Player.Position;
            
            _inputMap.Player.Click.started += OnClicked;
            _inputMap.Player.Click.performed += OnHold;
            _inputMap.Player.Click.canceled += OnReleased;
            
            //_inputMap.Player.Click
        }

        private void OnReleased(InputAction.CallbackContext obj)
        {
            Debug.Log("released");
            Released?.Invoke(obj.ReadValue<Vector2>());
        }

        private void OnHold(InputAction.CallbackContext obj)
        {
            Debug.Log("hold");
            Hold?.Invoke(obj.ReadValue<Vector2>());
        }

        private void OnClicked(InputAction.CallbackContext context)
        {
            Debug.Log("clicked");
            Clicked?.Invoke(context.ReadValue<Vector2>());
        }
    }
    // public class MouseInput : IInputService, ITickable
    // {
    //     private Vector2 _inputPosition;
    //     public event Action<Vector2> Clicked;
    //     public event Action<Vector2> Hold;
    //     public event Action<Vector2> Released;
    //     
    //     
    //     public void Tick()
    //     {
    //         _inputPosition = UnityEngine.Input.mousePosition;
    //
    //         if (UnityEngine.Input.GetMouseButtonDown(0)) 
    //             Clicked?.Invoke(_inputPosition);
    //
    //         if (UnityEngine.Input.GetMouseButton(0))
    //             Hold?.Invoke(_inputPosition);
    //
    //         if (UnityEngine.Input.GetMouseButtonUp(0)) 
    //             Released?.Invoke(_inputPosition);
    //     }
    // }
}