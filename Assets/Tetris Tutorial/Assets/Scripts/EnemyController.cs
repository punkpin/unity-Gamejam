﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] public float moveSpeed = 2f;  // 敌人的移动速度
    [SerializeField] public float attackRange = 5f;  // 攻击范围（单位：米）
    [SerializeField] public float attackCooldown = 5f;  // 攻击冷却时间，单位：秒
    [SerializeField] public float attackPowerMultiplier = 1f;  // 攻击力倍率，敌人速度的倍数
    [SerializeField] public bool IsDragon; //是否是火龙
    [SerializeField] public float Shoot_Time; //子弹发射间隔
    private float local_attack = 1;
    [Header("GameObject")]
    [SerializeField] public GameObject bulletPrefab;
    [Header("Movement Range")]
    [SerializeField] private float minX = -5f;  // 左侧移动范围
    [SerializeField] private float maxX = 5f;  // 右侧移动范围

    private bool isPlayerInRange = false;  // 玩家是否在攻击范围内
    private bool isAttacking = false;  // 是否正在攻击
    private Transform player;  // 玩家对象
    private Rigidbody2D rb;  // 敌人的刚体组件
    private float Timer1 = 0; //无敌时间
    private bool Close_move = false;
    [SerializeField]private Vector2 originalPosition;  // 敌人的初始位置

    private float moveDirection = 1f;  // 移动方向（1表示向右，-1表示向左）

    void Start()
    {
        local_attack = attackPowerMultiplier;
        // 获取玩家对象
        player = GameObject.FindWithTag("Player").transform;
        // 获取敌人的刚体组件
        rb = GetComponent<Rigidbody2D>();
        if (IsDragon)
        {
            StartCoroutine(Dragon_shoot());
        }
        else
        {
            StartCoroutine(AttackRoutine());
        }
        if (!IsDragon)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void Update()
    {
        if (Timer1 <= 0.5f)
        {
            Timer1 += Time.deltaTime;
        }
        if (!Close_move)
        {
            this.transform.position += new Vector3(0, -moveSpeed, 0) * Time.deltaTime;
        }
        // 检测玩家是否在攻击范围内
        if (transform.position.y- player.position.y <= attackRange)
        {

            attackRange *= 1.25f;
            isPlayerInRange = true;
            Close_move = true;
            if (originalPosition.x==0&& originalPosition.y == 0)
            {
                originalPosition = transform.position;
            }
        }
        else
        {
            isPlayerInRange = false;
        }

        // 控制敌人左右移动，并确保不超过设置的范围
        if (!isAttacking&& Close_move)
        {
            if (transform.position.x >= maxX) // 达到最大X值，反向移动
            {
                moveDirection = -1f;
                if (!IsDragon)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
            else if (transform.position.x <= minX) // 达到最小X值，反向移动
            {
                moveDirection = 1f;
                if (!IsDragon)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
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
        local_attack = moveSpeed;
        Vector2 attackDirection = (player.position - transform.position).normalized;

        // 发起冲击，敌人向玩家冲击
        rb.velocity = attackDirection * 6f;
        yield return new WaitForSeconds(1f); 

        float returnTime = 0.8f;  // 设定返回时间
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
        local_attack = attackPowerMultiplier;
        isAttacking = false;
    }

    private IEnumerator Dragon_shoot()
    {
        while (true)
        {
            if (isPlayerInRange)
            {
                // 创建子弹实例
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Enemys_Bullet bulletController = bullet.GetComponent<Enemys_Bullet>();
                bullet.transform.parent = GameObject.Find("Bullets").transform;
                // 计算子弹发射的方向，朝向玩家
                Vector2 direction = (player.position - transform.position).normalized;
                bulletController.Init(direction);
                
            }
            yield return new WaitForSeconds(Shoot_Time);
        }

    }

    // 如果玩家进入攻击范围时，启动攻击
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 玩家进入攻击范围
            isPlayerInRange = true;
            if (Timer1 >= 0.5f - 0.01f)
            {
                HealthSystem.Instance.TakeDamage(local_attack);
                Timer1 = 0;
            }
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
            ScoreManager.Score = ScoreManager.Score + 10;
        }
    }

    void Die()
    {
        // 怪物死亡逻辑
        Destroy(gameObject);
        Enemy_Factory.death_quantity++;
    }
    #endregion
}
