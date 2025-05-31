using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// PvP模式的玩家回合处理器
/// </summary>
public class PvPTurnHandler : IPlayerTurnHandler
{
    public void OnPlayerMove(GameInputController controller, int x, int y, Transform cellTransform)
    {
        controller.ProcessMove(x, y, cellTransform);
    }
}
