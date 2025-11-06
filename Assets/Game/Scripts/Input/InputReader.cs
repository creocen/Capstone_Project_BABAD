using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.PlayerInput
{
    public class InputReader : MonoBehaviour, PlayerInputActions.IPlayerActions
    {
        public event Action JumpPressed;
        public event Action JumpReleased;
        public event Action DoubleJumpPressed;
        public event Action DashPressed;
        public event Action GlidePressed;
        public event Action GlideReleased;
        public event Action InteractPressed;

        public float MoveInput { get; private set; }
        public bool isJumpReleased { get; private set; }

        PlayerInputActions inputActions;

        void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerInputActions();
                inputActions.Player.SetCallbacks(this);
            }
            inputActions.Player.Enable();
        }

        void OnDisable()
        {
            if (inputActions != null)
            {
                inputActions.Player.Disable();
            }
        }

        void OnDestroy()
        {
            if (inputActions != null)
            {
                inputActions.Player.Disable();
                inputActions.Dispose();
            }
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            MoveInput = input.x;
        }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                JumpPressed?.Invoke();
                isJumpReleased = false;
            }
            if (ctx.canceled)
            {
                JumpReleased?.Invoke();
                isJumpReleased = true;
            }
        }

        public void OnDoubleJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) DoubleJumpPressed?.Invoke();
        }

        public void OnDash(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) DashPressed?.Invoke();
        }

        public void OnGlide(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                GlidePressed?.Invoke();
            }
            if (ctx.canceled)
            {
                GlideReleased?.Invoke();
            }
        }

        public void OnInteract(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) InteractPressed?.Invoke();
        }
    }

}