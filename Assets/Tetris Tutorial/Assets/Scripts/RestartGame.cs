using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
public class RestartGame : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] public GameTimer timer;
    [SerializeField] public TetrisBlock block;
    [SerializeField] public Enemy_Factory enemy;
    [SerializeField] public GameObject[] Start_Prefabs;
    [SerializeField] public GameObject Starts;
    [SerializeField] public TextMeshProUGUI score_text;
    [SerializeField] public TextMeshProUGUI local_score_text;
    [SerializeField] private int local_text;
    [Header("GameObject")]
    [SerializeField] public GameObject Enemys;
    [SerializeField] public GameObject Finish;
    [SerializeField] public GameObject Player;
    private Vector3 player_position = new Vector3(3.87899995f, 2.54060006f, 0);

    // Start is called before the first frame update
    void Start()
    {
        // 设置为窗口化模式
        Screen.fullScreenMode = FullScreenMode.Windowed;

        // 可选：设置窗口大小
        Screen.SetResolution(540, 1080, false);

        local_text = PlayerPrefs.GetInt("HighScore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameConst.PLAYER_IS_DEAD = true;
            Finish_Game();
        }
        if (GameConst.PLAYER_IS_DEAD)
        {
            Finish_Game();
        }
    }

    public void RestartGame_()
    {
        Time.timeScale = 1f;
        GameConst.PLAYER_IS_DEAD = false;
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
            GameConst.BULLET_POOL_IS_NULL = true;
        }
        foreach (Transform child in Enemys.transform)
        {
            // 删除每个子物体
            Destroy(child.gameObject);
        }
        FindObjectOfType<SpawnTetromino>().NewTetromino();
        Instantiate(Player);
        Player.transform.position = player_position;
        enemy.Clear_Level();
        HealthSystem.Instance.hitPoint = 10f;
        Finish.SetActive(false);
    }
    public void StartGame()
    {
        Starts.SetActive(false);
        for (int i = 0; i < Start_Prefabs.Length; i++)
        {
            Start_Prefabs[i].SetActive(true);
        }
        Instantiate(Player);
        Player.transform.position = player_position;

    }
    public void Finish_Game()
    {
        Finish.SetActive(true);
        Time.timeScale = 0f;
        score_text.text = ScoreManager.Score.ToString();
        if (local_text < ScoreManager.Score)
        {
            local_text = ScoreManager.Score;
            PlayerPrefs.SetInt("HighScore", local_text);  // 存储分数，键值为 "HighScore"
            PlayerPrefs.Save();  // 强制保存到磁盘
        }
        local_score_text.text = local_text.ToString();

    }
}
