using System.Collections;
using System.Collections.Generic;
using Ueels.Core;
using UnityEngine;

public class GasRunner : Singleton<GasRunner>
{
    public static LocalServer localServer;

    public void StartServerThreads()
    {
        localServer = new LocalServer();
        localServer.Listen();
        
    }
}
