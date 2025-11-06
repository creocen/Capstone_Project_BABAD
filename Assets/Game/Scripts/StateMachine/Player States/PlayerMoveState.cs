using UnityEngine;
using Core.State_Machine;

namespace Core.Movement
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerMovement player, Animator animator) : base(player, animator) { }

        public override void OnEnter()
        {
            animator.CrossFade(MoveHash, CrossFadeDuration);
        }

        public override void FixedUpdate()
        {
            player.HandleMovement();
        }
    }
}
