using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class WndBase : MonoBehaviour
{
    protected Transform GetChildByName(string path)
    {
        var nodes=path.Split(".");
        Transform curNode=transform;
        for (int i = 0; i < nodes.Length; i++)
        {
            var nodeStr = nodes[i];
            var nextNode = curNode.Find(nodeStr);
            if (!nextNode) { return null; }
            curNode = nextNode;
        }
        return curNode;
    }
    protected void SetVisible(bool bVis)
    {
        gameObject.SetActive((bVis));
    }
    
}
