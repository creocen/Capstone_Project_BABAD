using UnityEngine;
using Core.Movement;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        animator.CrossFade(DashHash, CrossFadeDuration);
    }

    public override void FixedUpdate()
    {
        player.HandleDashMovement();
    }
}
