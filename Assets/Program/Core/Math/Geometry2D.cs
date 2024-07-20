
using UnityEngine;

namespace Ants.Utilities
{
    public class Geometry2D
    {
        public static TangentInfo2D getTangentInfo(Vector2 p, Sphere2D s, bool isClockWise = false)
        {
            float c = (s.center - p).magnitude, b = s.radius;
            float a = Mathf.Sqrt(c * c - b * b);
            Complex delta=new Complex(a,b);
            Complex pA=new Complex(s.center-p);
            Vector2 pC;
            if (!isClockWise)
            {
                pC= (pA * delta.normalized).toVec2;
            }
            else
            {
                pC= (pA / delta.normalized).toVec2;    
            }
            
            return  new TangentInfo2D(p+pC,pC.normalized);
        }
        
        public static void getTangentInfoPair(Vector2 p, Sphere2D s, out TangentInfo2D clockWise,out TangentInfo2D antiClockWise)
        {
            float c = (s.center - p).magnitude, b = s.radius;
            float a = Mathf.Sqrt(c * c - b * b);
            Complex delta=new Complex(a,b);
            Complex pA=new Complex(s.center-p);
            Vector2 pC;

                pC= (pA * delta.normalized).toVec2;
                antiClockWise.worldPos = p + pC;
                antiClockWise.dir = pC.normalized;
                pC= (pA / delta.normalized).toVec2;    
                clockWise.worldPos = p + pC;
                clockWise.dir = pC.normalized;
        }
    }

}

