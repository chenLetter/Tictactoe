using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




// 游戏逻辑核心
public class GameModel
{
    /// <summary>
    /// 棋盘，3x3格子，记录每格子的归属者
    /// </summary>
    public PlayerOrder[,] Board { get; private set; } = new PlayerOrder[GameSettingModel.Instance.mapWidth, GameSettingModel.Instance.mapHeight];

    /// <summary>
    /// 当前轮到哪个玩家
    /// </summary>
    public PlayerOrder CurrentPlayer { get; private set; } = PlayerOrder.One;

    /// <summary>
    /// 游戏是否结束
    /// </summary>    
    public bool GameOver { get; private set; }
    
    /// <summary>
    /// 重置游戏（清空棋盘，设置玩家为X）
    /// </summary>
    public void ResetGame()
    {
        Board = new PlayerOrder[GameSettingModel.Instance.mapWidth, GameSettingModel.Instance.mapHeight];
        CurrentPlayer = PlayerOrder.One;
        GameOver = false;
    }

    /// <summary>
    /// 尝试在(x,y)格子落子，如果成功返回true
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool MakeMove(int x, int y)
    {
        // 游戏已结束 或 该格已被占用时，不允许落子
        if (GameOver || Board[x, y] != PlayerOrder.None)
            return false;

        // 落子
        Board[x, y] = CurrentPlayer;

        // 判断是否获胜
        PlayerOrder pPlayerWinner = CheckWin(this.Board);
        if (pPlayerWinner != PlayerOrder.None)
        {
            GameOver = true;
        }

        //换下一个人
        SwitchPlayer();

        // 判断是否平局
        if (IsDraw())
        {
            GameOver = true; // 平局也算游戏结束
            CurrentPlayer = PlayerOrder.None; // 平局时没有胜者
        }

        return true;
    }

    /// <summary>
    /// 玩家切换
    /// </summary>
    private void SwitchPlayer()
    {
        CurrentPlayer = (CurrentPlayer == PlayerOrder.One) ? PlayerOrder.Two : PlayerOrder.One;
    }

    /// <summary>
    /// 判断是否有玩家获胜
    /// </summary>
    /// <returns>返回获胜玩家（Player.X 或 Player.O），如果没有胜者返回 Player.None</returns>
    public static PlayerOrder CheckWin(PlayerOrder[,] Board)
    {
        // 获得设置
        GameSettingModel pGameSettingModel = GameSettingModel.Instance;
        for (int x = 0; x < pGameSettingModel.mapWidth; x++)
        {
            for (int y = 0; y < pGameSettingModel.mapHeight; y++)
            {
                // 如果当前格子为空，则跳过
                if (Board[x, y] == PlayerOrder.None)
                    continue;

                PlayerOrder currentPlayer = Board[x, y];

                // 检查四个方向
                if (CheckDirection(Board, x, y, 1, 0, currentPlayer) ||   // 水平
                    CheckDirection(Board, x, y, 0, 1, currentPlayer) ||   // 垂直
                    CheckDirection(Board, x, y, 1, 1, currentPlayer) ||   // 正斜线
                    CheckDirection(Board, x, y, 1, -1, currentPlayer))    // 反斜线
                {
                    return currentPlayer;
                }
            }
        }

        return PlayerOrder.None;  // 如果没有获胜者
    }

    /// <summary>
    /// 检查某个方向上是否有连线的棋子
    /// </summary>
    private static bool CheckDirection(PlayerOrder[,] Board, int x, int y, int dx, int dy, PlayerOrder player)
    {
        // 获得设置
        GameSettingModel pGameSettingModel = GameSettingModel.Instance;
        int count = 0;

        for (int i = 0; i < pGameSettingModel.winNum; i++)
        {
            int newX = x + i * dx;
            int newY = y + i * dy;

            // 检查是否越界
            if (newX < 0 || newX >= pGameSettingModel.mapWidth || newY < 0 || newY >= pGameSettingModel.mapHeight)
                return false;

            // 检查当前位置是否有相同玩家的棋子
            if (Board[newX, newY] != player)
                return false;

            count++;
        }

        // 如果连线的数量等于 winNum，说明该方向上有足够的棋子连续
        return count == pGameSettingModel.winNum;
    }

    /// <summary>
    /// 判断是否平局（所有格子都被占用 且 没有胜者）
    /// </summary>
    /// <returns></returns>
    private bool IsDraw()
    {
        for (int x = 0; x < GameSettingModel.Instance.mapWidth; x++)
        {
            for (int y = 0; y < GameSettingModel.Instance.mapHeight; y++)
            {
                if (Board[x, y] == PlayerOrder.None)
                    return false; // 有空格，不是平局
            }
        }

        // 所有格子都填满，且没有胜利者 => 平局
        return true;
    }

    #region AI下棋
    /// <summary>
    /// 获得一个随机可用的格子坐标 弃用
    /// </summary>
    /// <returns></returns>
    public (int, int) GetRandomAvailableCell()
    {
        List<(int, int)> available = new List<(int, int)>();
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (Board[x, y] == PlayerOrder.None)
                    available.Add((x, y));
            }
        }

        return available[Random.Range(0, available.Count)];
    }
    #endregion
}

// 工具类
public static class GameTools
{
    /// <summary>
    /// 清空所有子对象
    /// </summary>
    /// <param name="transform"></param>
    public static void DestroyChild(this Transform transform)
    {
        // 遍历并销毁所有子对象
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }
    }
}