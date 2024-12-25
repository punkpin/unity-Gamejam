using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    
    public Vector3 rotationPoint; //旋转时的参考点。用于确定旋转的轴心。
    private float previousTime; //记录上次移动的时间，用于控制方块下落速度
    [Header("Value")]
    [SerializeField]public float fallTime;
    [SerializeField]public static int height = 20; //代表游戏网格的高度和宽度（即游戏区域的大小）。
    [SerializeField]public static int width = 10;
    private static Transform[,] grid = new Transform[width, height];
    [Header("storage")]
    [SerializeField]public BoxCollider2D[] allChildren;

    // Start is called before the first frame update
    void Start()
    {
        fallTime = 1f;
        allChildren = GetAllChildBoxCollider2DsArray();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow)) //方向键左
        {
            transform.position += new Vector3(-1, 0, 0);
            if(!ValidMove())
                transform.position -= new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) //方向键右
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) //方向键上
        {
            //rotate !
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), 90);
            if (!ValidMove())
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
        }


        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))  //方向键下
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                // add 24.12.25 玩家被方块压扁即死亡 by junpaku
                if (GameConst.PLAYER_IS_IN_BLOCK)
                {
                    GameConst.PLAYER_IS_DEAD = true;
                    Debug.Log("Player is killed by cube");
                }

                SetAllChildTrigger();
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                CheckForLines();

                this.enabled = false;
                FindObjectOfType<SpawnTetromino>().NewTetromino();

            }
            previousTime = Time.time;
        }
    }

    void CheckForLines() //检查
    {
        for (int i = height-1; i >= 0; i--)
        {
            if(HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    bool HasLine(int i)
    {
        for(int j = 0; j< width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }

        return true;
    }

    void DeleteLine(int i) 
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if(grid[j,y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }




    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;
        }
    }

    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if(roundedX < 0 || roundedX >= width || roundedY < 0 ||roundedY >= height)
            {
                return false;
            }

            if (grid[roundedX, roundedY] != null)
                return false;
        }

        return true;
    }

    BoxCollider2D[] GetAllChildBoxCollider2DsArray()
    {
       
        int childCount = transform.childCount;
        BoxCollider2D[] childArray = new BoxCollider2D[childCount];
        for (int i = 0; i < childCount; i++)
        {
            childArray[i] = transform.GetChild(i).gameObject.GetComponent<BoxCollider2D>();
        }
        return childArray;
    }

    private void SetAllChildTrigger()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            if (allChildren[i]!=null)
            allChildren[i].isTrigger = false;
        }

    }
}
