using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// PvAI 模式下的玩家轮次处理器
/// </summary>
public class PvAITurnHandler : IPlayerTurnHandler
{
    private bool isAITurn = false;

    public void OnPlayerMove(GameInputController controller, int x, int y, Transform cellTransform)
    {
        if (isAITurn) return;

        // 人先下，下完ai才下
        controller.ProcessMove(x, y, cellTransform);

        // 如果轮到 AI 落子并未结束
        if (!controller.model.GameOver && controller.model.CurrentPlayer == GameSettingModel.Instance.playerB.playerOrder)
        {
            OnAIMove(controller);
        }
    }

    /// <summary>
    /// ai 落子逻辑
    /// </summary>
    /// <param name="controller"></param>
    /// <returns></returns>
    private IEnumerator AIMove(GameInputController controller)
    {
        isAITurn = true;

        yield return null;

        //(int x, int y) = controller.model.GetRandomAvailableCell();
        Vector2Int bestMove = GameAI.GetBestMove(controller.model.Board);
        Transform cellTransform = controller.gameView.GetCellTransform(bestMove.x, bestMove.y);

        // 落子
        controller.ProcessMove(bestMove.x, bestMove.y, cellTransform);

        isAITurn = false;
    }

    public void OnAIMove(GameInputController controller)
    {
        controller.StartCoroutine(AIMove(controller));
    }
}
