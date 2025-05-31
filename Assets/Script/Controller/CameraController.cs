using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    private Camera cCamera;

    /// <summary>
    /// 设置摄像机位置
    /// </summary>
    /// <param name="pos"></param>
    public void SetCameraPosition(Vector3 pos)
    {
        if (cCamera == null)
        {
            cCamera = Camera.main;
            if (cCamera == null)
            {
                throw new System.Exception("当前没有摄像机");
            }
        }
        
        // 设置坐标
        cCamera.transform.position = new Vector3(pos.x , cCamera.transform.position.y, pos.z);    
    }

    /// <summary>
    /// 设置相机尺寸
    /// </summary>
    /// <param name="size"></param>
    /// <exception cref="System.Exception"></exception>
    public void SetCameraSize(float size)
    {
        if (cCamera == null)
        {
            cCamera = Camera.main;
            if (cCamera == null)
            {
                throw new System.Exception("当前没有摄像机");
            }
        }

        cCamera.orthographicSize = size;
    }
}
