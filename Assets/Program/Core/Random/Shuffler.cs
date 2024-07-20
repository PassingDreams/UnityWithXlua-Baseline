using System.Collections.Generic;
using UnityEngine;

namespace Ueels
{
    
    public class Shuffler
    {
        /// <summary>
        /// 洗牌洗到一个新的List中
        /// </summary>
        /// <param name="dataset">数据来源</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> Shuffle<T>(IEnumerable<T> dataset)
        {
            List<T> res=new List<T>(dataset);
            ShuffleAutochthonous(res);
            return res;
        }

        /// <summary>
        /// 原地洗牌
        /// </summary>
        /// <param name="sourceNtarget"></param>
        /// <typeparam name="T"></typeparam>
        public static void ShuffleAutochthonous<T>(List<T> sourceNtarget)
        {
            for (int i = 0; i < sourceNtarget.Count-2; i++)
            {
                int cursor = UnityEngine.Random.Range(i, sourceNtarget.Count-1);
                T t = sourceNtarget[cursor];
                sourceNtarget[cursor] = sourceNtarget[i];
                sourceNtarget[i] = t;
            }
        }
        
    }

}
