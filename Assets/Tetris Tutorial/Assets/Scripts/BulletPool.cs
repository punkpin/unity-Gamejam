// add 24.12.27 �ӵ������ by junpaku
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab; // �ӵ�Ԥ����
    public int poolSize = 10;       // ����ش�С
    private Queue<GameObject> bulletPool;

    void Update()
    {
        if (GameConst.BULLET_POOL_IS_NULL)
        {
            bulletPool = new Queue<GameObject>();

            // Ԥ�ȴ����ӵ������뻺���
            for (int i = 0; i < poolSize; i++)
            {
                AddBulletToPool();
            }

            GameConst.BULLET_POOL_IS_NULL = false;
        }
    }

    // ��̬�����ӵ�����������
    private void AddBulletToPool()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.parent = GameObject.Find("Bullets").transform;
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }

    // �ӻ�����л�ȡ�ӵ�
    public GameObject GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            // �����Ϊ�գ�����ѡ��̬�������ӵ�
            GameObject bullet = Instantiate(bulletPrefab);
            return bullet;
        }
    }

    // ���ӵ��Żػ����
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
