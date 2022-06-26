using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    private float movement;
    private Rigidbody2D rb;
    private SpriteRenderer playerSpriteRenderer;
    private Animator animator;
    public ParticleSystem dashParticles;
    public ParticleSystem jetpackParticles;
    public ParticleSystem burstParticles;

    [Header("Speed")]
    [SerializeField] private float speed = 10;

    [Header("Jumps")]
    [SerializeField] private float jumpForce = 1;
    [SerializeField] private float jumpTime = 1;
    [SerializeField] private float ascendingJumpClamp = 15;
    [SerializeField] private float descendingJumpClamp = -20;
    private float jumpTimeCounter;
    private bool jumpInput;
    private bool jumpBeingHeld;
    private bool isJumping;
    private bool jumpButtonDown; //redundant, may replace w/ jumpbeingheld

    [Header("Jetpack")]
    [SerializeField] private float jetpackForce = 200;
    [SerializeField] private float jetpackTime = 2;
    [SerializeField] private GameObject jetpackMeter;
    private float currentJetpackTime;
    private float jetpackMeterScale;
    private bool hasReleasedButtonDuringJump;
    private bool isFlying;


    [Header("Burst")]
    [SerializeField] private float jetpackBurstWindow = 0.1f;
    [SerializeField] private float jetpackBurstDuration = 0.1f;
    [SerializeField] private float jetpackBurstMultiplier = 50;
    private float currentBurstWindow;
    private float currentBurstTime;
    private bool canBurst = false;
    private bool isBursting = false;
    private bool hasBurst = false;
    private bool jetpackRefresherUsed = true;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 5;
    [SerializeField] private float dashDuration = 0.1f;
    private float dashTimer;
    private bool dashInput = false;
    private bool isDashing = false;
    private bool canDash = false;
    private bool hasTurnedDuringDash = false;


    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        jetpackMeter.SetActive(false);

        rb = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump")) jumpInput = true;

        if (Input.GetAxisRaw("Jump") == 1) jumpBeingHeld = true;
        else jumpBeingHeld = false;

        if (Input.GetButtonDown("Dash")) dashInput = true;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            //reset jetpack timer only when grounded
            currentJetpackTime = jetpackTime;
            jetpackRefresherUsed = true;
            isFlying = false;
            canBurst = false;
            hasBurst = false;

            hasReleasedButtonDuringJump = false;
            canDash = true;

            animator.SetBool("isGrounded", true);
        } 
        else
        {
            animator.SetBool("isGrounded", false);
        }

        #region ANIMATIONS

        if (movement != 0)
        {
            animator.SetBool("isMoving", true);
        } else
        {
            animator.SetBool("isMoving", false);
        }

        //Flip player when moving l/r
        if (movement < 0)
        {
            playerSpriteRenderer.flipX = true;
        }
        else if (movement > 0)
        {
            playerSpriteRenderer.flipX = false;
        }

        animator.SetFloat("yVelocity", rb.velocity.y);

        //Update jetpack meter
        if (isFlying)
        {
            jetpackMeter.SetActive(true);
            if (jetpackMeter != null)
            {
                jetpackMeterScale = currentJetpackTime / jetpackTime;
                jetpackMeter.transform.localScale = new Vector2(jetpackMeterScale, 1);
            }
        }
        else
        {
            jetpackMeter.SetActive(false);
        }

        #endregion

        #region PARTICLES

        var jetpackEmission = jetpackParticles.emission;
        if (isFlying && rb.velocity.y > 5 && !isBursting)
        {
            jetpackEmission.enabled = true;
        }
        else jetpackEmission.enabled = false;

        var burstEmission = burstParticles.emission;
        if (isBursting)
        {
            burstEmission.enabled = true;
        } else
        {
            burstEmission.enabled = false;
        }

        var dashEmission = dashParticles.emission;
        if (isDashing)
        {
            dashEmission.enabled = true;
        }
        else dashEmission.enabled = false;
        #endregion
    }

    private void FixedUpdate()
    {
        #region MOVEMENT
        if (rb.velocity.y > ascendingJumpClamp)
        {
            rb.velocity = new Vector2(movement * speed * Time.deltaTime, ascendingJumpClamp);
        } else if (rb.velocity.y < descendingJumpClamp)
        {
            rb.velocity = new Vector2(movement * speed * Time.deltaTime, descendingJumpClamp);
        } else
        {
            rb.velocity = new Vector2(movement * speed * Time.deltaTime, rb.velocity.y);
        }
        #endregion

        #region JUMPING
        if (jumpInput)
        {
            Jump();
        }

        //If jump is being held, either extend the current jump height or use the jetpack
        CheckIfJumpButtonHeld();

        //Move the player in a burst upward
        if (isBursting)
        {
            JetpackBurst();
        }

        CheckIfJumpButtonReleased();

        //Set canBurst to true. if the player inputs jump again while canburst is true it does a jetpack burst
        if (currentBurstWindow > 0)
        {
            canBurst = true;
            currentBurstWindow -= Time.deltaTime;
        }
        else
        {
            canBurst = false;
        }

        #endregion

        #region DASHING

        if (dashInput)
        {
            dashInput = false;
            if (canDash)
            {
                if (!isDashing)
                {
                    animator.SetTrigger("dashInput");

                    isJumping = false;
                    hasTurnedDuringDash = playerSpriteRenderer.flipX;

                    isDashing = true;
                    dashTimer = dashDuration;
                    canDash = false;
                }

                if (isDashing)
                {
                    dashTimer = dashDuration;
                    canDash = false;
                }
            }
        }

        if (isDashing)
        {
            if (playerSpriteRenderer.flipX != hasTurnedDuringDash)
            {
                isDashing = false;
                dashTimer = 0;
            }
            //Debug.Log(dashTimer);
            if (dashTimer > 0)
            {
                rb.velocity = new Vector2((playerSpriteRenderer.flipX ? -1 : 1) * dashSpeed * Time.deltaTime, 0);
            }
            else
            {
                isDashing = false;
            }
            dashTimer -= Time.deltaTime;
        }
        #endregion
    }



    public void Kill()
    {
        gameManager.StartRespawnTimer();
        Destroy(this.gameObject);
    }

    public void RefreshDash()
    {
        canDash = true;
    }

    public void RefreshJetpack()
    {
        currentJetpackTime = jetpackTime;
        currentBurstTime = jetpackBurstDuration;
        jetpackRefresherUsed = false;
        hasBurst = false;
    }

    private void Jump()
    {
        jumpInput = false;

        //Jump if grounded
        if (isGrounded && !jumpButtonDown)
        {
            animator.SetTrigger("jumpInput");
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
        }

        //Check if two jump inputs happened in quick succession, then burst
        if (canBurst)
        {
            canBurst = false;
            isFlying = false;
            isBursting = true;
            currentBurstTime = jetpackBurstDuration;
            jetpackRefresherUsed = true;
        }

        //Hover / jetpack if in the air and pressing jump a second time.
        if (hasReleasedButtonDuringJump && !isGrounded && !isBursting && !hasBurst)
        {
            isFlying = true;
        }
        else
        {
            isFlying = false;
        }

        //Set the timer for the jetpack burst for hitting jump in quick succession
        if (!isBursting && !hasBurst)
        {
            currentBurstWindow = jetpackBurstWindow;
        }
    }

    private void UseJetpack()
    {
        if (currentJetpackTime > 0 && jumpBeingHeld)
        {
            rb.velocity = new Vector2(rb.velocity.x, jetpackForce * Time.deltaTime);
            currentJetpackTime -= Time.deltaTime;
        }
        else if (currentJetpackTime < 0)
        {
            isFlying = false;
        }
    }

    private void JetpackBurst()
    {
        if (currentBurstTime > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jetpackBurstMultiplier * (currentJetpackTime / jetpackTime) * Time.deltaTime);
            currentBurstTime -= Time.deltaTime;
        }
        else
        {
            isBursting = false;

            if (!jetpackRefresherUsed)
            {
                currentBurstTime = jetpackBurstDuration;
                hasBurst = false;
            }
            else
            {
                hasBurst = true;
            }
        }
    }


    private void CheckIfJumpButtonHeld()
    {
        if (jumpBeingHeld)
        {
            //Check if the player is jumping
            jumpButtonDown = true;
            if (isJumping)
            {
                if (jumpTimeCounter > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }

            //Check if the player is using jetpack
            if (isFlying)
            {
                UseJetpack();
            }
        }
    }

    private void CheckIfJumpButtonReleased()
    {
        //Check if the jump button was released mid-jump
        if (!jumpBeingHeld)
        {
            hasReleasedButtonDuringJump = true;
            jumpButtonDown = false;
            isJumping = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Killer"))
        {
            Kill();
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            GameManager.instance.SwitchRoom(collision);
        }
    }*/
}
