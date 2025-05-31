using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责处理用户输入、AI落子、游戏逻辑控制
/// </summary>
public class GameInputController : MonoBehaviour
{
    [Header("游戏界面")]
    public GameView gameView; // 引用 View 脚本
    [Header("开始界面")]
    public StarView starView; // 引用 View 脚本
    [Header("设置界面")]
    public SettingView settingView; // 引用 View 脚本
    [Header("开始设置界面")]
    public StarSetView starSetView; // 引用 View 脚本
    [Header("难度设置（太高会爆栈）")]
    public int MaxDepth = 5; // 最大难度
    [Header("地图设置")]
    public int MaxMapWidth = 20; // 地图宽度
    public int MaxMapHeight = 20; // 地图高度
    [Header("格子间距")]
    public float cellSpacing = 2.5f;

    [Header("游戏模型管理")]
    public GameMode gameMode = GameMode.PvAI;

    /// <summary>
    /// 管理游戏数据和逻辑
    /// </summary>
    public GameModel model;
    

    /// <summary>
    /// 处理玩家回合逻辑的接口
    /// </summary>
    private IPlayerTurnHandler turnHandler;

    /// <summary>
    /// 相机控制器
    /// </summary>
    private CameraController cameraController;

    /// <summary>
    /// 玩家A的称呼
    /// </summary>
    private string PlayerAName;
    /// <summary>
    /// 玩家B的称呼
    /// </summary>
    private string PlayerBName;

    /// <summary>
    /// 玩家点击格子事件
    /// </summary>
    public void OnCellClicked(int x, int y, Transform cellTransform)
    {
        if (turnHandler == null)
        {
            throw new System.Exception("轮次处理器未设置，请检查游戏模式是否正确配置。");
        }
        turnHandler.OnPlayerMove(this, x, y, cellTransform);
    }

    /// <summary>
    /// 落子函数
    /// </summary>
    public void ProcessMove(int x, int y, Transform transform)
    {
        // 合法性检查
        if (transform == null)
        {
            throw new System.Exception("地图格子不能为空！请检查格子预制体是否正确设置。");
        }
        // 尝试落子
        if (!model.MakeMove(x, y))
        {
            // 落子失败，可能是格子已被占用或游戏已结束
            return;
        }
        // 设置格子的显示内容（注意：此时已经落完子切换了玩家，所以要反向显示）
        PlayerOrder pPlayerOrder = model.CurrentPlayer == PlayerOrder.One ? PlayerOrder.Two : PlayerOrder.One;
        // 棋子样式
        E_OX ox = E_OX.X;
        // 判断是否为玩家A落子
        if (GameSettingModel.Instance.playerA.playerOrder == pPlayerOrder)
        {
            ox = GameSettingModel.Instance.playerA.playerOX;
        }
        else
        {
            ox = GameSettingModel.Instance.playerB.playerOX;
        }
        gameView.PlaceSymbol(transform, ox);

        // 判断游戏状态
        HandleGameStateAfterMove();
    }

    

    /// <summary>
    /// 检查胜负或平局，并更新状态文本
    /// </summary>
    private void HandleGameStateAfterMove()
    {
        if (model.GameOver)
        {
            if (model.CurrentPlayer == PlayerOrder.None)
                gameView.SetStatus("游戏平局！");
            else
                gameView.SetStatus($"{(model.CurrentPlayer == GameSettingModel.Instance.playerA.playerOrder ? PlayerBName : PlayerAName)} 赢了！");
        }
        else
        {
            gameView.SetStatus($"{(model.CurrentPlayer == GameSettingModel.Instance.playerA.playerOrder ? PlayerAName : PlayerBName)} 的回合");
        }
    }


    /// <summary>
    /// 外部调用：重置游戏
    /// </summary>
    public void ResetGame()
    {
        // 合法性判定
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
            throw new System.Exception("游戏界面为空");
        }
        //重置模型
        model.ResetGame();
        //创建地图
        Vector3 vCenterPos = gameView.CreateCell(this, cellSpacing);
        
        //更新摄像机
        //确定位置
        cameraController?.SetCameraPosition(vCenterPos);
        //设置大小
        float nMaxSide = Mathf.Max(vCenterPos.x, vCenterPos.z);
        cameraController?.SetCameraSize(nMaxSide + 2.5f);


        // 根据游戏模型的模式,初始化设置
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

        // 更新提示
        gameView.SetStatus($"{(model.CurrentPlayer == GameSettingModel.Instance.playerA.playerOrder ? PlayerAName : PlayerBName)} 的回合");

    }

    /// <summary>
    /// 初始化PvP
    /// </summary>
    private void SetPvPGame()
    {
        // 设置玩家名称
        PlayerAName = "玩家1";
        PlayerBName = "玩家2";

        //选择合适的轮次处理器
        turnHandler = new PvPTurnHandler();

    }
    /// <summary>
    /// 初始化PvAI
    /// </summary>
    private void SetPvAIGame()
    {
        // 设置玩家名称
        PlayerAName = "玩家";
        PlayerBName = "人机";

        //选择合适的轮次处理器
        turnHandler = new PvAITurnHandler();
        // 判断是否为ai先手 直接让 AI 落子
        if (GameSettingModel.Instance.playerB.playerOrder == PlayerOrder.One)
        {
            GameInputController gic = this;
            PvAITurnHandler pPvAITurnHandler = (PvAITurnHandler)turnHandler;
            pPvAITurnHandler.OnAIMove(gic);
        }

    }

}

