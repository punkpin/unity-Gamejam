// add 24.12.25 游戏计时器 by junpaku
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TMP_Text timerText; // 用于显示时间的 UI 文本
    private float elapsedTime; // 累计的时间

    void Update()
    {
        // 累加时间
        elapsedTime += Time.deltaTime;

        // 格式化时间为 "分:秒"
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        // 更新 UI 文本
        timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }
}
