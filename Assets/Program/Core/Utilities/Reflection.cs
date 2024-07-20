using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Ueels
{
    
    /// <summary>
    /// 反射工具箱
    /// </summary>
    public static class Reflection 
    {
        
            public static string DataClassToString<T>(T data)
            {

                var stringBuilder = PoolManager.Depool<StringBuilder>();
                stringBuilder.Clear();
                var t = typeof(T);
                var fileds = t.GetFields();
                stringBuilder.Append("Class : ");
                stringBuilder.Append(t.Name+ "\n");
                foreach (var fieldInfo in fileds)
                {
                    stringBuilder.Append(fieldInfo.FieldType.Name+"  "+fieldInfo.Name+" = ");
                    stringBuilder.Append(fieldInfo.GetValue(data)+"\n");
                }

                string s = stringBuilder.ToString();
                PoolManager.Enpool(stringBuilder);
                return s;
            }
    }

}
