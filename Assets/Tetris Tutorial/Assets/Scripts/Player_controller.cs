using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Value")]
    [SerializeField]public float moveSpeed = 5f;  
    [SerializeField]public float jumpHeight = 10f;
    [SerializeField]private float moveInput;
    [SerializeField]private bool isGrounded;
    private Rigidbody2D rb; 
    public Transform groundCheck;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
        }
        else
        {
            moveInput = 0f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="fall_damage")
        {
            FindObjectOfType<SpawnTetromino>().NewTetromino();
            Destroy(collision.transform.parent.gameObject);
            HealthSystem.Instance.TakeDamage(5f);

        }
    }
}
