using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Value")]
    [SerializeField]public float moveSpeed = 5f;  // ���������ƶ����ٶ�
    [SerializeField]public float jumpHeight = 10f;  // ������Ծ�ĸ߶�
    [SerializeField]private float moveInput;  // �洢��ҵ�����ֵ
    [SerializeField]private bool isGrounded;  // �������Ƿ�վ�ڵ�����
    private Rigidbody2D rb;  // �洢��ҵĸ������
    public Transform groundCheck;  // ���ڼ�����λ��
    public LayerMask groundLayer;  // �������ü������ͼ��

    void Start()
    {
        // ��ȡ�������
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ��ȡ�����ƶ������루ֻͨ�� A �� D ����
        if (Input.GetKey(KeyCode.A)) // ���ƶ�
        {
            moveInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D)) // ���ƶ�
        {
            moveInput = 1f;
        }
        else
        {
            moveInput = 0f;  // ���û�а��� A �� D��ֹͣ�ƶ�
        }
        // �����Ծ���ո����W����
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // ����Ƿ�վ�ڵ����ϣ�ͨ�����߼�⣩
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer);
    }

    void FixedUpdate()
    {
        // ����ˮƽ�ƶ�
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
