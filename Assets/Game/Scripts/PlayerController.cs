using System;
using UnityEngine;

namespace Core.Movement
{
    public class PlayerController : MonoBehaviour
    {
        #region Declaring Variables
        [Header("References")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Animator animator;

        [Header("Movement")]
        [SerializeField] private float movementSpeed = 2f;
        [SerializeField] private float acceleration = 4f;
        [SerializeField] private float deceleration = 4f;

        [Header("Dash")]
        [SerializeField] private float dashSpeed = 8f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldown = 1f;
        [SerializeField] private bool canDashInAir = true;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float doubleJumpForce = 8f;
        [SerializeField] private float fallMultiplier = 2.5f;
        [SerializeField] private float lowJumpMultiplier = 2f;

        /*[Header("Wall Check & Jump")]
        [SerializeField] private Transform wallCheck;
        [SerializeField] private float wallCheckDistance = 0.5f;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private float wallSlideSpeed = 2f;
        [SerializeField] private float wallJumpForce = 10f;
        [SerializeField] private Vector2 wallJumpDirection = new Vector2(1, 2);*/


        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Vector2 groundCheckArea = new Vector2(0.8f, 0.1f);
        [SerializeField] private LayerMask groundLayer;

        [Header("Glide")]
        [SerializeField] private float glideGravityScale = 0.3f;
        [SerializeField] private float glideMaxFallSpeed = 2f;

        [Header("Cayote Time & Jump Buffer")]
        [SerializeField] private float coyoteTime = 0.15f;
        [SerializeField] private float jumpBufferTime = 0.2f;

        private float normalGravityScale;
        private float currentMovementSpeed;

        private bool isLookingRight = true;
        private bool isGrounded;
        private bool hasDoubleJump;
        private bool isGliding;
        private bool isDashing;
        private bool canDash = true;

        private float dashTimeCounter;
        private float dashCooldownCounter;
        private int dashDirection;

        private bool isTouchingWall;
        private bool isWallSliding;
        private int wallDirection; // 1 = right wall, -1 = left wall

        private float coyoteTimeCounter;
        private float jumpBufferCounter;
        


        StateMachine stateMachine;
        public Rigidbody2D Body => body;
        #endregion

        void Awake()
        {
            stateMachine = new StateMachine();
            normalGravityScale = body.gravityScale;
            SetupStateMachine();
        }

        #region Enable-Disable Input
        void OnEnable()
        {
            inputReader.JumpPressed += HandleJump;
            inputReader.JumpReleased += HandleJumpRelease;
            inputReader.DoubleJumpPressed += HandleDoubleJump;
            inputReader.DashPressed += HandleDash;
            inputReader.GlidePressed += HandleGlideStart;
            inputReader.GlideReleased += HandleGlideEnd;
        }

        void OnDisable()
        {
            inputReader.JumpPressed -= HandleJump;
            inputReader.JumpReleased -= HandleJumpRelease;
            inputReader.DoubleJumpPressed -= HandleDoubleJump;
            inputReader.DashPressed -= HandleDash;
            inputReader.GlidePressed -= HandleGlideStart;
            inputReader.GlideReleased -= HandleGlideEnd;
        }
        #endregion

        void Update()
        {
            CheckGrounded();
            UpdateTimers();
            FlipSprite();
            stateMachine.Update();
        }

        void FixedUpdate()
        {
            /*if (isDashing)
            {
                HandleDashMovement();
            } else
            {
                HandleMovement();
                HandleFall();
                HandleGlide();
            }*/

            stateMachine.FixedUpdate();// this will handle those physics-related updates
        }


        // TODO : Wall Check & Jump

        private void CheckGrounded()
        {
            bool wasGrounded = isGrounded;
            isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckArea, 0f, groundLayer);
            Debug.Log("is on ground");


            if (isGrounded && !wasGrounded)
            {
                hasDoubleJump = true;
                isGliding = false;
                canDash = true;
            }
        }

        private void UpdateTimers()
        {
            if (isGrounded)
            {
                coyoteTimeCounter = coyoteTime;
            } else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (jumpBufferCounter > 0)
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            if (isDashing)
            {
                dashTimeCounter -= Time.deltaTime;

                if (dashTimeCounter <= 0)
                {
                    isDashing = false;
                    body.gravityScale = normalGravityScale;
                }
            }

            if (dashCooldownCounter > 0)
            {
                dashCooldownCounter -= Time.deltaTime;

                if (dashCooldownCounter <= 0)
                {
                    canDash = true;
                }
            }
        }

        public void HandleMovement()
        {
            float targetSpeed = inputReader.MoveInput * movementSpeed;

            float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
            currentMovementSpeed = Mathf.MoveTowards(currentMovementSpeed, targetSpeed, accelerationRate * Time.deltaTime);

            body.linearVelocity = new Vector2(currentMovementSpeed, body.linearVelocity.y);
        }

        #region Dash
        private void HandleDash()
        {
            bool canPerformDash = canDash && (isGrounded || canDashInAir);

            if (!canPerformDash) return;

            if (Mathf.Abs(inputReader.MoveInput) > 0.01f)
            {
                dashDirection = inputReader.MoveInput > 0 ? 1 : -1;
            } 
            else
            {
                dashDirection = (int)Mathf.Sign(transform.localScale.x);
            }

            isDashing = true;
            isGliding = false;
            dashTimeCounter = dashDuration;
            dashCooldownCounter = dashCooldown;
            canDash = false;

            body.gravityScale = 0f;
        }

        public void HandleDashMovement()
        {
            body.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);
        }
        #endregion

        #region Jump
        private void HandleJump()
        {
            jumpBufferCounter = jumpBufferTime;

            if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
            {
                Jump(jumpForce);
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;
                Debug.Log("is Jumping");
            }
        }

        private void HandleDoubleJump()
        {
            if (!isGrounded && hasDoubleJump)
            {
                Jump(doubleJumpForce);
                hasDoubleJump = false;
            }
        }

        private void Jump(float force)
        {
            isGliding = false;
            body.gravityScale = normalGravityScale;
            body.linearVelocity = new Vector2(body.linearVelocity.x, force);
        }

        private void HandleJumpRelease()
        {
            if (body.linearVelocity.y > 0)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y * 0.5f);
            }
        }
        #endregion 

        public void HandleFall()
        {
            if (isGliding) return;

            if (body.linearVelocity.y < 0)
            {
                body.gravityScale = normalGravityScale * fallMultiplier;
            } /*else if (body.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space)) // need to figure this one out lol
            {
                body.gravityScale = normalGravityScale * lowJumpMultiplier;
            }*/ else
            {
                body.gravityScale = normalGravityScale;
            }
        }
        #region Glide
        private void HandleGlideStart()
        {
            if (!isGrounded && body.linearVelocity.y < 0)
            {
                isGliding = true;
            }
        }

        private void HandleGlideEnd()
        {
            isGliding = false;
            body.gravityScale = normalGravityScale;
        }

        public void HandleGlide()
        {
            if (isGliding)
            {
                body.gravityScale = glideGravityScale;

                if (body.linearVelocity.y < -glideMaxFallSpeed)
                {
                    body.linearVelocity = new Vector2(body.linearVelocity.x, -glideMaxFallSpeed);
                }
            }
        }
        #endregion



        private void FlipSprite()
        {
            if (inputReader.MoveInput > 0.01f)
            {
                transform.localScale = new Vector3(1, 1, 1);
                isLookingRight = true;
            }
            else if (inputReader.MoveInput < -0.01f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                isLookingRight = false;
            }
        }

        void SetupStateMachine()
        {
            var idle = new PlayerIdleState(this, animator);
            var move = new PlayerMoveState(this, animator);
            var jump = new PlayerJumpState(this, animator);
            var dash = new PlayerDashState(this, animator);
            var glide = new PlayerGlideState(this, animator);

            void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, new FuncPredicate(condition));
            void Any(IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, new FuncPredicate(condition));


            // TODO : Recheck these conditions my guy, they are fcked
            At(idle, move, () => isGrounded && isLookingRight);
            At(move, idle, () => isGrounded && !isLookingRight);
            At(idle, move, () => isGrounded && !isLookingRight);
            At(move, idle, () => isGrounded && isLookingRight);

            At(idle, jump, () => isGrounded && inputReader.isJumpPressed);
            At(move, jump, () => isGrounded && inputReader.isJumpPressed);
            At(jump, idle, () => isGrounded && body.linearVelocity.y <= 0);
            At(jump, move, () => isGrounded && body.linearVelocity.y <= 0);

            At(jump, glide, () => !isGrounded && isGliding && body.linearVelocity.y < 0);
            At(glide, jump, () => !isGliding); 
            At(glide, idle, () => isGrounded);

            At(idle, dash, () => isDashing);
            At(move, dash, () => isDashing);
            At(jump, dash, () => isDashing);
            At(dash, idle, () => !isDashing && isGrounded);
            At(dash, jump, () => !isDashing && !isGrounded);
            At(dash, move, () => !isDashing && isGrounded);

            Any(jump, () => !isGrounded && hasDoubleJump && inputReader.isJumpPressed);

            stateMachine.SetState(idle);
        }

        void OnDrawGizmosSelected()
        {
            if (groundCheck == null) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckArea);
        }
    }
}
