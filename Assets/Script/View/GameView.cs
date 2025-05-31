using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// ������� UI��������״̬��ʾ��
public class GameView : BaseView
{
    [Header("������ʾ�ı�")]
    public Text statusText; // ����״̬�ı�����ʾ��ǰ�ֵ�˭��˭Ӯ�˵�
    [Header("��ͼ���ɵ�")]
    public GameObject mapPoint;
    [Header("����Ԥ����")]
    public GameObject xPrefab;
    public GameObject oPrefab;
    [Header("����Ԥ����")]
    public GameObject cellPrefab;

    private List<CellPlane> cells = new List<CellPlane>(); // �洢���и��ӵ�����

    /// <summary>
    /// �����̸�λ������ X �� O ����
    /// </summary>
    /// <param name="position"></param>
    /// <param name="player"></param>
    public void PlaceSymbol(Transform transform, E_OX ox)
    {
        // �Ϸ����ж�
        if (xPrefab == null) {
            throw new System.Exception("X����Ԥ����Ϊ�գ�");
        }
        if (oPrefab == null)
        {
            throw new System.Exception("O����Ԥ����Ϊ�գ�");
        }

        // �����������ѡ��Ԥ����
        GameObject prefab = xPrefab;
        switch (ox)
        {
            case E_OX.O:
                prefab = oPrefab; // �����O���ӣ���ʹ��O��Ԥ����
                break;
            case E_OX.X:
                prefab = xPrefab;
                break;
            default:
                break;
        }
        // ʵ��������Ԥ����
        GameObject goItem = Instantiate(prefab, transform);
        goItem.transform.position = transform.position + Vector3.up * 0.5f; // ����λ����΢̧��һ��
    }

    

    /// <summary>
    /// ������ͼ����,�������ĵ�λ��
    /// </summary>
    public Vector3 CreateCell(GameInputController controller, float cellSpacing)
    {
        // �Ϸ����ж�
        if (cellPrefab == null)
        {
            throw new System.Exception("��ͼ����Ԥ����Ϊ�գ�����");
        }
        if (controller == null)
        {
            throw new System.Exception("��Ϸ������Ϊ�գ�����");
        }
        if (mapPoint == null)
        {
            throw new System.Exception("��ͼ���ɵ�Ϊ�գ�����");
        }

        // �������и��ӣ�����еĻ���
        mapPoint.transform.DestroyChild();
        cells.Clear();

        // ��ȡ������Ϣ
        GameSettingModel gameSettingModel = GameSettingModel.Instance;
        // ׼�����ĵ�λ��
        Vector3 vCenterPos = Vector3.zero;
        // ����3x3�ĸ��ӣ���9����
        for (int x = 0; x < gameSettingModel.mapWidth; x++)
        {
            for (int y = 0; y < gameSettingModel.mapHeight; y++)
            {
                // ���㵱ǰ������3D�ռ��е�λ��
                Vector3 position = new Vector3(x * cellSpacing, 0, y * cellSpacing);

                // ʵ����һ��Plane����
                GameObject cell = Instantiate(cellPrefab, mapPoint.transform);
                // ����λ��
                cell.transform.position = position;

                // ��ȡ�ø��ӵĽű����ã��������CellPlane�����
                CellPlane plane = cell.GetComponent<CellPlane>();
                
                // ��ʼ���ø��ӵ����������������
                plane.Initialize(x, y, controller);
                cells.Add(plane); // ��������ӵ��б���

                //// �ж��Ƿ�Ϊ���ĵ�
                //if (x == gameSettingModel.mapWidth/2 && y == gameSettingModel.mapHeight/2)
                //{
                //    // ��¼���ĵ�
                //    vCenterPos = position;
                //}
            }
        }

        // �����е���Ϣ ��������-1�� * ������� = ���ɵĿ��
        vCenterPos = new Vector3(((gameSettingModel.mapWidth - 1) * cellSpacing)/2, 0, ((gameSettingModel.mapHeight - 1) * cellSpacing)/2);

        return vCenterPos;
    }

    /// <summary>
    /// ���ָ�����ӵ�λ����Ϣ
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Transform GetCellTransform(int x, int y)
    {
        foreach (var cell in cells)
        {
            if(cell.IsTrueCell(x, y))
            {
                return cell.transform; // ����ƥ��ĸ��ӱ任
            }
        }
        return null;
    }


    // ������ʾ�ı����硰Player X's Turn����
    public void SetStatus(string text)
    {
        statusText.text = text;
        
    }
    
}
