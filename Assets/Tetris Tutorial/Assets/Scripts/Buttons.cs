using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Buttons: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private Image Button_Image;
    public Sprite sprite_new;
    public Sprite sprite_start;

    void Start()
    {
        Button_Image = GetComponent<Image>();
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("ButtonHoverResponder script must be attached to a Button");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button != null)
        {
            // 当鼠标进入按钮区域时执行的操作
            Debug.Log("Mouse entered the button area");
            Button_Image.sprite = sprite_new;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button != null)
        {
            // 当鼠标离开按钮区域时执行的操作
            Debug.Log("Mouse exited the button area");
            Button_Image.sprite = sprite_start;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
