using UnityEngine;

namespace AGDDPlatformer
{
    public class PlayerController : KinematicObject
    {
        [Header("Movement")]
        public float maxSpeed = 7;
        public float jumpSpeed = 7;
        public float jumpDeceleration = 0.5f; // Upwards slow after releasing jump button
        public float cayoteTime = 0.1f; // Lets player jump just after leaving ground
        public float jumpBufferTime = 0.1f; // Lets the player input a jump just before becoming grounded
        public bool canMove = true;
        public float friction = 1;
        public bool isDead;

        [Header("Dash")]
        public float dashSpeedOriginal;
        public float dashSpeed;
        public float dashTime;
        public float dashCooldown;
        public Color canDashColor;
        public Color cantDashColor;
        float lastDashTime;
        public Vector2 dashDirection;
        public bool isDashing;
        public bool canDash;
        bool wantsToDash;
        public bool inOrbRange = false; //sett by the orb manager script, to overite the desired dash direction

        [Header("Audio")]
        public AudioSource source;
        public AudioClip jumpSound;
        public AudioClip dashSound;

        Vector2 startPosition;
        bool startOrientation;

        float lastJumpTime;
        float lastGroundedTime;
        bool canJump;
        bool jumpReleased;
        Vector2 move;
        float defaultGravityModifier;

        SpriteRenderer spriteRenderer;

        Vector2 jumpBoost;

        void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            lastJumpTime = -jumpBufferTime * 2;

            startPosition = transform.position;
            startOrientation = spriteRenderer.flipX;

            defaultGravityModifier = gravityModifier;
        }

        void Update()
        {
            if (isDead)
                return;
            isFrozen = GameManager.instance.timeStopped;

            if (!inOrbRange)
                dashSpeed = dashSpeedOriginal;  //Asignes the player dash speed to it's original when he is not in range of a gem

            /* --- Read Input --- */

            move.x = Input.GetAxisRaw("Horizontal");
            if ((velocity.x > 0 && move.x < 0)
                || (velocity.x < 0 && move.x > 0)
                || (velocity.x <= maxSpeed && velocity.x >= 0)
                || (velocity.x >= maxSpeed * -1 && velocity.x <= 0))

                canMove = true;

            if (gravityModifier < 0)
            {
                move.x *= -1;
            }

            move.y = Input.GetAxisRaw("Vertical");

            if (Input.GetButtonDown("Jump"))
            {
                // Store jump time so that we can buffer the input
                lastJumpTime = Time.time;
            }

            if (Input.GetButtonUp("Jump"))
            {
                jumpReleased = true;
            }
            // Clamp directional input to 8 directions for dash
            Vector2 desiredDashDirection = new Vector2(
                move.x == 0 ? 0 : (move.x > 0 ? 1 : -1),
                move.y == 0 ? 0 : (move.y > 0 ? 1 : -1));
            if (desiredDashDirection == Vector2.zero)
            {
                // Dash in facing direction if there is no directional input;
                desiredDashDirection = spriteRenderer.flipX ? -Vector2.right : Vector2.right;
            }

            desiredDashDirection = desiredDashDirection.normalized;
            desiredDashDirection.x /= 2;

            if (Input.GetButtonDown("Dash"))
            {
                wantsToDash = true;
            }

            /* --- Compute Velocity --- */

            if ((canDash || inOrbRange) && wantsToDash)
            {
                dash(desiredDashDirection, true);
                source.PlayOneShot(dashSound);
            }
            wantsToDash = false;

            if (isDashing)
            {
                velocity = dashDirection * dashSpeed;
                if (Time.time - lastDashTime >= dashTime)
                {
                    isDashing = false;

                    gravityModifier = defaultGravityModifier;
                    if ((gravityModifier >= 0 && velocity.y > 0) ||
                        (gravityModifier < 0 && velocity.y < 0))
                    {
                        velocity.y *= jumpDeceleration;
                    }

                }
            }
            else
            {
                if (isGrounded)
                {
                    // Store grounded time to allow for late jumps
                    lastGroundedTime = Time.time;
                    canJump = true;
                    if (!isDashing && Time.time - lastDashTime >= dashCooldown)
                        canDash = true;
                }

                // Check time for buffered jumps and late jumps
                float timeSinceJumpInput = Time.time - lastJumpTime;
                float timeSinceLastGrounded = Time.time - lastGroundedTime;

                if (canJump && timeSinceJumpInput <= jumpBufferTime && timeSinceLastGrounded <= cayoteTime)
                {
                    velocity.y = Mathf.Sign(gravityModifier) * jumpSpeed;
                    canJump = false;
                    isGrounded = false;

                    source.PlayOneShot(jumpSound);
                }
                else if (jumpReleased)
                {
                    // Decelerate upwards velocity when jump button is released
                    if ((gravityModifier >= 0 && velocity.y > 0) ||
                        (gravityModifier < 0 && velocity.y < 0))
                    {
                        velocity.y *= jumpDeceleration;
                    }
                    jumpReleased = false;
                }
                if (!isDashing)
                    if (isGrounded)
                        velocity.x = Mathf.MoveTowards(velocity.x, 0.0f, friction * 100 * Time.deltaTime);
                    else
                        velocity.x = Mathf.MoveTowards(velocity.x, 0.0f, friction * 10 * Time.deltaTime);
                if (canMove && move.x != 0)
                    velocity.x = move.x * maxSpeed;
                    

                if (isGrounded || (velocity + jumpBoost).magnitude < velocity.magnitude)
                {
                    jumpBoost = Vector2.zero;
                }
                else
                {
                    velocity += jumpBoost;
                    jumpBoost -= jumpBoost * Mathf.Min(1f, Time.deltaTime);
                }
            }

            /* --- Adjust Sprite --- */

            // Assume the sprite is facing right, flip it if moving left
            if (move.x > 0.01f)
            {
                spriteRenderer.flipX = false;
            }
            else if (move.x < -0.01f)
            {
                spriteRenderer.flipX = true;
            }

            spriteRenderer.color = canDash ? canDashColor : cantDashColor;
        }

        public void ResetPlayer()
        {
            transform.position = startPosition;
            spriteRenderer.flipX = startOrientation;

            lastJumpTime = -jumpBufferTime * 2;

            velocity = Vector2.zero;
            isDashing = false;
            canDash = false;
            gravityModifier = defaultGravityModifier;
        }

        public void ResetDash()
        {
            canDash = true;
        }

        //Add a short mid-air boost to the player (unrelated to dash). Will be reset upon landing.
        public void SetJumpBoost(Vector2 jumpBoost)
        {
            this.jumpBoost = jumpBoost;
        }

        public void dash(Vector2 desiredDashDirection, bool normalDash)
        {
            velocity = Vector2.zero;
            canMove = false;
            isDashing = true;
            if (!inOrbRange)
                dashDirection = desiredDashDirection;
            lastDashTime = Time.time;
            if (normalDash)
                canDash = false;
            gravityModifier = 0;
        }

    }
}
