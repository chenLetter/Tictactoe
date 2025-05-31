using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ��̬�ࣺ���� AI �����߼�
/// </summary>
public static class GameAI
{    
    /// <summary>
    /// ���ݹ���ȣ���ֹ��ջ����ȼ��㣩,�ѷ�����Ϸ������
    /// </summary>
    public static int maxDepth = 5;

    /// <summary>
    /// ����ȡԤ��������(�޸���߼��㷶Χ�����ǻ����)(̫�ͻ�Ӱ����������ֻ�������ĵ㸽����)
    /// </summary>
    public static int maxGetCandidateNum = 40;

    /// <summary>
    /// ������ӷ�Χ����������������Χ N ����������
    /// </summary>
    public static int maxRange = 2;

    /// <summary>
    /// AI ���
    /// </summary>
    public static PlayerOrder aiPlayer = GameSettingModel.Instance.playerB.playerOrder;

    /// <summary>
    /// ��ң�AI ���֣�
    /// </summary>
    public static PlayerOrder humanPlayer = GameSettingModel.Instance.playerA.playerOrder;

    #region ��ai

    /// <summary>
    /// �������˳��
    /// </summary>
    public static void UpdatePlayerOrder()
    {
        humanPlayer = GameSettingModel.Instance.playerA.playerOrder;
        aiPlayer = GameSettingModel.Instance.playerB.playerOrder;
    }
    /// <summary>
    /// ��ȡ AI �������λ�ã���������������Χ��Χ����������װ����ռ��������
    /// </summary>
    public static Vector2Int GetBestMove(PlayerOrder[,] board)
    {
        //Debug.Log(maxDepth.ToString());
        // ��ȡ��ѡ���
        var candidates = GetCandidateMoves(board, maxRange);
        // ����������ΧһȦ�ĵ�
        var allCandidates = GetAllCandidateMoves(board, 1);

        // 1. �ȿ��Ƿ� AI �Լ���������һ ���� ������ʤ
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

        // 2. ���Ƿ�����������Ͼ�Ӯ ���� �������
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

        // 3. ����ʹ�� minimax ����,�ҵ����ָߵĵ�
        int bestScore = int.MinValue;
        Vector2Int bestMove = new Vector2Int(-1, -1);

        // �����ĵ�ƫ�� ,���ڱ�����ȵ����⣬��ͬ�������£�ai����ԭ��Ҳ������С��ƫ�ƣ�����ʹ�����������������������
        Vector2 center = new Vector2(board.GetLength(0) / 2f, board.GetLength(1) / 2f);
        foreach (var pos in candidates)
        {
            board[pos.x, pos.y] = aiPlayer;
            int score = Minimax(board, 1, false, int.MinValue, int.MaxValue);
            board[pos.x, pos.y] = PlayerOrder.None;

            // ��¼��߷�
            if (score > bestScore)
            {
                bestScore = score;
                bestMove = pos;
            }
            else if(score == bestScore)
            {
                // �������ĵ���� �� ����ѡ��
                float currentDist = Vector2.Distance(pos, center);
                float bestDist = Vector2.Distance(bestMove, center);
                if (currentDist < bestDist)
                {
                    bestMove = pos;
                }
            }
        }

        // �����ʼ�������޺�ѡ�㣬��������
        if (bestMove.x == -1 && bestMove.y == -1)
        {
            bestMove = new Vector2Int(board.GetLength(0) / 2, board.GetLength(1) / 2);
        }

        return bestMove;
    }


    /// <summary>
    /// ��ȡ���е�ǰ������Χ N ���ڵĺ�ѡ���ӵ�  ���� ����Ԥ��
    /// </summary>
    private static List<Vector2Int> GetCandidateMoves(PlayerOrder[,] board, int range)
    {
        HashSet<Vector2Int> candidates = new HashSet<Vector2Int>();
        int width = board.GetLength(0);
        int height = board.GetLength(1);
        // ��ȡ���ĵ㣬�������ĵ����򣬼��ٱ������
        //Vector2 center = new Vector2(width / 2f, height / 2f);

        // �������̣��ҵ��������ӵ�λ��
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // �����λ�������ӣ�������Χ N ���ڵĿ�λ
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

        // �������Ϊ�գ��������ĵ�
        if (candidates.Count == 0)
        {
            candidates.Add(new Vector2Int(width / 2, height / 2));
            return candidates.ToList();
        }

        // ��Ϊ�հ����ĵ���������ȡ���޵���       
        return candidates
        .OrderBy(pos => Mathf.Abs(pos.x - width / 2) + Mathf.Abs(pos.y - height / 2)) // ��� Vector2.Distance
        .Take(maxGetCandidateNum) // �ɵ�������
        .ToList(); 
    }

    /// <summary>
    /// ��ȡ���е�ǰ������Χ N ���ڵĺ�ѡ���ӵ�   ȫ��  ���ڷ�ֹ��һ�ʤ���� �Լ���ʤ
    /// </summary>
    /// <param name="board"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    private static List<Vector2Int> GetAllCandidateMoves(PlayerOrder[,] board, int range)
    {
        HashSet<Vector2Int> candidates = new HashSet<Vector2Int>();
        int width = board.GetLength(0);
        int height = board.GetLength(1);

        // �������̣��ҵ��������ӵ�λ��
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // �����λ�������ӣ�������Χ N ���ڵĿ�λ
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

        // �������Ϊ�գ��������ĵ�
        if (candidates.Count == 0)
        {
            candidates.Add(new Vector2Int(width / 2, height / 2));            
        }

        // �����������е㷵��
        return candidates.ToList();
    }

    /// <summary>
    /// ������ǰ CheckWin() ���������ָ������ж��߼�
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
    /// ���ָ�������Ƿ����� winNum ������
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

    #region ��ai
    /// <summary>
    /// ��ȡ AI �������λ�� �ϵ�
    /// </summary>
    public static Vector2Int OldGetBestMove(PlayerOrder[,] board)
    {
        int bestScore = int.MinValue;
        Vector2Int bestMove = new Vector2Int(-1, -1);

        // �������п��ܵ�λ��
        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                // �����λ��Ϊ��
                if (board[x, y] == PlayerOrder.None)
                {
                    // ���� AI ����
                    board[x, y] = aiPlayer;

                    // ���� Minimax + ��֦
                    int score = Minimax(board, 1, false, int.MinValue, int.MaxValue);

                    // ��������
                    board[x, y] = PlayerOrder.None;

                    // ��¼��ǰ���Ž�
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
    /// Minimax �㷨���� Alpha-Beta ��֦��
    /// </summary>
    private static int Minimax(PlayerOrder[,] board, int depth, bool isMaximizing, int alpha, int beta)
    {
        // �����ֹ������ʤ����������
        PlayerOrder winner = GameModel.CheckWin(board);

        if (winner == aiPlayer)
            return 1000 - depth;  // Խ��Ӯ��Խ��
        else if (winner == humanPlayer)
            return -1000 + depth; // Խ�����Խ��
        else if (depth >= GameSettingModel.Instance.maxDepth || IsBoardFull(board))
            return 0; // ƽ�ֻ�����þ�������0��

        if (isMaximizing)
        {
            // ai ��һغϣ�Ѱ�����ֵ
            int maxEval = int.MinValue;

            // �������п������ӵ�
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x, y] == PlayerOrder.None)
                    {
                        // ���� AI ����
                        board[x, y] = aiPlayer;

                        // ���ظ÷�֧�÷�
                        int eval = Minimax(board, depth + 1, false, alpha, beta);

                        // ��������
                        board[x, y] = PlayerOrder.None;

                        maxEval = Math.Max(maxEval, eval);
                        // �������ֵ
                        alpha = Math.Max(alpha, eval);

                        // ��֦�������ǰ��֧�Ѿ������ܳ��������ڵ��ֵ�����ټ�������
                        // ��֦����������Ҫ��������
                        if (beta <= alpha)
                            return maxEval;
                    }
                }
            }

            return maxEval;
        }
        else
        {
            // ������һغϣ�Ѱ����Сֵ
            int minEval = int.MaxValue;

            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x, y] == PlayerOrder.None)
                    {
                        // ���������������
                        board[x, y] = humanPlayer;

                        int eval = Minimax(board, depth + 1, true, alpha, beta);

                        // ��������
                        board[x, y] = PlayerOrder.None;

                        minEval = Math.Min(minEval, eval);
                        // ������Сֵ
                        beta = Math.Min(beta, eval);

                        // ��֦�������ǰ��֧�Ѿ������ܳ��������ڵ��ֵ�����ټ�������
                        // ��֦����������Ҫ��������
                        if (beta <= alpha)
                            return minEval;
                    }
                }
            }

            return minEval;
        }
    }

    /// <summary>
    /// �жϵ�ǰ�����Ƿ�����
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
    /// ��¡һ���µ����̸����������ƻ�ԭʼ���ݣ�
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