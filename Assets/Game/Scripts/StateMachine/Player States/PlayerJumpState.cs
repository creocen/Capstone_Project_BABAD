using UnityEngine;
using Core.Movement;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        animator.CrossFade(JumpHash, CrossFadeDuration);
    }

    public override void FixedUpdate()
    {
        player.HandleMovement();
        player.HandleFall();
    }
}
