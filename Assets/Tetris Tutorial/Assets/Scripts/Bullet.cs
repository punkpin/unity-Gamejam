// add 24.12.25 �ӵ��ű� by junpaku

using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;        // �ӵ��ٶ�
    private int damage;         // �ӵ��˺�
    private Vector2 direction;  // �ӵ�����

    // �����ӵ��ٶ�
    public void SetSpeed(float bulletSpeed)
    {
        speed = bulletSpeed;
    }

    // �����ӵ��˺�
    public void SetDamage(int bulletDamage)
    {
        damage = bulletDamage;
    }

    // �����ӵ�����
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        // �ӵ��ƶ�
        transform.Translate(direction * speed * Time.deltaTime);

        // ����ӵ��Ƿ񳬳���Ļ�߽�
        CheckOutOfBounds();
    }

    void CheckOutOfBounds()
    {
        // ��ȡ�ӵ�����Ļ�ϵ�λ��
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        // ���������Ļ��Χ������
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // ����ӵ��Ƿ���й���
        if (collision.CompareTag("Enemy"))
        {
            // �Թ�������˺�
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // �����ӵ�
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            // �����ϰ���ʱ�����ӵ�
            Destroy(gameObject);
        }
    }
}
