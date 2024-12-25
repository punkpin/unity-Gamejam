// add 24.12.25 根据方块改变射击类型 by junpaku
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Block1":
                GameConst.BULLET_INTERVAL = 0.4f;
                break;
            case "Block2":
                GameConst.BULLET_INTERVAL = 0.2f;
                break;
            case "Block3":
                GameConst.BULLET_INTERVAL = 0.1f;
                break;
            default:
                GameConst.BULLET_INTERVAL = 0.4f;
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Block1":
                GameConst.BULLET_INTERVAL = 0.4f;
                break;
            case "Block2":
                GameConst.BULLET_INTERVAL = 0.2f;
                break;
            case "Block3":
                GameConst.BULLET_INTERVAL = 0.1f;
                break;
            default:
                GameConst.BULLET_INTERVAL = 0.4f;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Block1":
                GameConst.BULLET_INTERVAL = 0.4f;
                break;
            case "Block2":
                GameConst.BULLET_INTERVAL = 0.4f;
                break;
            case "Block3":
                GameConst.BULLET_INTERVAL = 0.4f;
                break;
            default:
                GameConst.BULLET_INTERVAL = 0.4f;
                break;
        }
    }
}
