using UnityEngine;
using Core.State_Machine;
using Core.PlayerInput;

namespace Core.Movement
{
    public class PlayerDashState : PlayerBaseState
    {
        public PlayerDashState(PlayerMovement player, Animator animator, InputReader inputReader) : base(player, animator, inputReader) { }

        public override void OnEnter()
        {
            base.OnEnter();
            animator.CrossFade(DashHash, 0.01f);
        }

        public override void FixedUpdate()
        {
            player.HandleDashMovement();
        }
    }
}


