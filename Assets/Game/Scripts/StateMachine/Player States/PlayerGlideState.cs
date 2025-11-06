using UnityEngine;
using Core.State_Machine;

namespace Core.Movement
{
    public class PlayerGlideState : PlayerBaseState
    {
        public PlayerGlideState(PlayerMovement player, Animator animator) : base(player, animator) { }

        public override void OnEnter()
        {
            animator.CrossFade(GlideHash, 0.01f);
        }

        public override void FixedUpdate()
        {
            player.HandleGlide();
            player.HandleMovement();
        }
    }

}