using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTetromino : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField]public GameObject[] Tetrominoes; //储存各种类型

    void Start()
    {
        NewTetromino();
    }

    public void NewTetromino()
    {
       GameObject Blocks =  Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position, Quaternion.identity);
        Blocks.transform.parent = GameObject.Find("Blocks").transform;
    }
}
