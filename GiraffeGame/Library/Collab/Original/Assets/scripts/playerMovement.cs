using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] int coyoteFrames = 0;
    [SerializeField] bool canJump = true;
    [SerializeField] bool jumpDone = false;
    bool isFacingRight = true;
    bool grounded = true;
    // Movement along the x axis from given input
    float hMovement = 0;
    // Movement along the y axis from given input
    float vMovement = 0;
    Rigidbody2D playerRB;
    float playerX;
    float playerY;
    const float jumpForce = 400.0f * 2.5f;
    [SerializeField] float moveSpeed = 40.0f;
    Vector2 m_velocity = Vector2.zero;
    [Range(0, .3f)] [SerializeField] float movementSmoothing = .05f;
    private SpriteRenderer playerSR;
    private bool lockPlayer;
    Animator pAnimator;
    List<bool> animConstraints;
    setAnimBools setAB;

    void Start()
    {
        setAB = gameObject.GetComponent<setAnimBools>();
        animConstraints = new List<bool>();
        lockPlayer = true;
        playerX = transform.position.x;
        playerY = transform.position.y;
        playerRB = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();
        pAnimator = GetComponent<Animator>();
    }
    public void lockThem()
    {
        lockPlayer = true;
    }
    public void unlockThem()
    {
        lockPlayer = false;
    }
    public bool getLock()
    {
        return lockPlayer;
    }

    void dropDown()
    {
        // Check the certain layers
        Collider2D dropColl = Physics2D.OverlapCircle(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), 2, LayerMask.GetMask("Platforms"));
        // Get collider circle
        if (dropColl)
            Debug.Log(dropColl.name);
        // Get objects within radius
        GameObject platform = dropColl.gameObject;
        // If it is a step in front of a window, turn the collider off or set it to a trigger
        // TODO: Make trigger code for platforms
        platform.GetComponent<Collider2D>().isTrigger = true;
    }

    void jump()
    {
        // jump transitions: idle->jump, run->jump

        setAB.setTrue("startJumping");
        setAB.setAllFalse();
        playerRB.AddForce(new Vector2(0, jumpForce));
        jumpDone = true;
    }

    void updatePlayerPos()
    {
        playerX = transform.position.x;
        playerY = transform.position.y;
    }

    //Problem with the coyote time
    //In those 6 frames the player can jump as many times as they want

    void checkForGround()
    {

        coyoteTime(coyoteFrames);
        // If ground is detected under the player then they are grounded and not jumping
        if (Physics2D.OverlapCircle(new Vector2(playerX, playerY - 0.2f), 1.0f, LayerMask.GetMask("Walkable", "Platforms")))
        {
            grounded = true;
            coyoteFrames = 0;
            jumpDone = false;
        }

        // If there is no ground under the player then they are jumping/falling
        else
        {
            grounded = false;
            coyoteFrames += 1;
        }

    }

    void getInput()
    {
        bool hasJumped = Input.GetButtonDown("Jump");
        float verticalInput = Input.GetAxisRaw("Vertical");
        hMovement = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if((hMovement > 0  && !isFacingRight))
        {
            isFacingRight = !isFacingRight;
            flip(isFacingRight);
            // running transitions: jump->run, repair->run, idle->run
            pAnimator.SetBool("startRunning", true);
            pAnimator.SetBool("startRepairing", false);
            pAnimator.SetBool("startJumping", false);
            pAnimator.SetBool("startThrowing", false);
        }

        else if(hMovement < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            flip(isFacingRight);
            pAnimator.SetBool("startRunning", true);
            pAnimator.SetBool("startRepairing", false);
            pAnimator.SetBool("startJumping", false);
            pAnimator.SetBool("startThrowing", false);
        }

        // if the jump button is hit (space, A button)
        //removed !jumpDone
        if (hasJumped && canJump &&  grounded && verticalInput >= 0)
        {
            jump();
            canJump = false;
        }

        // On controller will need to press up and the jump button at the same time
        if (grounded && (verticalInput < 0 && hasJumped))
        {
            dropDown();
        }


    }

    void flip(bool isFlipped)
    {

        playerSR.flipX = !isFlipped;

    }

    void coyoteTime(int cFrames)
    {
        if (cFrames > 6)
        {
            canJump = false;
        }
        else if (cFrames <= 6)
        {
            canJump = true;
        }
    }

    void Update()
    {
        if (!lockPlayer)
        {
            getInput();
            checkForGround();
            updatePlayerPos();
        }
        
    }
    void fixedMove(float fixedHMove, bool isJumpAllowed)
    {
        Vector2 targetVelocity = new Vector2(fixedHMove * 10.0f, playerRB.velocity.y);
        playerRB.velocity = Vector2.SmoothDamp(playerRB.velocity, targetVelocity, ref m_velocity, movementSmoothing);
    }

    public void fixedDamage(Rigidbody2D rb)
    {
        rb.AddForce(new Vector2(-400.0f, 400.0f));
    }

    private void FixedUpdate()
    {
        if (!lockPlayer)
        {
            fixedMove(hMovement * Time.fixedDeltaTime, canJump);
        }
        
    }
}
