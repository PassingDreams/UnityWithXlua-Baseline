using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenWorldParams //�Ѳ���
{
    public static float ScreenPixelWidth
    {
        get;
        private set;
    }
    public static float ScreenPixelHeight 
    {
        get;
        private set;
    }
    /// <summary>
    /// ��Ļ����߶�
    /// </summary>
    public static float ViewWorldHeight 
    {
        get;
        private set;
    }

    /// <summary>
    /// ��Ļ������
    /// </summary>
    public static float ViewWorldWidth 
    {
        get;
        private set;
    }


    static ScreenWorldParams()
    {
        ResetParam();
    }
    public static void ResetParam()
    {
        ScreenPixelWidth= Screen.width;
        ScreenPixelHeight = Screen.height;

        //��������xyƽ���2D�������Ϸƽ��zֵΪ0
        var hw = ScreenPixelWidth * .5f;
        var hh = ScreenPixelHeight * .5f;
        var rightUp=Camera.main.ScreenToWorldPoint(new Vector3(hw, hh, -Camera.main.transform.position.z));
        var leftDown=Camera.main.ScreenToWorldPoint(new Vector3(-hw, -hh, -Camera.main.transform.position.z));

        var dir = rightUp - leftDown;
        ViewWorldWidth = dir.x;
        ViewWorldHeight = dir.y;

    }

}
