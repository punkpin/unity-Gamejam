using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;        // 子弹速度
    private int damage;         // 子弹伤害
    private Vector2 direction;  // 子弹方向
    private BulletPool bulletPool; // 缓存池引用

    public void Initialize(BulletPool pool)
    {
        bulletPool = pool;
    }

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
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        // 如果超出屏幕范围则回收到缓存池
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            bulletPool.ReturnBullet(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // 回收子弹
            bulletPool.ReturnBullet(gameObject);
        }
        else if (collision.CompareTag("Block1") || collision.CompareTag("Block2") || collision.CompareTag("Block3"))
        {
            bulletPool.ReturnBullet(gameObject);
        }
    }
}
