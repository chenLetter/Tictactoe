using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


interface IPlayerTurnHandler
{
    /// <summary>
    /// 玩家控制时处理的函数
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="cellTransform"></param>
    void OnPlayerMove(GameInputController controller, int x, int y, Transform cellTransform);
}
