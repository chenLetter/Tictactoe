using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// ����� UI �������ť��ģʽ�л��ȣ�����֪ͨ GameInputController
/// </summary>
public class GameUIController : MonoBehaviour
{
    

    // ��Ϸ�߼�����ű�
    private GameInputController inputController;
    private void Start()
    {
        inputController = transform.GetComponent<GameInputController>();        
    }

    /// <summary>
    /// ������ð�ť
    /// </summary>
    public void OnResetClicked()
    {
        if (inputController == null)
        {
            return;
        }
        // ��������������Ϸ����
        GameSettingModel.Instance.SetPlayerData(GameSettingModel.Instance.nOneTwoSet, GameSettingModel.Instance.nOXSet);
        inputController.ResetGame();
        //OnClickedStartSetting();
    }

    /// <summary>
    /// �л���Ϸģʽ����������
    /// </summary>
    public void OnGameModeChanged(int index)
    {
        if (inputController == null)
        {
            return;
        }
        inputController.gameMode = (GameMode)index;
        inputController.ResetGame();
    }

    /// <summary>
    /// �����ʼpvp
    /// </summary>
    public void OnClickedPvP()
    {
        if (inputController == null)
        {
            return;
        }
        // ����Ϸ��ͼ
        OnClickedEnter(GameMode.PvP);

        // ������Ϸ״̬
        //inputController.ResetGame();
    }

    /// <summary>
    /// �����ʼpvai
    /// </summary>
    public void OnClickedPvAI()
    {
        if (inputController == null)
        {
            return;
        }
        // ����Ϸ��ͼ
        OnClickedEnter(GameMode.PvAI);

        // ������Ϸ״̬
        //inputController.ResetGame();
    }
    /// <summary>
    /// ������ð�ť
    /// </summary>
    public void OnClickedSetting()
    {
        if (inputController == null)
        {
            return;
        }
        // ����Ϸ��ͼ
        inputController.starView.CloseView();
        inputController.settingView.OpenView();
    }
    /// <summary>
    /// ������ز˵�
    /// </summary>
    public void OnClickedReturn(BaseView baseView)
    {
        if (inputController == null)
        {
            return;
        }
        if (baseView == null)
        {
            return;
        }
        // ����Ϸ��ͼ
        inputController.starView.OpenView();
        baseView.CloseView();
    }

    /// <summary>
    /// �л���Ϸ�Ѷ�ģʽ
    /// </summary>
    public void OnGameDifficultyChanged(string index)
    {
        // ת��������
        int nNum = int.Parse(index);

        // �Ϸ����ж�
        if (nNum < 1)
        {
            Debug.Log("����Ѷ�Ϊ1");
            nNum = 1;
        }
        if (inputController == null)
        {
            return;
        }
        if (nNum > inputController.MaxDepth)
        {
            Debug.Log($"����Ѷ�Ϊ{inputController.MaxDepth}");
            nNum = inputController.MaxDepth;
        }

        // ������Ϣ
        //GameAI.maxDepth = nNum;
        GameSettingModel.Instance.maxDepth = nNum;
        inputController.settingView.SetInputFieldAI(nNum.ToString());

    }
    /// <summary>
    /// ��ͼ�������
    /// </summary>
    public void OnMapWidthChanged(string index)
    {
        // ת��������
        int nNum = int.Parse(index);

        // �Ϸ����ж�
        if (nNum < 3)
        {
            Debug.Log("��С���Ϊ3");
            nNum = 3;
        }
        if (inputController == null)
        {
            return;
        }
        if (nNum > inputController.MaxMapWidth)
        {
            Debug.Log($"�����Ϊ{inputController.MaxMapWidth}");
            nNum = inputController.MaxMapWidth;
        }

        // ������Ϣ
        GameSettingModel.Instance.mapWidth = nNum;
        inputController.settingView.SetInputFieldWidth(nNum.ToString());

        // �ж��Ƿ���Ҫ������ʤ����
        if (nNum < GameSettingModel.Instance.winNum)
        {
            GameSettingModel.Instance.winNum = nNum;
            inputController.settingView.SetInputFieldWinNum(nNum.ToString());
        }
    }

    /// <summary>
    /// ��ͼ�߶�����
    /// </summary>
    public void OnMapHeightChanged(string index)
    {
        // ת��������
        int nNum = int.Parse(index);
        // �Ϸ����ж�
        if (nNum < 3)
        {
            Debug.Log("��С�߶�Ϊ3");
            nNum = 3;
        }
        if (inputController == null)
        {
            return;
        }
        if (nNum > inputController.MaxMapHeight)
        {
            Debug.Log($"���߶�Ϊ{inputController.MaxMapHeight}");
            nNum = inputController.MaxMapHeight;
        }
        GameSettingModel.Instance.mapHeight = nNum;
        inputController.settingView.SetInputFieldHeight(nNum.ToString());

        // �ж��Ƿ���Ҫ������ʤ����
        if (nNum < GameSettingModel.Instance.winNum)
        {
            GameSettingModel.Instance.winNum = nNum;
            inputController.settingView.SetInputFieldWinNum(nNum.ToString());
        }
    }

    /// <summary>
    /// ��ʤ��������
    /// </summary>
    public void OnWinNumChanged(string index)
    {
        // ת��������
        int nNum = int.Parse(index);
        // �Ϸ����ж�
        if (nNum < 3)
        {
            Debug.Log("��С��ʤ����Ϊ3");
            nNum = 3;
        }
        int nMin = Mathf.Min(GameSettingModel.Instance.mapHeight, GameSettingModel.Instance.mapWidth);
        if (nNum > nMin)
        {
            Debug.Log("��ʤ���Ȳ��ô��ڱ߳�");
            nNum = nMin;
        }
        if (inputController == null)
        {
            return;
        }

        GameSettingModel.Instance.winNum = nNum;
        inputController.settingView.SetInputFieldWinNum(nNum.ToString());
    }

    /// <summary>
    /// �����ʼ��ť
    /// </summary>
    public void OnClickedStartSetting()
    {
        if (inputController == null)
        {
            return;
        }
        // �򿪿�ʼ���ý���
        int nOneTwo  = inputController.starSetView.GetOneTwoSet();
        int nOX = inputController.starSetView.GetOXSet();
        // ������Ϸ��¼
        GameSettingModel.Instance.SetPlayerData(nOneTwo, nOX);
        


        // �ر�������ͼ
        inputController.starSetView.CloseView();
        // ����Ϸ��ͼ
        inputController.gameView.OpenView();
        // ������Ϸ״̬
        inputController.ResetGame();
    }

    /// <summary>
    /// ������Ϸ��ʼѡ��
    /// </summary>
    /// <param name="gameMode"></param>
    public void OnClickedEnter(GameMode gameMode)
    {
        if (inputController == null)
        {
            return;
        }
        // ������ϷģʽΪ PvP
        inputController.gameMode = gameMode;
        // ����Ϸ��ͼ
        inputController.starView.CloseView();
        inputController.starSetView.OpenView(gameMode);
    }
}
