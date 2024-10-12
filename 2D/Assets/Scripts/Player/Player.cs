using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.5f;

    private Rigidbody2D rb;
    private float moveInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        moveInput = UserInput.moveInput.x;

        rb.velocity = new Vector2 (moveInput*moveSpeed,rb.velocity.y);
    }
}
