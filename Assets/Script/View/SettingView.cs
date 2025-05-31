using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : BaseView
{
    [Header("AI�Ѷ�����")]
    public InputField inputFieldAI;
    [Header("��ͼ�������")]    
    public InputField inputFieldWidth;
    [Header("��ͼ�߶�����")]
    public InputField inputFieldHeight;
    [Header("ʤ����������")]
    public InputField inputFieldWinNum;

    /// <summary>
    /// ���� InputField ���ı�����
    /// </summary>
    public void SetInputFieldText(InputField inputField, string text)
    {
        if (inputField == null)
        {
            throw new System.Exception($"��������Ϊ��");
        }
        // �����ı�����
        inputField.text = text;
    }
    /// <summary>
    /// ������Ϸ�Ѷ���Ϣ
    /// </summary>
    /// <param name="text"></param>
    public void SetInputFieldAI(string text)
    {
        SetInputFieldText(inputFieldAI, text);
    }
    /// <summary>
    /// ���õ�ͼ�����Ϣ
    /// </summary>
    /// <param name="text"></param>
    public void SetInputFieldWidth(string text)
    {
        SetInputFieldText(inputFieldWidth, text);
    }

    /// <summary>
    /// ���õ�ͼ�߶���Ϣ
    /// </summary>
    /// <param name="text"></param>
    public void SetInputFieldHeight(string text)
    {
        SetInputFieldText(inputFieldHeight, text);
    }

    /// <summary>
    /// ����ʤ��������Ϣ
    /// </summary>
    /// <param name="text"></param>
    public void SetInputFieldWinNum(string text)
    {
        SetInputFieldText(inputFieldWinNum, text);
    }
}
