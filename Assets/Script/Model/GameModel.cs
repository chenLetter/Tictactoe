using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




// ��Ϸ�߼�����
public class GameModel
{
    /// <summary>
    /// ���̣�3x3���ӣ���¼ÿ���ӵĹ�����
    /// </summary>
    public PlayerOrder[,] Board { get; private set; } = new PlayerOrder[GameSettingModel.Instance.mapWidth, GameSettingModel.Instance.mapHeight];

    /// <summary>
    /// ��ǰ�ֵ��ĸ����
    /// </summary>
    public PlayerOrder CurrentPlayer { get; private set; } = PlayerOrder.One;

    /// <summary>
    /// ��Ϸ�Ƿ����
    /// </summary>    
    public bool GameOver { get; private set; }
    
    /// <summary>
    /// ������Ϸ��������̣��������ΪX��
    /// </summary>
    public void ResetGame()
    {
        Board = new PlayerOrder[GameSettingModel.Instance.mapWidth, GameSettingModel.Instance.mapHeight];
        CurrentPlayer = PlayerOrder.One;
        GameOver = false;
    }

    /// <summary>
    /// ������(x,y)�������ӣ�����ɹ�����true
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool MakeMove(int x, int y)
    {
        // ��Ϸ�ѽ��� �� �ø��ѱ�ռ��ʱ������������
        if (GameOver || Board[x, y] != PlayerOrder.None)
            return false;

        // ����
        Board[x, y] = CurrentPlayer;

        // �ж��Ƿ��ʤ
        PlayerOrder pPlayerWinner = CheckWin(this.Board);
        if (pPlayerWinner != PlayerOrder.None)
        {
            GameOver = true;
        }

        //����һ����
        SwitchPlayer();

        // �ж��Ƿ�ƽ��
        if (IsDraw())
        {
            GameOver = true; // ƽ��Ҳ����Ϸ����
            CurrentPlayer = PlayerOrder.None; // ƽ��ʱû��ʤ��
        }

        return true;
    }

    /// <summary>
    /// ����л�
    /// </summary>
    private void SwitchPlayer()
    {
        CurrentPlayer = (CurrentPlayer == PlayerOrder.One) ? PlayerOrder.Two : PlayerOrder.One;
    }

    /// <summary>
    /// �ж��Ƿ�����һ�ʤ
    /// </summary>
    /// <returns>���ػ�ʤ��ң�Player.X �� Player.O�������û��ʤ�߷��� Player.None</returns>
    public static PlayerOrder CheckWin(PlayerOrder[,] Board)
    {
        // �������
        GameSettingModel pGameSettingModel = GameSettingModel.Instance;
        for (int x = 0; x < pGameSettingModel.mapWidth; x++)
        {
            for (int y = 0; y < pGameSettingModel.mapHeight; y++)
            {
                // �����ǰ����Ϊ�գ�������
                if (Board[x, y] == PlayerOrder.None)
                    continue;

                PlayerOrder currentPlayer = Board[x, y];

                // ����ĸ�����
                if (CheckDirection(Board, x, y, 1, 0, currentPlayer) ||   // ˮƽ
                    CheckDirection(Board, x, y, 0, 1, currentPlayer) ||   // ��ֱ
                    CheckDirection(Board, x, y, 1, 1, currentPlayer) ||   // ��б��
                    CheckDirection(Board, x, y, 1, -1, currentPlayer))    // ��б��
                {
                    return currentPlayer;
                }
            }
        }

        return PlayerOrder.None;  // ���û�л�ʤ��
    }

    /// <summary>
    /// ���ĳ���������Ƿ������ߵ�����
    /// </summary>
    private static bool CheckDirection(PlayerOrder[,] Board, int x, int y, int dx, int dy, PlayerOrder player)
    {
        // �������
        GameSettingModel pGameSettingModel = GameSettingModel.Instance;
        int count = 0;

        for (int i = 0; i < pGameSettingModel.winNum; i++)
        {
            int newX = x + i * dx;
            int newY = y + i * dy;

            // ����Ƿ�Խ��
            if (newX < 0 || newX >= pGameSettingModel.mapWidth || newY < 0 || newY >= pGameSettingModel.mapHeight)
                return false;

            // ��鵱ǰλ���Ƿ�����ͬ��ҵ�����
            if (Board[newX, newY] != player)
                return false;

            count++;
        }

        // ������ߵ��������� winNum��˵���÷��������㹻����������
        return count == pGameSettingModel.winNum;
    }

    /// <summary>
    /// �ж��Ƿ�ƽ�֣����и��Ӷ���ռ�� �� û��ʤ�ߣ�
    /// </summary>
    /// <returns></returns>
    private bool IsDraw()
    {
        for (int x = 0; x < GameSettingModel.Instance.mapWidth; x++)
        {
            for (int y = 0; y < GameSettingModel.Instance.mapHeight; y++)
            {
                if (Board[x, y] == PlayerOrder.None)
                    return false; // �пո񣬲���ƽ��
            }
        }

        // ���и��Ӷ���������û��ʤ���� => ƽ��
        return true;
    }

    #region AI����
    /// <summary>
    /// ���һ��������õĸ������� ����
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

// ������
public static class GameTools
{
    /// <summary>
    /// ��������Ӷ���
    /// </summary>
    /// <param name="transform"></param>
    public static void DestroyChild(this Transform transform)
    {
        // ���������������Ӷ���
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            GameObject.Destroy(child.gameObject);
        }
    }
}