using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������û����룬Э�� Model �� View
public class GameController : MonoBehaviour
{
    //[Header("��Ϸ��ͼ����")]
    //public GameView view; // ���� View �ű�

    //[Header("���Ӽ��")]
    //public float cellSpacing = 2.5f;

    //[Header("��Ϸģ�͹���")]
    //public GameMode gameMode = GameMode.PvAI;

    ///// <summary>
    ///// ������Ϸ���ݺ��߼�
    ///// </summary>
    //public GameModel model;

    ///// <summary>
    ///// ������һغ��߼��Ľӿ�
    ///// </summary>
    //private IPlayerTurnHandler turnHandler;

    //void Start()
    //{
    //    ResetGame();
    //}

    ///// <summary>
    ///// ���û����ĳ������ʱ������
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <param name="transform">���ӵ�λ��</param>
    //public void OnCellClicked(int x, int y, Transform transform)
    //{
    //    if (turnHandler == null)
    //    {
    //        throw new System.Exception("�ִδ�����δ���ã�������Ϸģʽ�Ƿ���ȷ���á�");
    //    }
    //    turnHandler.OnPlayerMove(this, x, y, transform);        
    //}

    ///// <summary>
    ///// ������Ӻ���
    ///// </summary>
    //public void ProcessMove(int x, int y, Transform transform)
    //{
    //    // �Ϸ��Լ��
    //    if (transform == null)
    //    {
    //        throw new System.Exception("��ͼ���Ӳ���Ϊ�գ��������Ԥ�����Ƿ���ȷ���á�");
    //    }
    //    // ��������
    //    if (!model.MakeMove(x, y))
    //    {
    //        // ����ʧ�ܣ������Ǹ����ѱ�ռ�û���Ϸ�ѽ���
    //        return;
    //    }
    //    // ���ø��ӵ���ʾ���ݣ�ע�⣺��ʱ�Ѿ��������л�����ң�����Ҫ������ʾ��
    //    Player pPlayer = model.CurrentPlayer == Player.X ? Player.O : Player.X;
    //    view.PlaceSymbol(transform, pPlayer);

    //    // �����Ϸ�Ƿ����
    //    if (model.GameOver)
    //    {
    //        // �ж��Ƿ�ƽ��
    //        if (model.CurrentPlayer == Player.None)
    //        {
    //            view.SetStatus("��Ϸƽ��!");
    //            return; // ƽ��ʱֱ�ӷ���
    //        }
    //        // ��Ϸ��������ʾʤ��
    //        view.SetStatus($" {(model.CurrentPlayer == Player.X ? "���O" : "���X")} Ӯ��!");
    //    }
    //    else
    //    {
    //        // ��Ϸ��������ʾ��һ�����
    //        view.SetStatus($"��� {model.CurrentPlayer} �Ļغ�");
    //    }

    //}

    ///// <summary>
    ///// ������Ϸ״̬
    ///// </summary>
    //public void ResetGame()
    //{
    //    //����ģ��
    //    if (model == null)
    //    {
    //        model = new GameModel();
    //    }
    //    model.ResetGame();

    //    view.CreateCell(this, cellSpacing);

    //    view.SetStatus($"��� {model.CurrentPlayer} �Ļغ�"); // 3. ������ʾ

    //    // ������Ϸģ�͵�ģʽѡ����ʵ��ִδ�����
    //    switch (gameMode)
    //    {
    //        case GameMode.PvAI:
    //            turnHandler = new PvAITurnHandler();
    //            break;
    //        case GameMode.PvP:
    //            turnHandler = new PvPTurnHandler();
    //            break;
    //        default:
    //            turnHandler = null;
    //            break;
    //    }
    //}
}
