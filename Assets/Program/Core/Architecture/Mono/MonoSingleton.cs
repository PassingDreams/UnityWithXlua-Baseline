using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger = Ueels.Core.Debug.Logger;

namespace Ueels.Core
{

    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>, new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Logger.PrintError("Empty MonoSingleton:" + typeof(T));
                }

                return instance;
            }
        }

        public MonoSingleton()
        {
            instance = (T) this;
        }
    }
}
