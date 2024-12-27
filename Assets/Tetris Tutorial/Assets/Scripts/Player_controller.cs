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
    private Animator Player_animator;
    private Rigidbody2D rb; 
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool hasTriggered = false;
    private bool IsHave_Second = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player_animator = GetComponent<Animator>();
        // add 24.12.25 开始自动射击 by junpaku
        StartShooting();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 获取玩家的 SpriteRenderer 组件
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

        if (Input.GetKeyDown(KeyCode.A))
        {
            Player_animator.SetTrigger("IsWalk_left");
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Player_animator.SetTrigger("IsIdle");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Player_animator.SetTrigger("IsWalk_right");
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            Player_animator.SetTrigger("IsIdle");
        }

        if ((Input.GetButtonDown("Jump") && isGrounded)|| (!IsHave_Second)&& Input.GetButtonDown("Jump"))
        {
            Jump();
            IsHave_Second = true;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
        if(isGrounded)
        {
            IsHave_Second = false;
        }
        // add 24.12.25 启动玩家无敌时间 by junpaku
        // 如果玩家处于无敌状态，更新计时器
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
            {
                // 无敌时间结束
                isInvincible = false;
                spriteRenderer.enabled = true; // 恢复玩家的显示
            }
        }

        // add 24.12.25 增加玩家死亡判定 by junpaku
        if (HealthSystem.Instance.hitPoint <= 0)
        {
            GameConst.PLAYER_IS_DEAD = true;
        }

        if (GameConst.PLAYER_IS_DEAD)
        {
            Die();
        }

        FindBulletPool();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    // add 24.12.25 增加玩家死亡函数 by junpaku
    void Die()
    {
        HealthSystem.Instance.hitPoint = 0;
        Destroy(this.gameObject);

        Debug.Log("Game Over");
    }

    // delete 24.12.25 by junpaku
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "fall_damage")
    //    {
    //        Destroy(collision.transform.parent.gameObject);
    //        FindObjectOfType<SpawnTetromino>().NewTetromino();
    //        HealthSystem.Instance.TakeDamage(5f);
    //    }
    //}

    // add 24.12.25 by junpaku
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Block1HitBox" || collision.tag == "Block2HitBox" || collision.tag == "Block3HitBox")
        {
            if (isInvincible)
            {
                return;
            }
            else
            {
                HealthSystem.Instance.TakeDamage(5f);
                StartInvincibility();
            }
            GameConst.PLAYER_IS_IN_BLOCK = true;
        }
    }

    // add 24.12.25 by junpaku
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Block1HitBox" || collision.tag == "Block2HitBox" || collision.tag == "Block3HitBox")
        {
            GameConst.PLAYER_IS_IN_BLOCK = false;
        }
    }

    // add 24.12.25 玩家受伤无敌功能 by junpaku
    #region 玩家受伤无敌功能
    private float invincibleTime = 2f; // 无敌时间（单位：秒）
    private bool isInvincible = false; // 玩家是否处于无敌状态
    private float invincibleTimer = 0f; // 无敌计时器
    private SpriteRenderer spriteRenderer; // 用于控制玩家的视觉效果（例如闪烁效果）
    private void StartInvincibility()
    {
        isInvincible = true;
        invincibleTimer = invincibleTime; // 重置无敌时间
        StartCoroutine(InvincibilityFlash()); // 启动闪烁效果
    }
    private IEnumerator InvincibilityFlash()
    {
        while (invincibleTimer > 0)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // 切换显示和隐藏
            yield return new WaitForSeconds(0.1f); // 每0.1秒切换一次
        }
        spriteRenderer.enabled = true; // 确保无敌时间结束后玩家是可见的
    }
    #endregion

    // add 24.12.25 子弹射击功能 by junpaku
    #region 子弹射击功能
    //public GameObject[] bulletPrefabs; // 不同类型的子弹预制体
    public Transform shootPoint;       // 子弹发射点
    public static float bulletSpeed = 10f;    // 子弹速度

    private int currentBulletIndex = 0;
    private Coroutine shootingCoroutine;

    private BulletPool bulletPool; // 缓存池引用

    // 开始射击
    void StartShooting()
    {
        FindBulletPool();
        if (shootingCoroutine == null)
        {
            shootingCoroutine = StartCoroutine(AutoShoot());
        }
    }

    // 停止射击
    void StopShooting()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    IEnumerator AutoShoot()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(GameConst.BULLET_INTERVAL);
        }
    }

    void Shoot()
    {
        // 实例化子弹
        GameObject bullet = bulletPool.GetBullet();
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bullet.transform.parent = GameObject.Find("Bullets").transform;
        // 设置子弹速度
        float bulletSpeed = GameConst.BULLET_SPEED;
        // 设置子弹伤害
        int bulletDamage = 1;
        // 设置子弹方向
        Vector2 direction = transform.up; // 向上射击

        if (bulletScript != null)
        {
            bulletScript.SetSpeed(bulletSpeed);
            bulletScript.SetDamage(bulletDamage);
            bulletScript.SetDirection(direction);
            bulletScript.Initialize(bulletPool);
        }
        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = shootPoint.rotation;
    }

    // add 24.12.27 增加子弹池对象获取 by junpaku
    void FindBulletPool()
    {
        if (bulletPool == null)
        {
            bulletPool = FindObjectOfType<BulletPool>();
            if (bulletPool == null)
            {
                Debug.Log("获取子弹池失败");
            }
        }
        else
        {
            Debug.Log("子弹池为" + bulletPool);
        }
    }
    // 切换子弹类型
    //void SwitchBullet(int bulletType)
    //{
    //    switch (bulletType)
    //    {
    //        case 0:
    //            currentBulletIndex = 0;
    //            break;
    //        case 1:
    //            currentBulletIndex = 1;
    //            break;
    //        default:
    //            currentBulletIndex = 0;
    //            break;
    //    }
    //}

    //新增策划案：当方块接触玩家时玩家受到5点伤害并且该方块消失
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasTriggered&&(collision.tag=="Block1HitBox"||collision.tag=="Block2HitBox"|| collision.tag == "Block3HitBox"))
        {
            hasTriggered = true;
            Destroy(collision.transform.parent.parent.gameObject);
            HealthSystem.Instance.TakeDamage(5f);
            FindObjectOfType<SpawnTetromino>().NewTetromino();

        }
    }


    #endregion
}
