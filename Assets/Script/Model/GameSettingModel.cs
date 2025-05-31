using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// ��Ϸ������
/// </summary>
public class GameSettingModel
{
    #region ����
    /// <summary>
    /// ����
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
    /// ��Ϸ�Ѷȣ�Ĭ��Ϊ��
    /// </summary>
    public int maxDepth = 3;

    // ��Ϸ��ͼ
    /// <summary>
    /// ��ͼ���
    /// </summary>
    public int mapWidth = 3;
    /// <summary>
    /// ��ͼ�߶�
    /// </summary>
    public int mapHeight = 3;
    /// <summary>
    /// ʤ������������
    /// </summary>
    public int winNum = 3;

    /// <summary>
    /// �����Ϣ�࣬���ڴ洢����������
    /// </summary>
    public PlayerData playerA = new PlayerData();
    /// <summary>
    /// �����Ϣ�࣬���ڴ洢����������
    /// </summary>
    public PlayerData playerB = new PlayerData();

    /// <summary>
    /// ����Ⱥ�����Ϣ 0 ����ΪA 1 ����ΪB 2���
    /// </summary>
    public int nOneTwoSet = 0;
    /// <summary>
    /// �������ѡ����Ϣ 0 ΪX 1 ΪO 2Ϊ���
    /// </summary>
    public int nOXSet = 0;

    /// <summary>
    /// ���������Ϣ
    /// </summary>
    public void SetPlayerData(PlayerData pPlayerA, PlayerData pPlayerB)
    {
        playerA = pPlayerA;
        playerB = pPlayerB;
    }

    /// <summary>
    /// ��������������������Ϣ
    /// </summary>
    /// <param name="pOneTwon"></param>
    /// <param name="pOX"></param>
    public void SetPlayerData(int pOneTwon, int pOX)
    {
        // ��¼��Ϣ
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

        Debug.Log($"�Ⱥ�������: {pOneTwoSet}, ��������: {pOXSet}");

        // ������Ϸģ�͵��Ⱥ��ֺ���������
        PlayerOrder pPlayerOrderA = PlayerOrder.One;
        PlayerOrder pPlayerOrderB = PlayerOrder.Two;
        switch (pOneTwoSet)
        {
            case 0: // ����
                pPlayerOrderA = PlayerOrder.One;
                pPlayerOrderB = PlayerOrder.Two;
                break;
            case 1: // ����
                pPlayerOrderA = PlayerOrder.Two;
                pPlayerOrderB = PlayerOrder.One;
                break;
        }
        E_OX pOXA = E_OX.X; // Ĭ������Ϊ X
        E_OX pOXB = E_OX.O; // Ĭ������Ϊ O
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
        // ����������ݵ���Ϸ����ģ����
        GameSettingModel.Instance.SetPlayerData(playerA, playerB);
    }
}

/// <summary>
/// �����Ϣ��
/// </summary>
public class PlayerData
{
    /// <summary>
    /// ����˳��
    /// </summary>
    public PlayerOrder playerOrder = PlayerOrder.One; // Ĭ�����1����
    /// <summary>
    /// �����������
    /// </summary>
    public E_OX playerOX = E_OX.X; // Ĭ�����1ΪX

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
    Easy,   // ��
    Hard,   // ����
}
/// <summary>
/// ��Ϸģʽö��
/// </summary>
public enum GameMode
{
    PvP,
    PvAI
}

/// <summary>
/// ��Ϸ����Ⱥ�����ö��
/// </summary>
public enum PlayerOrder { None, One, Two }

/// <summary>
/// �����������ö��
/// </summary>
public enum E_OX
{
    O, // ���O
    X  // ���X
}