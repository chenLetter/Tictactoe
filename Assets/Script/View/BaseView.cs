using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseView : MonoBehaviour
{
    /// <summary>
    /// �򿪽���
    /// </summary>
    public virtual void OpenView()
    {
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// �رս���
    /// </summary>
    public virtual void CloseView()
    {
        this.gameObject.SetActive(false);
    }
}
