// add 24.12.25 存放游戏常数的地方 by junpaku
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConst
{
    //1×1，2×2，3×3生成的概率分别为5:3:2

    // 子弹速度
    public static float BULLET_SPEED = 10f;

    // 子弹射击间隔
    public static float BULLET_INTERVAL = 0.4f; 

    // 玩家是否在方块内部
    public static bool PLAYER_IS_IN_BLOCK = false;

    // 玩家是否死亡
    public static bool PLAYER_IS_DEAD = false;
}
