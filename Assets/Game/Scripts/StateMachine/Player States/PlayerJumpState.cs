using UnityEngine;
using Core.State_Machine;

namespace Core.Movement
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerMovement player, Animator animator) : base(player, animator) { }

        public override void OnEnter()
        {
            animator.CrossFade(JumpHash, CrossFadeDuration);
        }

        public override void FixedUpdate()
        {
            player.canDashInAir = true;
            player.HandleMovement();
            player.HandleFall();

        }
    }
}
