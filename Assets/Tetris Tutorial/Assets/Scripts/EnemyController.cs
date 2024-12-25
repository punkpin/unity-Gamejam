using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float moveSpeed = 3f;  // 敌人的移动速度
    [SerializeField] private float attackRange = 5f;  // 攻击范围（单位：米）
    [SerializeField] private float attackCooldown = 5f;  // 攻击冷却时间，单位：秒
    [SerializeField] private float attackPowerMultiplier = 2f;  // 攻击力倍率，敌人速度的倍数
    [SerializeField] private float local_attack = 1;

    [Header("Movement Range")]
    [SerializeField] private float minX = -5f;  // 左侧移动范围
    [SerializeField] private float maxX = 5f;  // 右侧移动范围

    private bool isPlayerInRange = false;  // 玩家是否在攻击范围内
    private bool isAttacking = false;  // 是否正在攻击
    private Transform player;  // 玩家对象
    private Rigidbody2D rb;  // 敌人的刚体组件
    private Vector2 originalPosition;  // 敌人的初始位置

    private float moveDirection = 1f;  // 移动方向（1表示向右，-1表示向左）

    void Start()
    {
        local_attack = 1;
        // 获取玩家对象
        player = GameObject.FindWithTag("Player").transform;
        // 获取敌人的刚体组件
        rb = GetComponent<Rigidbody2D>();
        // 记录敌人的初始位置
        originalPosition = transform.position;
        StartCoroutine(AttackRoutine());
    }

    void Update()
    {
        // 检测玩家是否在攻击范围内
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
        }

        // 控制敌人左右移动，并确保不超过设置的范围
        if (!isAttacking)
        {
            if (transform.position.x >= maxX) // 达到最大X值，反向移动
            {
                moveDirection = -1f;
            }
            else if (transform.position.x <= minX) // 达到最小X值，反向移动
            {
                moveDirection = 1f;
            }

            // 根据反向移动的方向设置速度
            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackCooldown);

            // 如果玩家在攻击范围内，并且敌人没有正在攻击
            if (isPlayerInRange && !isAttacking)
            {
                // 发起一次攻击
                StartCoroutine(Attack());
            }
        }
    }

    private IEnumerator Attack()
    {
        // 标记敌人正在攻击
        isAttacking = true;
        float originalSpeed = moveSpeed;
        // 攻击力为敌人当前速度的2倍
        local_attack = originalSpeed * attackPowerMultiplier;
        Vector2 attackDirection = (player.position - transform.position).normalized;

        // 发起冲击，敌人向玩家冲击
        rb.velocity = attackDirection * local_attack;
        yield return new WaitForSeconds(1f); 

        float returnTime = 1f;  // 设定返回时间
        Vector2 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < returnTime)
        {
            transform.position = Vector2.Lerp(startPosition, originalPosition, elapsedTime / returnTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保回到原始位置
        transform.position = originalPosition;

        rb.velocity = Vector2.zero;
        isAttacking = false;
    }

    // 如果玩家进入攻击范围时，启动攻击
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 玩家进入攻击范围
            isPlayerInRange = true;
            HealthSystem.Instance.TakeDamage(local_attack);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 玩家离开攻击范围
            isPlayerInRange = false;

        }
    }

    // add 24.12.25 怪物血量、受伤相关 by junpaku
    #region 怪物血量、受伤相关
    public int health = 3; // 怪物的生命值
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 怪物死亡逻辑
        Destroy(gameObject);
    }
    #endregion
}
