using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RestartGame : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] public GameTimer timer;
    [SerializeField] public TetrisBlock block;
    [SerializeField] public Enemy_Factory enemy;
    [SerializeField] public GameObject[] Start_Prefabs;
    [SerializeField] public GameObject Starts;
    [Header("GameObject")]
    [SerializeField] public GameObject Enemys;
    private Vector3 player_position = new Vector3(3.87899995f, 2.54060006f, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame_();
        }
    }

    public void RestartGame_()
    {
       
        timer.elapsedTime = 0f;
        ScoreManager.Score = 0;
        foreach (Transform child in GameObject.Find("Blocks").transform)
        {
            // 删除每个子物体
            Destroy(child.gameObject);
        }
        foreach (Transform child in GameObject.Find("Bullets").transform)
        {
            // 删除每个子物体
            Destroy(child.gameObject);
        }
        foreach (Transform child in Enemys.transform)
        {
            // 删除每个子物体
            Destroy(child.gameObject);
        }
        FindObjectOfType<SpawnTetromino>().NewTetromino();
        GameObject.Find("Player").transform.position = player_position;
        enemy.Clear_Level();
        HealthSystem.Instance.hitPoint = 10f;
    }
    public void StartGame()
    {
        Starts.SetActive(false);
        for (int i = 0; i < Start_Prefabs.Length; i++)
        {
            Start_Prefabs[i].SetActive(true);
        }
        
    }
}
