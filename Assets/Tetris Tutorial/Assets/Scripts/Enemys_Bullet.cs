using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemys_Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 5f;  // �ӵ��ٶ�
    [SerializeField] private float lifetime = 10f;  // �ӵ�����������

    private Vector2 direction;

    void Start()
    {
        Destroy(gameObject, lifetime);  // �����������ڣ���ʱ������
    }

    public void Init(Vector2 direction)
    {
        this.direction = direction;
    }

    void Update()
    {
        // �ƶ��ӵ�
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �ӵ��������ʱ
            HealthSystem.Instance.TakeDamage(2f);  // �������
            Destroy(gameObject);  // �����ӵ�
        }
        //if (collision.CompareTag("Block1")|| collision.CompareTag("Block2")|| collision.CompareTag("Block3"))
        //{

        //    Destroy(collision.transform.parent.gameObject);
        //    Destroy(gameObject);
        //}
    }
}