using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramManager : MonoBehaviour
{
    private void Awake()
    {
        //运行服务线程
        GasRunner.Instance.StartServerThreads();
        
    }

    void OnApplicationQuit()
    {
        Debug.Log("程序退出中...");
        GasRunner.Instance.Close();
        ThreadPool.Instance.StopAll();
    }
}
