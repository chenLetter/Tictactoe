using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : BaseView
{
    [Header("AI难度设置")]
    public InputField inputFieldAI;
    [Header("地图宽度设置")]    
    public InputField inputFieldWidth;
    [Header("地图高度设置")]
    public InputField inputFieldHeight;
    [Header("胜利条件设置")]
    public InputField inputFieldWinNum;

    /// <summary>
    /// 设置 InputField 的文本内容
    /// </summary>
    public void SetInputFieldText(InputField inputField, string text)
    {
        if (inputField == null)
        {
            throw new System.Exception($"输入框对象为空");
        }
        // 设置文本内容
        inputField.text = text;
    }
    /// <summary>
    /// 设置游戏难度信息
    /// </summary>
    /// <param name="text"></param>
    public void SetInputFieldAI(string text)
    {
        SetInputFieldText(inputFieldAI, text);
    }
    /// <summary>
    /// 设置地图宽度信息
    /// </summary>
    /// <param name="text"></param>
    public void SetInputFieldWidth(string text)
    {
        SetInputFieldText(inputFieldWidth, text);
    }

    /// <summary>
    /// 设置地图高度信息
    /// </summary>
    /// <param name="text"></param>
    public void SetInputFieldHeight(string text)
    {
        SetInputFieldText(inputFieldHeight, text);
    }

    /// <summary>
    /// 设置胜利条件信息
    /// </summary>
    /// <param name="text"></param>
    public void SetInputFieldWinNum(string text)
    {
        SetInputFieldText(inputFieldWinNum, text);
    }
}
