using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    private Camera cCamera;

    /// <summary>
    /// ���������λ��
    /// </summary>
    /// <param name="pos"></param>
    public void SetCameraPosition(Vector3 pos)
    {
        if (cCamera == null)
        {
            cCamera = Camera.main;
            if (cCamera == null)
            {
                throw new System.Exception("��ǰû�������");
            }
        }
        
        // ��������
        cCamera.transform.position = new Vector3(pos.x , cCamera.transform.position.y, pos.z);    
    }

    /// <summary>
    /// ��������ߴ�
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
                throw new System.Exception("��ǰû�������");
            }
        }

        cCamera.orthographicSize = size;
    }
}
