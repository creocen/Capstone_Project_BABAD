using UnityEngine;
using Core.State_Machine;

namespace Core.Movement
{
    public class PlayerDashState : PlayerBaseState
    {
        public PlayerDashState(PlayerMovement player, Animator animator) : base(player, animator) { }

        public override void OnEnter()
        {
            animator.CrossFade(DashHash, 0.01f);
        }

        public override void FixedUpdate()
        {
            player.HandleDashMovement();
        }
    }
}


