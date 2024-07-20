namespace Ueels.Core
{
    public class Math
    {
        public static float Remap(float originMin,float originMax,float targetMin,float targetMax,float t)
        {
            return (t - originMin) / (originMax - originMin) * (targetMax - targetMin) + targetMin;
        }
        
        public static float Abs(float v)
        {
            return UnityEngine.Mathf.Abs(v);
        }
    }

}

