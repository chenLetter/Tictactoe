using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// 负责绑定 UI 组件（按钮、模式切换等），并通知 GameInputController
/// </summary>
public class GameUIController : MonoBehaviour
{
    

    // 游戏逻辑处理脚本
    private GameInputController inputController;
    private void Start()
    {
        inputController = transform.GetComponent<GameInputController>();        
    }

    /// <summary>
    /// 点击重置按钮
    /// </summary>
    public void OnResetClicked()
    {
        if (inputController == null)
        {
            return;
        }
        // 根据设置设置游戏进程
        GameSettingModel.Instance.SetPlayerData(GameSettingModel.Instance.nOneTwoSet, GameSettingModel.Instance.nOXSet);
        inputController.ResetGame();
        //OnClickedStartSetting();
    }

    /// <summary>
    /// 切换游戏模式（从下拉框）
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
    /// 点击开始pvp
    /// </summary>
    public void OnClickedPvP()
    {
        if (inputController == null)
        {
            return;
        }
        // 打开游戏视图
        OnClickedEnter(GameMode.PvP);

        // 重置游戏状态
        //inputController.ResetGame();
    }

    /// <summary>
    /// 点击开始pvai
    /// </summary>
    public void OnClickedPvAI()
    {
        if (inputController == null)
        {
            return;
        }
        // 打开游戏视图
        OnClickedEnter(GameMode.PvAI);

        // 重置游戏状态
        //inputController.ResetGame();
    }
    /// <summary>
    /// 点击设置按钮
    /// </summary>
    public void OnClickedSetting()
    {
        if (inputController == null)
        {
            return;
        }
        // 打开游戏视图
        inputController.starView.CloseView();
        inputController.settingView.OpenView();
    }
    /// <summary>
    /// 点击返回菜单
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
        // 打开游戏视图
        inputController.starView.OpenView();
        baseView.CloseView();
    }

    /// <summary>
    /// 切换游戏难度模式
    /// </summary>
    public void OnGameDifficultyChanged(string index)
    {
        // 转换成整数
        int nNum = int.Parse(index);

        // 合法性判定
        if (nNum < 1)
        {
            Debug.Log("最低难度为1");
            nNum = 1;
        }
        if (inputController == null)
        {
            return;
        }
        if (nNum > inputController.MaxDepth)
        {
            Debug.Log($"最低难度为{inputController.MaxDepth}");
            nNum = inputController.MaxDepth;
        }

        // 设置信息
        //GameAI.maxDepth = nNum;
        GameSettingModel.Instance.maxDepth = nNum;
        inputController.settingView.SetInputFieldAI(nNum.ToString());

    }
    /// <summary>
    /// 地图宽度设置
    /// </summary>
    public void OnMapWidthChanged(string index)
    {
        // 转换成整数
        int nNum = int.Parse(index);

        // 合法性判定
        if (nNum < 3)
        {
            Debug.Log("最小宽度为3");
            nNum = 3;
        }
        if (inputController == null)
        {
            return;
        }
        if (nNum > inputController.MaxMapWidth)
        {
            Debug.Log($"最大宽度为{inputController.MaxMapWidth}");
            nNum = inputController.MaxMapWidth;
        }

        // 设置信息
        GameSettingModel.Instance.mapWidth = nNum;
        inputController.settingView.SetInputFieldWidth(nNum.ToString());

        // 判断是否需要调整获胜长度
        if (nNum < GameSettingModel.Instance.winNum)
        {
            GameSettingModel.Instance.winNum = nNum;
            inputController.settingView.SetInputFieldWinNum(nNum.ToString());
        }
    }

    /// <summary>
    /// 地图高度设置
    /// </summary>
    public void OnMapHeightChanged(string index)
    {
        // 转换成整数
        int nNum = int.Parse(index);
        // 合法性判定
        if (nNum < 3)
        {
            Debug.Log("最小高度为3");
            nNum = 3;
        }
        if (inputController == null)
        {
            return;
        }
        if (nNum > inputController.MaxMapHeight)
        {
            Debug.Log($"最大高度为{inputController.MaxMapHeight}");
            nNum = inputController.MaxMapHeight;
        }
        GameSettingModel.Instance.mapHeight = nNum;
        inputController.settingView.SetInputFieldHeight(nNum.ToString());

        // 判断是否需要调整获胜长度
        if (nNum < GameSettingModel.Instance.winNum)
        {
            GameSettingModel.Instance.winNum = nNum;
            inputController.settingView.SetInputFieldWinNum(nNum.ToString());
        }
    }

    /// <summary>
    /// 获胜条件设置
    /// </summary>
    public void OnWinNumChanged(string index)
    {
        // 转换成整数
        int nNum = int.Parse(index);
        // 合法性判定
        if (nNum < 3)
        {
            Debug.Log("最小获胜长度为3");
            nNum = 3;
        }
        int nMin = Mathf.Min(GameSettingModel.Instance.mapHeight, GameSettingModel.Instance.mapWidth);
        if (nNum > nMin)
        {
            Debug.Log("获胜长度不该大于边长");
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
    /// 点击开始按钮
    /// </summary>
    public void OnClickedStartSetting()
    {
        if (inputController == null)
        {
            return;
        }
        // 打开开始设置界面
        int nOneTwo  = inputController.starSetView.GetOneTwoSet();
        int nOX = inputController.starSetView.GetOXSet();
        // 设置游戏记录
        GameSettingModel.Instance.SetPlayerData(nOneTwo, nOX);
        


        // 关闭设置视图
        inputController.starSetView.CloseView();
        // 打开游戏视图
        inputController.gameView.OpenView();
        // 重置游戏状态
        inputController.ResetGame();
    }

    /// <summary>
    /// 进入游戏开始选项
    /// </summary>
    /// <param name="gameMode"></param>
    public void OnClickedEnter(GameMode gameMode)
    {
        if (inputController == null)
        {
            return;
        }
        // 设置游戏模式为 PvP
        inputController.gameMode = gameMode;
        // 打开游戏视图
        inputController.starView.CloseView();
        inputController.starSetView.OpenView(gameMode);
    }
}
