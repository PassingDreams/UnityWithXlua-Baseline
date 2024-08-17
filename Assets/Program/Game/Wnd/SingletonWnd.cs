using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonWnd<T> : WndBase where T:SingletonWnd<T>,new()
{
    private static T instance;
    
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("Empty Singleton:" + typeof(T));
            }
    
            return instance;
        }
    }
    public SingletonWnd()
    {
        instance = (T) this;
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

}
