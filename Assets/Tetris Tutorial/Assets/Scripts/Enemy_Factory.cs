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
    [SerializeField] public GameObject Enemys_;
    [Header("Value")]
    [SerializeField] public int Ini_quantity; //初始数量
    [SerializeField] public float Wave_interval;
    private int quantity; //当前生成最大数量
    public static int death_quantity; //当前敌人死亡量
    private int enemys_count = 0;
    private int small_boci = 1;

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
        
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (enemys_count < (int)(quantity *0.2)&&small_boci ==1)
            {
                GameObject enemy = Instantiate(Enemys_Prefabs[Random.Range(0, Enemys_Prefabs.Length)], Create_Positions[Random.Range(0, Create_Positions.Length)].position, Quaternion.identity);
                enemy.transform.parent = Enemys_.transform;
                enemys_count++;
            } 
            if(death_quantity == (int)(quantity *0.2)&& small_boci == 1)
            {
                yield return new WaitForSeconds(5f);
                small_boci++;
            }
            //Debug.Log(death_quantity);
            if (enemys_count < (int)(quantity*0.6)&& small_boci == 2)
            {
                GameObject enemy = Instantiate(Enemys_Prefabs[Random.Range(0, Enemys_Prefabs.Length)], Create_Positions[Random.Range(0, Create_Positions.Length)].position, Quaternion.identity);
                enemy.transform.parent = Enemys_.transform;
                enemys_count++;
            }
            if (death_quantity == (int)(quantity * 0.6)&& small_boci == 2)
            {
                yield return new WaitForSeconds(5f);
                small_boci++;
            }

            if (enemys_count < quantity&& small_boci == 3)
            {
                GameObject enemy = Instantiate(Enemys_Prefabs[Random.Range(0, Enemys_Prefabs.Length)], Create_Positions[Random.Range(0, Create_Positions.Length)].position, Quaternion.identity);
                enemy.transform.parent = Enemys_.transform;
                enemys_count++;
            }

            if (quantity== death_quantity)
            {
                yield return new WaitForSeconds(10f);
                enemys_count = 0;
                death_quantity = 0;
                small_boci = 1;
                quantity = Ini_quantity;
                for (int i=0;i<(int)(ScoreManager.Score / 150) - 1; i++)
                {
                    quantity = (int)(quantity*1.2);
                }
                Debug.Log(quantity);
            }

        }
    }
    public void Clear_Level()
    {
        enemys_count = 0;
        death_quantity = 0;
        small_boci = 1;
        quantity = Ini_quantity;
    }
}
