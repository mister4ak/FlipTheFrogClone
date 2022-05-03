using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace CodeBase.Infrastructure.Services.Input
{
    public class MouseInput : IInputService, IInitializable
    {
        public event Action<Vector2> Clicked;
        public event Action<Vector2> Hold;
        public event Action<Vector2> Released;
        
        private InputAction _position;
    
        public void Initialize()
        {
            PlayerInput inputMap = new PlayerInput();
            inputMap.Enable();
    
            InputAction playerClick = inputMap.Player.Click;
            playerClick.started += OnClicked;
            playerClick.performed += OnPerfomed;
            playerClick.canceled += OnReleased;
            
            _position = inputMap.Player.Position;
        }
    
        private void OnClicked(InputAction.CallbackContext _) => 
            Clicked?.Invoke(_position.ReadValue<Vector2>());
        
        private void OnPerfomed(InputAction.CallbackContext _) => 
            _position.performed += OnHold;

        private void OnHold(InputAction.CallbackContext context) => 
            Hold?.Invoke(context.ReadValue<Vector2>());

        private void OnReleased(InputAction.CallbackContext obj)
        {
            _position.performed -= OnHold;
            Released?.Invoke(_position.ReadValue<Vector2>());
        }
    }
}