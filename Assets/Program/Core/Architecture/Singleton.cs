using UnityEngine;
using Logger = Ueels.Core.Debug.Logger;
namespace Ueels.Core
{

    public class Singleton<T> : object where T: Singleton<T>,new()
    {
            private static T instance;
        
            public static T Instance
            {
                get
                {
                    if (instance == null)
                    {
                        Logger.PrintError("Empty Singleton:" + typeof(T));
                        instance = new T();
                    }
        
                    return instance;
                }
            }
        
            public Singleton()
            {
                instance = (T) this;
            }
    }

}
