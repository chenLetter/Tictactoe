using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 负责处理用户输入，协调 Model 和 View
public class GameController : MonoBehaviour
{
    //[Header("游戏视图管理")]
    //public GameView view; // 引用 View 脚本

    //[Header("格子间距")]
    //public float cellSpacing = 2.5f;

    //[Header("游戏模型管理")]
    //public GameMode gameMode = GameMode.PvAI;

    ///// <summary>
    ///// 管理游戏数据和逻辑
    ///// </summary>
    //public GameModel model;

    ///// <summary>
    ///// 处理玩家回合逻辑的接口
    ///// </summary>
    //private IPlayerTurnHandler turnHandler;

    //void Start()
    //{
    //    ResetGame();
    //}

    ///// <summary>
    ///// 当用户点击某个格子时被调用
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <param name="transform">格子的位置</param>
    //public void OnCellClicked(int x, int y, Transform transform)
    //{
    //    if (turnHandler == null)
    //    {
    //        throw new System.Exception("轮次处理器未设置，请检查游戏模式是否正确配置。");
    //    }
    //    turnHandler.OnPlayerMove(this, x, y, transform);        
    //}

    ///// <summary>
    ///// 玩家落子函数
    ///// </summary>
    //public void ProcessMove(int x, int y, Transform transform)
    //{
    //    // 合法性检查
    //    if (transform == null)
    //    {
    //        throw new System.Exception("地图格子不能为空！请检查格子预制体是否正确设置。");
    //    }
    //    // 尝试落子
    //    if (!model.MakeMove(x, y))
    //    {
    //        // 落子失败，可能是格子已被占用或游戏已结束
    //        return;
    //    }
    //    // 设置格子的显示内容（注意：此时已经落完子切换了玩家，所以要反向显示）
    //    Player pPlayer = model.CurrentPlayer == Player.X ? Player.O : Player.X;
    //    view.PlaceSymbol(transform, pPlayer);

    //    // 检查游戏是否结束
    //    if (model.GameOver)
    //    {
    //        // 判断是否平局
    //        if (model.CurrentPlayer == Player.None)
    //        {
    //            view.SetStatus("游戏平局!");
    //            return; // 平局时直接返回
    //        }
    //        // 游戏结束，显示胜者
    //        view.SetStatus($" {(model.CurrentPlayer == Player.X ? "玩家O" : "玩家X")} 赢了!");
    //    }
    //    else
    //    {
    //        // 游戏继续，提示下一个玩家
    //        view.SetStatus($"玩家 {model.CurrentPlayer} 的回合");
    //    }

    //}

    ///// <summary>
    ///// 重置游戏状态
    ///// </summary>
    //public void ResetGame()
    //{
    //    //重置模型
    //    if (model == null)
    //    {
    //        model = new GameModel();
    //    }
    //    model.ResetGame();

    //    view.CreateCell(this, cellSpacing);

    //    view.SetStatus($"玩家 {model.CurrentPlayer} 的回合"); // 3. 设置提示

    //    // 根据游戏模型的模式选择合适的轮次处理器
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
