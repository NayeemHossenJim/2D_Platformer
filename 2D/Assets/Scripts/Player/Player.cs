using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.5f;
    [SerializeField] private GameObject lLag;
    [SerializeField] private GameObject rLag;
    [HideInInspector] public bool isFacingRight;

    private Rigidbody2D rb;
    private Animator anim;
    private float moveInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCheckingDirection();
    }

    private void Update()
    {
        Move();
    }

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
}
