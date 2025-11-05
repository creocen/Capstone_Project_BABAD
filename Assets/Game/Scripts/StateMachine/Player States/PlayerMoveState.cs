using UnityEngine;
using Core.Movement;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        animator.CrossFade(MoveHash, CrossFadeDuration);
    }

    public override void FixedUpdate()
    {
        player.HandleMovement();
        player.HandleFall();
    }
}
