using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���˹���
/// </summary>
public class Enemy_Factory : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] public GameObject[] Enemys_Prefabs; //������ֵ�������
    [SerializeField] public Transform[] Create_Positions; //����������ɵ�λ
    [Header("Value")]
    [SerializeField] public int Ini_quantity; //��ʼ����
    [SerializeField] public float Wave_interval;
    private int quantity; //��ǰ�����������
    public static int death_quantity; //��ǰ����������

    // Start is called before the first frame update
    void Start()
    {
        quantity = Ini_quantity;
        death_quantity = 0; //����������Ϊ0;
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
