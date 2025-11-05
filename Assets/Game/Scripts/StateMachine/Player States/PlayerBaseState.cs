using UnityEngine;
using Core.Movement;

public class PlayerBaseState : IState
{
    protected PlayerController player;
    protected Animator animator;

    protected static readonly int IdleHash = Animator.StringToHash("playerIDLE");
    protected static readonly int MoveHash = Animator.StringToHash("playerMOVE");
    protected static readonly int JumpHash = Animator.StringToHash("playerJUMP");
    protected static readonly int DashHash = Animator.StringToHash("playerDASH");
    protected static readonly int GlideHash = Animator.StringToHash("playerGLIDE");

    protected const float CrossFadeDuration = 0.1f;

    protected PlayerBaseState(PlayerController player, Animator animator)
    {
        this.player = player;
        this.animator = animator;
    }

    public virtual void OnEnter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void OnExit() { }
}
