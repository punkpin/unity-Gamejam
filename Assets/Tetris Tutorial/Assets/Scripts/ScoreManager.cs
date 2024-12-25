using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText; // 用于显示分数的 UI 文本
    public static int Score; // 累计的分数

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = string.Format("Score: {0}", Score);
    }
}
