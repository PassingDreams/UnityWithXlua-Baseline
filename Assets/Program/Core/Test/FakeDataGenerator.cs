using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ueels.Core.Test
{
    public class FakeDataGenerator 
    {
        public static List<Vector2> Vector2List(int num,Vector2 xRange,Vector2 yRange,uint seed=0)
        {
            List<Vector2> l=new List<Vector2>(num);
            for (uint i = 0; i < num; i++)
            {
                Vector2 v=Hasher.Hash13(i + seed);
                v.x = Math.Remap(v.x, 0, 1, xRange.x, xRange.y);
                v.y = Math.Remap(v.y, 0, 1, yRange.x, yRange.y);
                l.Add(v);

            }
            return l;
        }
        
    }
}
