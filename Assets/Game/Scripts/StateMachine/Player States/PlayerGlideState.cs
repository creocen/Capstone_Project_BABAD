using UnityEngine;
using Core.Movement;

public class PlayerGlideState : PlayerBaseState
{
    public PlayerGlideState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        animator.CrossFade(GlideHash, CrossFadeDuration);
    }

    public override void FixedUpdate()
    {
        player.HandleMovement();
        player.HandleGlide();
    }
}
