using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseView : MonoBehaviour
{
    /// <summary>
    /// 打开界面
    /// </summary>
    public virtual void OpenView()
    {
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    public virtual void CloseView()
    {
        this.gameObject.SetActive(false);
    }
}
