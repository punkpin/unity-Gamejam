// add 24.12.25 子弹脚本 by junpaku

using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;        // 子弹速度
    private int damage;         // 子弹伤害
    private Vector2 direction;  // 子弹方向

    // 设置子弹速度
    public void SetSpeed(float bulletSpeed)
    {
        speed = bulletSpeed;
    }

    // 设置子弹伤害
    public void SetDamage(int bulletDamage)
    {
        damage = bulletDamage;
    }

    // 设置子弹方向
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        // 子弹移动
        transform.Translate(direction * speed * Time.deltaTime);

        // 检测子弹是否超出屏幕边界
        CheckOutOfBounds();
    }

    void CheckOutOfBounds()
    {
        // 获取子弹在屏幕上的位置
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        // 如果超出屏幕范围则销毁
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 检测子弹是否击中怪物
        if (collision.CompareTag("Enemy"))
        {
            // 对怪物造成伤害
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // 销毁子弹
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            // 碰到障碍物时销毁子弹
            Destroy(gameObject);
        }
    }
}
