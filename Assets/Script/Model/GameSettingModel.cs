using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 游戏内设置
/// </summary>
public class GameSettingModel
{
    #region 单例
    /// <summary>
    /// 单例
    /// </summary>
    public static GameSettingModel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameSettingModel();
            }
            return _instance;
        }
    }

    private static GameSettingModel _instance;
    #endregion

    /// <summary>
    /// 游戏难度，默认为简单
    /// </summary>
    public int maxDepth = 3;

    // 游戏地图
    /// <summary>
    /// 地图宽度
    /// </summary>
    public int mapWidth = 3;
    /// <summary>
    /// 地图高度
    /// </summary>
    public int mapHeight = 3;
    /// <summary>
    /// 胜利所需连线数
    /// </summary>
    public int winNum = 3;

    /// <summary>
    /// 玩家信息类，用于存储玩家相关数据
    /// </summary>
    public PlayerData playerA = new PlayerData();
    /// <summary>
    /// 玩家信息类，用于存储玩家相关数据
    /// </summary>
    public PlayerData playerB = new PlayerData();

    /// <summary>
    /// 玩家先后手信息 0 先手为A 1 先手为B 2随机
    /// </summary>
    public int nOneTwoSet = 0;
    /// <summary>
    /// 玩家棋子选择信息 0 为X 1 为O 2为随机
    /// </summary>
    public int nOXSet = 0;

    /// <summary>
    /// 设置玩家信息
    /// </summary>
    public void SetPlayerData(PlayerData pPlayerA, PlayerData pPlayerB)
    {
        playerA = pPlayerA;
        playerB = pPlayerB;
    }

    /// <summary>
    /// 根据先手玩家设置玩家信息
    /// </summary>
    /// <param name="pOneTwon"></param>
    /// <param name="pOX"></param>
    public void SetPlayerData(int pOneTwon, int pOX)
    {
        // 记录信息
        nOneTwoSet = pOneTwon;
        nOXSet = pOX;

        int pOneTwoSet = 0;
        switch (pOneTwon)
        {
            case 0:
                pOneTwoSet = 0;
                break;
            case 1:
                pOneTwoSet = 1;
                break;
            case 2:
                pOneTwoSet = Random.Range(0,2);
                break;
        }

        int pOXSet = 0;
        switch (pOX)
        {
            case 0:
                pOXSet = 0;
                break;
            case 1:
                pOXSet = 1;
                break;
            case 2:
                pOXSet = Random.Range(0, 2);
                break;
        }

        Debug.Log($"先后手设置: {pOneTwoSet}, 棋子设置: {pOXSet}");

        // 设置游戏模型的先后手和棋子类型
        PlayerOrder pPlayerOrderA = PlayerOrder.One;
        PlayerOrder pPlayerOrderB = PlayerOrder.Two;
        switch (pOneTwoSet)
        {
            case 0: // 先手
                pPlayerOrderA = PlayerOrder.One;
                pPlayerOrderB = PlayerOrder.Two;
                break;
            case 1: // 后手
                pPlayerOrderA = PlayerOrder.Two;
                pPlayerOrderB = PlayerOrder.One;
                break;
        }
        E_OX pOXA = E_OX.X; // 默认棋子为 X
        E_OX pOXB = E_OX.O; // 默认棋子为 O
        switch (pOXSet)
        {
            case 0: // X
                pOXA = E_OX.X;
                pOXB = E_OX.O;
                break;
            case 1: // O
                pOXA = E_OX.O;
                pOXB = E_OX.X;
                break;
        }
        PlayerData playerA = new PlayerData(pPlayerOrderA, pOXA);
        PlayerData playerB = new PlayerData(pPlayerOrderB, pOXB);
        // 设置玩家数据到游戏设置模型中
        GameSettingModel.Instance.SetPlayerData(playerA, playerB);
    }
}

/// <summary>
/// 玩家信息类
/// </summary>
public class PlayerData
{
    /// <summary>
    /// 出手顺序
    /// </summary>
    public PlayerOrder playerOrder = PlayerOrder.One; // 默认玩家1先手
    /// <summary>
    /// 玩家棋子类型
    /// </summary>
    public E_OX playerOX = E_OX.X; // 默认玩家1为X

    public PlayerData(PlayerOrder pPlayerOrder, E_OX pE_OX)
    {
        playerOrder = pPlayerOrder;
        playerOX = pE_OX;
    }
    public PlayerData()
    {
    }
}

public enum E_GameDifficulty
{
    Easy,   // 简单
    Hard,   // 困难
}
/// <summary>
/// 游戏模式枚举
/// </summary>
public enum GameMode
{
    PvP,
    PvAI
}

/// <summary>
/// 游戏玩家先后类型枚举
/// </summary>
public enum PlayerOrder { None, One, Two }

/// <summary>
/// 玩家落子类型枚举
/// </summary>
public enum E_OX
{
    O, // 玩家O
    X  // 玩家X
}