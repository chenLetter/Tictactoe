using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����ÿ��Plane�ϣ����ڼ����
public class CellPlane : MonoBehaviour
{
    /// <summary>
    /// ��ǰ���ӵ����꣨xΪ������yΪ������
    /// </summary>
    [Header("��ǰ���ӵ����꣨xΪ������yΪ������")]
    public int x, y;

    // ��Ϸ������,���ڵ��õ���¼�
    private GameInputController controller;

    /// <summary>
    /// ��ʼ����������Ϳ���������
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="controller"></param>
    public void Initialize(int x, int y, GameInputController controller)
    {
        this.x = x;
        this.y = y;
        this.controller = controller;
    }

    /// <summary>
    /// �������ʱ����
    /// </summary>
    void OnMouseDown()
    {
        controller.OnCellClicked(x, y, transform);
    }

    /// <summary>
    /// �жϵ�ǰ�����Ƿ�Ϊָ��λ�õĸ���
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool IsTrueCell(int x, int y)
    {
        bool isTrue = this.x == x && this.y == y;
        return isTrue;
    }
}
