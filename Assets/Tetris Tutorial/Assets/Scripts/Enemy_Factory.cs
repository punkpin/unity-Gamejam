using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人工厂
/// </summary>
public class Enemy_Factory : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] public GameObject[] Enemys_Prefabs; //储存各种敌人类型
    [SerializeField] public Transform[] Create_Positions; //储存各种生成点位
    [Header("Value")]
    [SerializeField] public int Ini_quantity; //初始数量
    [SerializeField] public float Wave_interval;
    private int quantity; //当前生成最大数量
    public static int death_quantity; //当前敌人死亡量

    // Start is called before the first frame update
    void Start()
    {
        quantity = Ini_quantity;
        death_quantity = 0; //敌人死亡量为0;
        StartCoroutine(Enemys_Create());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Enemys_Create()
    {
        int enemys_count = 0;
        while (true)
        {
            if (enemys_count < quantity)
            {
                GameObject enemy = Instantiate(Enemys_Prefabs[Random.Range(0, Enemys_Prefabs.Length)], Create_Positions[Random.Range(0, Create_Positions.Length)].position, Quaternion.identity);
                enemy.transform.parent = this.transform;
            }
            if(quantity== death_quantity)
            {
                yield return new WaitForSeconds(10f);
                enemys_count = 0;
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
