// add 24.12.25 ��Ϸ��ʱ�� by junpaku
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TMP_Text timerText; // ������ʾʱ��� UI �ı�
    private float elapsedTime; // �ۼƵ�ʱ��

    void Update()
    {
        // �ۼ�ʱ��
        elapsedTime += Time.deltaTime;

        // ��ʽ��ʱ��Ϊ "��:��"
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        // ���� UI �ı�
        timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }
}
