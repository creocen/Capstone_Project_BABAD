using UnityEngine;
using Core.State_Machine;

namespace Core.Movement
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerMovement player, Animator animator) : base(player, animator) { }

        public override void OnEnter()
        {
            animator.CrossFade(IdleHash, 0.1f);
        }

        public override void FixedUpdate()
        {
            player.HandleMovement();
        }
    }
}
