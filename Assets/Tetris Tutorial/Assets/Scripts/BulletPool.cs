// add 24.12.27 子弹缓存池 by junpaku
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab; // 子弹预制体
    public int poolSize = 10;       // 缓存池大小
    private Queue<GameObject> bulletPool;

    void Update()
    {
        if (GameConst.BULLET_POOL_IS_NULL)
        {
            bulletPool = new Queue<GameObject>();

            // 预先创建子弹并存入缓存池
            for (int i = 0; i < poolSize; i++)
            {
                AddBulletToPool();
            }

            GameConst.BULLET_POOL_IS_NULL = false;
        }
    }

    // 动态生成子弹并加入对象池
    private void AddBulletToPool()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.parent = GameObject.Find("Bullets").transform;
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }

    // 从缓存池中获取子弹
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
            // 如果池为空，可以选择动态创建新子弹
            GameObject bullet = Instantiate(bulletPrefab);
            return bullet;
        }
    }

    // 将子弹放回缓存池
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
