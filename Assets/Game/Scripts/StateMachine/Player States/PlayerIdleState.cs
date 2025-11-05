using UnityEngine;
using Core.Movement;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        animator.CrossFade(IdleHash, CrossFadeDuration);
    }

    public override void FixedUpdate()
    {
        player.Body.linearVelocity = new Vector2(0, player.Body.linearVelocity.y);
        player.HandleFall();
    }
}
