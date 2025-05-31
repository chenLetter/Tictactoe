using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StarSetView : BaseView
{
    [Header("先手设置信息")]
    public GameObject OneTwonSetTitle;
    public ToggleGroup OneTwonToggleGroup;
    [Header("棋子设置信息")]
    public GameObject OXTitle;
    public ToggleGroup OXToggleGroup;

    private GameMode _gameMode;
    /// <summary>
    /// 打开界面
    /// </summary>
    public void OpenView(GameMode gameMode)
    {
        // 打开界面
        base.OpenView();

        // 设置游戏模式
        _gameMode = gameMode;

        // 根据游戏模式设置界面内容
        if (OneTwonSetTitle == null || OneTwonToggleGroup == null || OXTitle == null || OXToggleGroup == null)
        {
            throw new System.Exception("有预制体为空！！！");
        }
                
        switch (_gameMode)
        {
            case GameMode.PvP:
                OneTwonSetTitle.SetActive(false);
                OneTwonToggleGroup.gameObject.SetActive(false);
                break;
            case GameMode.PvAI:
                OneTwonSetTitle.SetActive(true);
                OneTwonToggleGroup.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获得先后信息 0代表先手，1代表后手
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public int GetOneTwoSet()
    {
        if (_gameMode == GameMode.PvP)
        {
            return 0;
        }
        // 获取先手设置
        if (OneTwonToggleGroup == null)
        {
            throw new System.Exception("先后手预制体为空");
        }

        // 获取第一个选中的Toggle
        foreach (var selectedToggle in OneTwonToggleGroup.ActiveToggles())
        {
            if (selectedToggle.isOn)
            {

                if (selectedToggle != null)
                {
                    int index = 0;
                    switch (selectedToggle.name)
                    {
                        case "OneToggle":
                            index = 0; // 先手
                            break;
                        case "TwoToggle":
                            index = 1; // 后手                            
                            break;
                        case "RandomToggle":
                            index = 2; // 随机
                            break;
                        default:
                            break;
                    }
                    //Debug.Log(selectedToggle.name);
                    return index;
                }
            }
        }
        

        return 0; // 默认返回0
    }

    /// <summary>
    /// 获取棋子信息 0代表方形 1代表圆形
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public int GetOXSet()
    {
        // 获取先手设置
        if (OXToggleGroup == null)
        {
            throw new System.Exception("棋子选项卡预制体为空");
        }

        // 获取第一个选中的Toggle
        foreach (var selectedToggle in OXToggleGroup.ActiveToggles())
        {
            if (selectedToggle.isOn)
            {
                if (selectedToggle != null)
                {
                    int index = 0;
                    switch (selectedToggle.name)
                    {
                        case "OToggle":
                            index = 0; // 方形
                            break;
                        case "XToggle":
                            index = 1; // 圆形
                            break;
                        case "RandomToggle":
                            index = 2; // 随机
                            break;
                        default:
                            break;
                    }

                    return index;
                }
            }
        }        

        return 0; // 默认返回0
    }
}
