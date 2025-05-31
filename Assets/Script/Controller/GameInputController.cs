using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������û����롢AI���ӡ���Ϸ�߼�����
/// </summary>
public class GameInputController : MonoBehaviour
{
    [Header("��Ϸ����")]
    public GameView gameView; // ���� View �ű�
    [Header("��ʼ����")]
    public StarView starView; // ���� View �ű�
    [Header("���ý���")]
    public SettingView settingView; // ���� View �ű�
    [Header("��ʼ���ý���")]
    public StarSetView starSetView; // ���� View �ű�
    [Header("�Ѷ����ã�̫�߻ᱬջ��")]
    public int MaxDepth = 5; // ����Ѷ�
    [Header("��ͼ����")]
    public int MaxMapWidth = 20; // ��ͼ���
    public int MaxMapHeight = 20; // ��ͼ�߶�
    [Header("���Ӽ��")]
    public float cellSpacing = 2.5f;

    [Header("��Ϸģ�͹���")]
    public GameMode gameMode = GameMode.PvAI;

    /// <summary>
    /// ������Ϸ���ݺ��߼�
    /// </summary>
    public GameModel model;
    

    /// <summary>
    /// ������һغ��߼��Ľӿ�
    /// </summary>
    private IPlayerTurnHandler turnHandler;

    /// <summary>
    /// ���������
    /// </summary>
    private CameraController cameraController;

    /// <summary>
    /// ���A�ĳƺ�
    /// </summary>
    private string PlayerAName;
    /// <summary>
    /// ���B�ĳƺ�
    /// </summary>
    private string PlayerBName;

    /// <summary>
    /// ��ҵ�������¼�
    /// </summary>
    public void OnCellClicked(int x, int y, Transform cellTransform)
    {
        if (turnHandler == null)
        {
            throw new System.Exception("�ִδ�����δ���ã�������Ϸģʽ�Ƿ���ȷ���á�");
        }
        turnHandler.OnPlayerMove(this, x, y, cellTransform);
    }

    /// <summary>
    /// ���Ӻ���
    /// </summary>
    public void ProcessMove(int x, int y, Transform transform)
    {
        // �Ϸ��Լ��
        if (transform == null)
        {
            throw new System.Exception("��ͼ���Ӳ���Ϊ�գ��������Ԥ�����Ƿ���ȷ���á�");
        }
        // ��������
        if (!model.MakeMove(x, y))
        {
            // ����ʧ�ܣ������Ǹ����ѱ�ռ�û���Ϸ�ѽ���
            return;
        }
        // ���ø��ӵ���ʾ���ݣ�ע�⣺��ʱ�Ѿ��������л�����ң�����Ҫ������ʾ��
        PlayerOrder pPlayerOrder = model.CurrentPlayer == PlayerOrder.One ? PlayerOrder.Two : PlayerOrder.One;
        // ������ʽ
        E_OX ox = E_OX.X;
        // �ж��Ƿ�Ϊ���A����
        if (GameSettingModel.Instance.playerA.playerOrder == pPlayerOrder)
        {
            ox = GameSettingModel.Instance.playerA.playerOX;
        }
        else
        {
            ox = GameSettingModel.Instance.playerB.playerOX;
        }
        gameView.PlaceSymbol(transform, ox);

        // �ж���Ϸ״̬
        HandleGameStateAfterMove();
    }

    

    /// <summary>
    /// ���ʤ����ƽ�֣�������״̬�ı�
    /// </summary>
    private void HandleGameStateAfterMove()
    {
        if (model.GameOver)
        {
            if (model.CurrentPlayer == PlayerOrder.None)
                gameView.SetStatus("��Ϸƽ�֣�");
            else
                gameView.SetStatus($"{(model.CurrentPlayer == GameSettingModel.Instance.playerA.playerOrder ? PlayerBName : PlayerAName)} Ӯ�ˣ�");
        }
        else
        {
            gameView.SetStatus($"{(model.CurrentPlayer == GameSettingModel.Instance.playerA.playerOrder ? PlayerAName : PlayerBName)} �Ļغ�");
        }
    }


    /// <summary>
    /// �ⲿ���ã�������Ϸ
    /// </summary>
    public void ResetGame()
    {
        // �Ϸ����ж�
        if (model == null)
        {
            model = new GameModel();
        }        
        if (cameraController == null)
        {
            cameraController = this.GetComponent<CameraController>();
        }
        if (gameView == null)
        {
            throw new System.Exception("��Ϸ����Ϊ��");
        }
        //����ģ��
        model.ResetGame();
        //������ͼ
        Vector3 vCenterPos = gameView.CreateCell(this, cellSpacing);
        
        //���������
        //ȷ��λ��
        cameraController?.SetCameraPosition(vCenterPos);
        //���ô�С
        float nMaxSide = Mathf.Max(vCenterPos.x, vCenterPos.z);
        cameraController?.SetCameraSize(nMaxSide + 2.5f);


        // ������Ϸģ�͵�ģʽ,��ʼ������
        switch (gameMode)
        {
            case GameMode.PvAI:
                SetPvAIGame();
                break;
            case GameMode.PvP:
                SetPvPGame();
                break;
            default:
                turnHandler = null;
                break;
        }

        // ������ʾ
        gameView.SetStatus($"{(model.CurrentPlayer == GameSettingModel.Instance.playerA.playerOrder ? PlayerAName : PlayerBName)} �Ļغ�");

    }

    /// <summary>
    /// ��ʼ��PvP
    /// </summary>
    private void SetPvPGame()
    {
        // �����������
        PlayerAName = "���1";
        PlayerBName = "���2";

        //ѡ����ʵ��ִδ�����
        turnHandler = new PvPTurnHandler();

    }
    /// <summary>
    /// ��ʼ��PvAI
    /// </summary>
    private void SetPvAIGame()
    {
        // �����������
        PlayerAName = "���";
        PlayerBName = "�˻�";

        //ѡ����ʵ��ִδ�����
        turnHandler = new PvAITurnHandler();
        // �ж��Ƿ�Ϊai���� ֱ���� AI ����
        if (GameSettingModel.Instance.playerB.playerOrder == PlayerOrder.One)
        {
            GameInputController gic = this;
            PvAITurnHandler pPvAITurnHandler = (PvAITurnHandler)turnHandler;
            pPvAITurnHandler.OnAIMove(gic);
        }

    }

}

