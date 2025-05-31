using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 掛在每个Plane上，用于检测点击
public class CellPlane : MonoBehaviour
{
    /// <summary>
    /// 当前格子的坐标（x为列数，y为行数）
    /// </summary>
    [Header("当前格子的坐标（x为行数，y为列数）")]
    public int x, y;

    // 游戏控制器,用于调用点击事件
    private GameInputController controller;

    /// <summary>
    /// 初始化格子坐标和控制器引用
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
    /// 点击格子时调用
    /// </summary>
    void OnMouseDown()
    {
        controller.OnCellClicked(x, y, transform);
    }

    /// <summary>
    /// 判断当前格子是否为指定位置的格子
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
