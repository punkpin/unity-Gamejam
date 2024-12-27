using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemys_Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 5f;  // 子弹速度
    [SerializeField] private float lifetime = 10f;  // 子弹的生命周期
    [SerializeField] public GameObject next; 

    private Vector2 direction;

    void Start()
    {
        Destroy(gameObject, lifetime);  // 设置生命周期，超时后销毁
    }

    public void Init(Vector2 direction)
    {
        this.direction = direction;
    }

    void Update()
    {
        // 移动子弹
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            // 子弹碰到玩家时
            HealthSystem.Instance.TakeDamage(2f);  // 玩家受伤
            GameObject next1 = Instantiate(next);
            next1.transform.position = this.transform.position;
            Destroy(next1, 1f);
            Destroy(gameObject);  // 销毁子弹
            
        }
        if (collision.tag != "Enemy")
        {
            GameObject next1 = Instantiate(next);
            next1.transform.position = this.transform.position;
            Destroy(next1, 1f);
            Destroy(gameObject);  // 销毁子弹
        }
      
        //if (collision.CompareTag("Block1")|| collision.CompareTag("Block2")|| collision.CompareTag("Block3"))
        //{

        //    Destroy(collision.transform.parent.gameObject);
        //    Destroy(gameObject);
        //}
    }
}
