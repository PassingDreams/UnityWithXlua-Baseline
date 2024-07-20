
using Unity.VisualScripting;
using UnityEngine;

namespace Ueels
{
    
    public class Mathv //math for vectors
    {
        public const float EPS = 1e-4f;

        public static Vector3 SinPiecewise(Vector3 v)
        {
            return new Vector3(Mathf.Sin(v.x),Mathf.Sin(v.y),Mathf.Sin(v.z));
        }
        
        public static bool IsZero(Vector2 v,float threshold=EPS)
        {
            return SqrLength( v) < threshold*threshold;

        }


        public static float SqrLength(Vector2 v)
        {
            return Vector2.Dot(v, v);
        }
        public static float SqrLength(Vector3 v)
        {
            return Vector3.Dot(v, v);
        }

        public static Vector2 MaxPiecewise(Vector2 a, Vector2 b)
        {
            return new Vector2(Mathf.Max(a.x,b.x),Mathf.Max(a.y,b.y));
        }
        public static Vector2 MinPiecewise(Vector2 a, Vector2 b)
        {
            return new Vector2(Mathf.Min(a.x,b.x),Mathf.Min(a.y,b.y));
        }

        public static Vector2 Abs(Vector2 v)
        {
            return new Vector2(Mathf.Abs(v.x),Mathf.Abs(v.y));
        }


        /// <summary>
        /// 判断u和v的距离是否小于dist
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="dist"></param>
        /// <returns></returns>
        public static bool IsCloseEnough2D(Vector2 u, Vector2 v, float dist)
        {
            Vector2 d = u - v;
            return SqrLength( d) < dist * dist;
        }

        public static bool IsCloseEnough3DIgnoreY(Vector3 u, Vector3 v, float dist)
        {
            u.y = v.y = 0;
            Vector3 d = u - v;
            return SqrLength( d) <= dist * dist;

        }

        public static int ManhattanDistance(Vector2Int a, Vector2Int b)
        {
            var dir = b - a;
            return Mathf.Abs(dir.x) + Mathf.Abs(dir.y);
        }
    }

}
