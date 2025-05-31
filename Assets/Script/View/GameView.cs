using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 负责更新 UI（棋盘与状态提示）
public class GameView : BaseView
{
    [Header("顶部提示文本")]
    public Text statusText; // 顶部状态文本：显示当前轮到谁、谁赢了等
    [Header("地图生成点")]
    public GameObject mapPoint;
    [Header("棋子预制体")]
    public GameObject xPrefab;
    public GameObject oPrefab;
    [Header("格子预制体")]
    public GameObject cellPrefab;

    private List<CellPlane> cells = new List<CellPlane>(); // 存储所有格子的引用

    /// <summary>
    /// 在棋盘格位置生成 X 或 O 物体
    /// </summary>
    /// <param name="position"></param>
    /// <param name="player"></param>
    public void PlaceSymbol(Transform transform, E_OX ox)
    {
        // 合法性判断
        if (xPrefab == null) {
            throw new System.Exception("X棋子预制体为空！");
        }
        if (oPrefab == null)
        {
            throw new System.Exception("O棋子预制体为空！");
        }

        // 根据玩家类型选择预制体
        GameObject prefab = xPrefab;
        switch (ox)
        {
            case E_OX.O:
                prefab = oPrefab; // 如果是O棋子，则使用O的预制体
                break;
            case E_OX.X:
                prefab = xPrefab;
                break;
            default:
                break;
        }
        // 实例化棋子预制体
        GameObject goItem = Instantiate(prefab, transform);
        goItem.transform.position = transform.position + Vector3.up * 0.5f; // 设置位置稍微抬高一点
    }

    

    /// <summary>
    /// 创建地图格子,返回中心点位置
    /// </summary>
    public Vector3 CreateCell(GameInputController controller, float cellSpacing)
    {
        // 合法性判断
        if (cellPrefab == null)
        {
            throw new System.Exception("地图格子预制体为空！！！");
        }
        if (controller == null)
        {
            throw new System.Exception("游戏控制器为空！！！");
        }
        if (mapPoint == null)
        {
            throw new System.Exception("地图生成点为空！！！");
        }

        // 清理现有格子（如果有的话）
        mapPoint.transform.DestroyChild();
        cells.Clear();

        // 获取设置信息
        GameSettingModel gameSettingModel = GameSettingModel.Instance;
        // 准备中心点位置
        Vector3 vCenterPos = Vector3.zero;
        // 创建3x3的格子（共9个）
        for (int x = 0; x < gameSettingModel.mapWidth; x++)
        {
            for (int y = 0; y < gameSettingModel.mapHeight; y++)
            {
                // 计算当前格子在3D空间中的位置
                Vector3 position = new Vector3(x * cellSpacing, 0, y * cellSpacing);

                // 实例化一个Plane格子
                GameObject cell = Instantiate(cellPrefab, mapPoint.transform);
                // 设置位置
                cell.transform.position = position;

                // 获取该格子的脚本引用（必须存在CellPlane组件）
                CellPlane plane = cell.GetComponent<CellPlane>();
                
                // 初始化该格子的坐标与控制器引用
                plane.Initialize(x, y, controller);
                cells.Add(plane); // 将格子添加到列表中

                //// 判断是否为中心点
                //if (x == gameSettingModel.mapWidth/2 && y == gameSettingModel.mapHeight/2)
                //{
                //    // 记录中心点
                //    vCenterPos = position;
                //}
            }
        }

        // 计算中点信息 （宽数量-1） * 间隔距离 = 生成的宽度
        vCenterPos = new Vector3(((gameSettingModel.mapWidth - 1) * cellSpacing)/2, 0, ((gameSettingModel.mapHeight - 1) * cellSpacing)/2);

        return vCenterPos;
    }

    /// <summary>
    /// 获得指定格子的位置信息
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Transform GetCellTransform(int x, int y)
    {
        foreach (var cell in cells)
        {
            if(cell.IsTrueCell(x, y))
            {
                return cell.transform; // 返回匹配的格子变换
            }
        }
        return null;
    }


    // 设置提示文本（如“Player X's Turn”）
    public void SetStatus(string text)
    {
        statusText.text = text;
        
    }
    
}
