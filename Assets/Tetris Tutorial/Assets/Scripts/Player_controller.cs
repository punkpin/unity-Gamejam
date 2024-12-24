using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Value")]
    [SerializeField]public float moveSpeed = 5f;  // 控制左右移动的速度
    [SerializeField]public float jumpHeight = 10f;  // 控制跳跃的高度
    [SerializeField]private float moveInput;  // 存储玩家的输入值
    [SerializeField]private bool isGrounded;  // 检测玩家是否站在地面上
    private Rigidbody2D rb;  // 存储玩家的刚体组件
    public Transform groundCheck;  // 用于检测地面位置
    public LayerMask groundLayer;  // 用于设置检测地面的图层

    void Start()
    {
        // 获取刚体组件
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 获取左右移动的输入（只通过 A 和 D 键）
        if (Input.GetKey(KeyCode.A)) // 左移动
        {
            moveInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D)) // 右移动
        {
            moveInput = 1f;
        }
        else
        {
            moveInput = 0f;  // 如果没有按下 A 或 D，停止移动
        }
        // 检测跳跃（空格键或W键）
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // 检测是否站在地面上（通过射线检测）
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer);
    }

    void FixedUpdate()
    {
        // 控制水平移动
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
