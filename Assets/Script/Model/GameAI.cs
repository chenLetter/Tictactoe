using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 静态类：负责 AI 决策逻辑
/// </summary>
public static class GameAI
{    
    /// <summary>
    /// 最大递归深度（防止爆栈或过度计算）,已放至游戏设置里
    /// </summary>
    public static int maxDepth = 5;

    /// <summary>
    /// 最大获取预备点数量(修改提高计算范围，但是会变慢)(太低会影响落子区域，只会在中心点附近下)
    /// </summary>
    public static int maxGetCandidateNum = 40;

    /// <summary>
    /// 最大落子范围（仅在已有棋子周围 N 格内搜索）
    /// </summary>
    public static int maxRange = 2;

    /// <summary>
    /// AI 玩家
    /// </summary>
    public static PlayerOrder aiPlayer = GameSettingModel.Instance.playerB.playerOrder;

    /// <summary>
    /// 玩家（AI 对手）
    /// </summary>
    public static PlayerOrder humanPlayer = GameSettingModel.Instance.playerA.playerOrder;

    #region 新ai

    /// <summary>
    /// 更新玩家顺序
    /// </summary>
    public static void UpdatePlayerOrder()
    {
        humanPlayer = GameSettingModel.Instance.playerA.playerOrder;
        aiPlayer = GameSettingModel.Instance.playerB.playerOrder;
    }
    /// <summary>
    /// 获取 AI 最佳落子位置，仅在已有棋子周围范围内搜索，包装了抢占或封堵三连
    /// </summary>
    public static Vector2Int GetBestMove(PlayerOrder[,] board)
    {
        //Debug.Log(maxDepth.ToString());
        // 获取候选落点
        var candidates = GetCandidateMoves(board, maxRange);
        // 所有棋子周围一圈的点
        var allCandidates = GetAllCandidateMoves(board, 1);

        // 1. 先看是否 AI 自己有四连差一 —— 立即获胜
        foreach (var pos in allCandidates)
        {
            board[pos.x, pos.y] = aiPlayer;
            if (CheckWinWithPlayer(board, aiPlayer) == aiPlayer)
            {
                board[pos.x, pos.y] = PlayerOrder.None;
                return pos;
            }
            board[pos.x, pos.y] = PlayerOrder.None;
        }

        // 2. 看是否玩家有人马上就赢 —— 立即封堵
        foreach (var pos in allCandidates)
        {
            board[pos.x, pos.y] = humanPlayer;
            if (CheckWinWithPlayer(board, humanPlayer) == humanPlayer)
            {
                board[pos.x, pos.y] = PlayerOrder.None;
                return pos;
            }
            board[pos.x, pos.y] = PlayerOrder.None;
        }

        // 3. 正常使用 minimax 搜索,找到评分高的点
        int bestScore = int.MinValue;
        Vector2Int bestMove = new Vector2Int(-1, -1);

        // 向中心点偏移 ,由于遍历深度的问题，在同样评分下，ai会向原点也就是左小点偏移，所以使用中心优先来限制这种情况
        Vector2 center = new Vector2(board.GetLength(0) / 2f, board.GetLength(1) / 2f);
        foreach (var pos in candidates)
        {
            board[pos.x, pos.y] = aiPlayer;
            int score = Minimax(board, 1, false, int.MinValue, int.MaxValue);
            board[pos.x, pos.y] = PlayerOrder.None;

            // 记录最高分
            if (score > bestScore)
            {
                bestScore = score;
                bestMove = pos;
            }
            else if(score == bestScore)
            {
                // 距离中心点更近 ， 优先选择
                float currentDist = Vector2.Distance(pos, center);
                float bestDist = Vector2.Distance(bestMove, center);
                if (currentDist < bestDist)
                {
                    bestMove = pos;
                }
            }
        }

        // 如果初始空棋盘无候选点，落在中心
        if (bestMove.x == -1 && bestMove.y == -1)
        {
            bestMove = new Vector2Int(board.GetLength(0) / 2, board.GetLength(1) / 2);
        }

        return bestMove;
    }


    /// <summary>
    /// 获取所有当前棋子周围 N 格内的候选落子点  有限 用于预测
    /// </summary>
    private static List<Vector2Int> GetCandidateMoves(PlayerOrder[,] board, int range)
    {
        HashSet<Vector2Int> candidates = new HashSet<Vector2Int>();
        int width = board.GetLength(0);
        int height = board.GetLength(1);
        // 获取中心点，用于中心点排序，减少遍历深度
        //Vector2 center = new Vector2(width / 2f, height / 2f);

        // 遍历棋盘，找到已有棋子的位置
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 如果该位置有棋子，则检查周围 N 格内的空位
                if (board[x, y] != PlayerOrder.None)
                {
                    for (int dx = -range; dx <= range; dx++)
                    {
                        for (int dy = -range; dy <= range; dy++)
                        {
                            int nx = x + dx;
                            int ny = y + dy;
                            if (nx >= 0 && ny >= 0 && nx < width && ny < height && board[nx, ny] == PlayerOrder.None)
                            {
                                candidates.Add(new Vector2Int(nx, ny));
                            }
                        }
                    }
                }
            }
        }

        // 如果棋盘为空，返回中心点
        if (candidates.Count == 0)
        {
            candidates.Add(new Vector2Int(width / 2, height / 2));
            return candidates.ToList();
        }

        // 不为空按中心点优先排序，取有限点数       
        return candidates
        .OrderBy(pos => Mathf.Abs(pos.x - width / 2) + Mathf.Abs(pos.y - height / 2)) // 替代 Vector2.Distance
        .Take(maxGetCandidateNum) // 可调节数量
        .ToList(); 
    }

    /// <summary>
    /// 获取所有当前棋子周围 N 格内的候选落子点   全部  用于防止玩家获胜，和 自己获胜
    /// </summary>
    /// <param name="board"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    private static List<Vector2Int> GetAllCandidateMoves(PlayerOrder[,] board, int range)
    {
        HashSet<Vector2Int> candidates = new HashSet<Vector2Int>();
        int width = board.GetLength(0);
        int height = board.GetLength(1);

        // 遍历棋盘，找到已有棋子的位置
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 如果该位置有棋子，则检查周围 N 格内的空位
                if (board[x, y] != PlayerOrder.None)
                {
                    for (int dx = -range; dx <= range; dx++)
                    {
                        for (int dy = -range; dy <= range; dy++)
                        {
                            int nx = x + dx;
                            int ny = y + dy;
                            if (nx >= 0 && ny >= 0 && nx < width && ny < height && board[nx, ny] == PlayerOrder.None)
                            {
                                candidates.Add(new Vector2Int(nx, ny));
                            }
                        }
                    }
                }
            }
        }

        // 如果棋盘为空，返回中心点
        if (candidates.Count == 0)
        {
            candidates.Add(new Vector2Int(width / 2, height / 2));            
        }

        // 将附近所所有点返回
        return candidates.ToList();
    }

    /// <summary>
    /// 拷贝当前 CheckWin() 函数并添加指定玩家判断逻辑
    /// </summary>
    private static PlayerOrder CheckWinWithPlayer(PlayerOrder[,] board, PlayerOrder player)
    {
        var setting = GameSettingModel.Instance;
        int width = board.GetLength(0);
        int height = board.GetLength(1);
        int winNum = setting.winNum;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (board[x, y] != player) continue;

                if (CheckDirection(board, x, y, 1, 0, player, winNum) ||
                    CheckDirection(board, x, y, 0, 1, player, winNum) ||
                    CheckDirection(board, x, y, 1, 1, player, winNum) ||
                    CheckDirection(board, x, y, 1, -1, player, winNum))
                {
                    return player;
                }
            }
        }
        return PlayerOrder.None;
    }

    /// <summary>
    /// 检查指定方向是否连续 winNum 个棋子
    /// </summary>
    private static bool CheckDirection(PlayerOrder[,] board, int startX, int startY, int dx, int dy, PlayerOrder player, int winNum)
    {
        int count = 1;
        int x = startX + dx;
        int y = startY + dy;

        while (IsInside(board, x, y) && board[x, y] == player)
        {
            count++;
            x += dx;
            y += dy;
        }

        x = startX - dx;
        y = startY - dy;
        while (IsInside(board, x, y) && board[x, y] == player)
        {
            count++;
            x -= dx;
            y -= dy;
        }

        return count >= winNum;
    }

    private static bool IsInside(PlayerOrder[,] board, int x, int y)
    {
        return x >= 0 && y >= 0 && x < board.GetLength(0) && y < board.GetLength(1);
    }

    #endregion

    #region 老ai
    /// <summary>
    /// 获取 AI 最佳落子位置 老的
    /// </summary>
    public static Vector2Int OldGetBestMove(PlayerOrder[,] board)
    {
        int bestScore = int.MinValue;
        Vector2Int bestMove = new Vector2Int(-1, -1);

        // 遍历所有可能的位置
        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                // 如果该位置为空
                if (board[x, y] == PlayerOrder.None)
                {
                    // 假设 AI 落子
                    board[x, y] = aiPlayer;

                    // 启动 Minimax + 剪枝
                    int score = Minimax(board, 1, false, int.MinValue, int.MaxValue);

                    // 撤销落子
                    board[x, y] = PlayerOrder.None;

                    // 记录当前最优解
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = new Vector2Int(x, y);
                    }
                }
            }
        }

        return bestMove;
    }

    /// <summary>
    /// Minimax 算法（带 Alpha-Beta 剪枝）
    /// </summary>
    private static int Minimax(PlayerOrder[,] board, int depth, bool isMaximizing, int alpha, int beta)
    {
        // 检查终止条件：胜负或最大深度
        PlayerOrder winner = GameModel.CheckWin(board);

        if (winner == aiPlayer)
            return 1000 - depth;  // 越早赢分越高
        else if (winner == humanPlayer)
            return -1000 + depth; // 越晚输分越高
        else if (depth >= GameSettingModel.Instance.maxDepth || IsBoardFull(board))
            return 0; // 平局或深度用尽，返回0分

        if (isMaximizing)
        {
            // ai 玩家回合，寻找最大值
            int maxEval = int.MinValue;

            // 遍历所有可能落子点
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x, y] == PlayerOrder.None)
                    {
                        // 假设 AI 落子
                        board[x, y] = aiPlayer;

                        // 返回该分支得分
                        int eval = Minimax(board, depth + 1, false, alpha, beta);

                        // 撤销落子
                        board[x, y] = PlayerOrder.None;

                        maxEval = Math.Max(maxEval, eval);
                        // 更新最大值
                        alpha = Math.Max(alpha, eval);

                        // 剪枝：如果当前分支已经不可能超过过父节点的值，则不再继续搜索
                        // 剪枝：后续不需要再搜索了
                        if (beta <= alpha)
                            return maxEval;
                    }
                }
            }

            return maxEval;
        }
        else
        {
            // 人类玩家回合，寻找最小值
            int minEval = int.MaxValue;

            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x, y] == PlayerOrder.None)
                    {
                        // 假设人类玩家落子
                        board[x, y] = humanPlayer;

                        int eval = Minimax(board, depth + 1, true, alpha, beta);

                        // 撤销落子
                        board[x, y] = PlayerOrder.None;

                        minEval = Math.Min(minEval, eval);
                        // 更新最小值
                        beta = Math.Min(beta, eval);

                        // 剪枝：如果当前分支已经不可能超过过父节点的值，则不再继续搜索
                        // 剪枝：后续不需要再搜索了
                        if (beta <= alpha)
                            return minEval;
                    }
                }
            }

            return minEval;
        }
    }

    /// <summary>
    /// 判断当前棋盘是否已满
    /// </summary>
    private static bool IsBoardFull(PlayerOrder[,] board)
    {
        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                if (board[x, y] == PlayerOrder.None)
                    return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 克隆一个新的棋盘副本（避免破坏原始数据）
    /// </summary>
    private static PlayerOrder[,] CloneBoard(PlayerOrder[,] board)
    {
        int w = board.GetLength(0), h = board.GetLength(1);
        PlayerOrder[,] clone = new PlayerOrder[w, h];
        System.Array.Copy(board, clone, board.Length);
        return clone;
    }
    #endregion



}