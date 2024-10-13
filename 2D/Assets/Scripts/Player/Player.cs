using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.5f;

    [Header("Jump")]
    [SerializeField] private float jumpforcee = 5.5f;
    [SerializeField] private float jumptime = 1f;

    [Header("TurnCheck")]
    [SerializeField] private GameObject lLag;
    [SerializeField] private GameObject rLag;
    [HideInInspector] public bool isFacingRight;

    [Header("Ground Check")]
    [SerializeField] private float extraHeight = 0.25f;
    [SerializeField] private LayerMask whatIsGround;

    private Rigidbody2D rb;
    private Collider2D Coll;
    private Animator anim;
    private RaycastHit2D groundHit;

    private float moveInput;
    private bool isJumping;
    private bool isFalling;
    private float jumpTimeCounter;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Coll = GetComponent<Collider2D>();
        StartCheckingDirection();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    #region Movement Direction
    private void Move()
    {
        moveInput = UserInput.moveInput.x;

        if(moveInput >0 || moveInput <0)
        {
            anim.SetBool("IsWalking", true);

            TurnCheck();
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }

        rb.velocity = new Vector2 (moveInput*moveSpeed,rb.velocity.y);
    }

    private void Jump()
    {
        if(UserInput.controls.Jumping.Jump.WasPressedThisFrame() && IsGrounded())
        {
            isJumping = true;
            jumpTimeCounter = jumptime;
            rb.velocity = new Vector2(rb.velocity.x,jumpforcee);
        }
        if(UserInput.controls.Jumping.Jump.IsPressed())
        {
            if(jumpTimeCounter > 0 && isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforcee);
                jumpTimeCounter -= Time.deltaTime;
            }
        }
        if(UserInput.controls.Jumping.Jump.WasReleasedThisFrame())
        {
            isJumping = false;
        }

        DrawGroundCheck();
    }
    #endregion

    #region Ground Check

    private bool IsGrounded()
    {
        groundHit = Physics2D.BoxCast(Coll.bounds.center, Coll.bounds.size, 0f, Vector2.down,extraHeight,whatIsGround);

        if(groundHit.collider != null) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Debug RayCast

    private void DrawGroundCheck()
    {
        Color rayColor;

        if(IsGrounded())
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(Coll.bounds.center + new Vector3(Coll.bounds.extents.x, 0),Vector2.down * (Coll.bounds.extents.y + extraHeight),rayColor);

        Debug.DrawRay(Coll.bounds.center - new Vector3(Coll.bounds.extents.x, 0), Vector2.down * (Coll.bounds.extents.y + extraHeight), rayColor);

        Debug.DrawRay(Coll.bounds.center - new Vector3(Coll.bounds.extents.x, Coll.bounds.extents.y + extraHeight ) , Vector2.right * (Coll.bounds.extents.x * 2),rayColor);
    }

    #endregion

    #region TurnCheck
    private void StartCheckingDirection()
    {
        if(rLag.transform.position.x > lLag.transform.position.x)
        {
            isFacingRight = true;
        }

        else
        {
            isFacingRight = false;
        }
    }

    private void TurnCheck()
    {
        if(UserInput.moveInput.x > 0 && !isFacingRight)
        {
            Turn();
        }

        else if(UserInput.moveInput.x < 0 && isFacingRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        if(isFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
        
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    }
    #endregion
}
