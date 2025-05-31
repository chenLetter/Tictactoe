using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StarSetView : BaseView
{
    [Header("����������Ϣ")]
    public GameObject OneTwonSetTitle;
    public ToggleGroup OneTwonToggleGroup;
    [Header("����������Ϣ")]
    public GameObject OXTitle;
    public ToggleGroup OXToggleGroup;

    private GameMode _gameMode;
    /// <summary>
    /// �򿪽���
    /// </summary>
    public void OpenView(GameMode gameMode)
    {
        // �򿪽���
        base.OpenView();

        // ������Ϸģʽ
        _gameMode = gameMode;

        // ������Ϸģʽ���ý�������
        if (OneTwonSetTitle == null || OneTwonToggleGroup == null || OXTitle == null || OXToggleGroup == null)
        {
            throw new System.Exception("��Ԥ����Ϊ�գ�����");
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
    /// ����Ⱥ���Ϣ 0�������֣�1�������
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public int GetOneTwoSet()
    {
        if (_gameMode == GameMode.PvP)
        {
            return 0;
        }
        // ��ȡ��������
        if (OneTwonToggleGroup == null)
        {
            throw new System.Exception("�Ⱥ���Ԥ����Ϊ��");
        }

        // ��ȡ��һ��ѡ�е�Toggle
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
                            index = 0; // ����
                            break;
                        case "TwoToggle":
                            index = 1; // ����                            
                            break;
                        case "RandomToggle":
                            index = 2; // ���
                            break;
                        default:
                            break;
                    }
                    //Debug.Log(selectedToggle.name);
                    return index;
                }
            }
        }
        

        return 0; // Ĭ�Ϸ���0
    }

    /// <summary>
    /// ��ȡ������Ϣ 0������ 1����Բ��
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public int GetOXSet()
    {
        // ��ȡ��������
        if (OXToggleGroup == null)
        {
            throw new System.Exception("����ѡ�Ԥ����Ϊ��");
        }

        // ��ȡ��һ��ѡ�е�Toggle
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
                            index = 0; // ����
                            break;
                        case "XToggle":
                            index = 1; // Բ��
                            break;
                        case "RandomToggle":
                            index = 2; // ���
                            break;
                        default:
                            break;
                    }

                    return index;
                }
            }
        }        

        return 0; // Ĭ�Ϸ���0
    }
}
