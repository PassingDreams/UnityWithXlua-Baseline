
using UnityEngine;
using Ueels;

public static class Noise2D 
{

    private static float Floor(float t)
    {
        return Mathf.Floor(t);

    }

    private static Vector2 Floor(Vector2 v)
    {
        return new Vector2(Floor(v.x),Floor(v.y));
    }

    private static float Lerp(float a, float b, float t)
    {
        return Mathf.Lerp(a, b, t);
    }

    private static float Dot(Vector2 a, Vector2 b)
    {
        return Vector2.Dot(a, b);
    }
    
    public static Vector2 RandomUnitVector2(Vector2 uv,float seed=0)
    {
        float t = Mathf.PI * 2f * Hasher.Hash21(uv,seed);
       return new Vector2(Mathf.Cos(t),Mathf.Sin(t));
    }
    
    
    /// <summary>
    /// 值大部分处于±0.5,且集中于0，即中部范围
    /// </summary>
    /// <param name="uv">传入uv时，采样步距越接近0结果越平滑,且采样步距总是应该小于1</param>
    /// <returns>range(0,1)</returns>
    /// https://www.shadertoy.com/view/flsXz7
   public static float PerlinNoise(Vector2 uv,float seed=0)
   {
       Vector2 id=Floor(uv);uv=uv-id;
       float a=Dot(uv-new Vector2(0,0),RandomUnitVector2(id+new Vector2(0,0),seed)),
             b=Dot(uv-new Vector2(1,0),RandomUnitVector2(id+new Vector2(1,0),seed)),
             c=Dot(uv-new Vector2(0,1),RandomUnitVector2(id+new Vector2(0,1),seed)),
             d=Dot(uv-new Vector2(1,1),RandomUnitVector2(id+new Vector2(1,1),seed));
       uv=uv*uv*(new Vector2(3,3)-2f*uv);//f'(0)=f'(1)=0
       a=Lerp(a,b,uv.x);b=Lerp(c,d,uv.x);
       c = Lerp(a, b, uv.y) *1.1f;
       //print(c);
       return c;
   }

    /// <summary>
    /// 开销巨大，避免持续使用
    /// 采样步距应小于1
    /// </summary>
    /// <param name="uv"></param>
    /// <param name="level"></param>
    /// <returns>range(0,1)</returns>
    public static float FBM(Vector2 uv,float level=4,float seed=0)
    {
           float res=0f,r=1f,nml=0;
           for (int i = 0; i < level; i++)
           {
               res += PerlinNoise(uv / r,seed+160f*r)*r;
               nml += r;
               r /= 2;
           }
           return res/nml; 
    }

}

public static class Hasher
{
    /// <summary>
    /// 取小数
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static float Fract(float x)
    {
        return x-(int)x ;
    }

    public static Vector2 Fract(Vector2 v)
    {
        return new Vector2(Fract(v.x),Fract(v.y));
    }

    /// <summary>
    /// 取正小数
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static float Mod(float x)
    {
        return x - Mathf.Floor(x);

    }
    public static Vector3 Mod(Vector3 v)
    {
        return new Vector3(Mod(v.x),Mod(v.y),Mod(v.z));
    }
    
    /*--------------------------sin式-------------------------*/
    //返回0-1范围
    public static float Hash21(Vector2 v,float seed=0)//待改进
    {
        
        v *= 1.32425f;
        float pseed = Mathf.Max(1f, seed);
        v+=new Vector2(3,181-seed);
        return Fract(Mathf.Sin(83.231f * v.x) * v.y* ((44.23141f+seed) - v.y * 5f*pseed))*.5f+.5f;
    }
   public static Vector3 Hash33(Vector3 v)
   {
        v = new Vector3( Vector3.Dot(v,new Vector3(127.1f,311.7f, 74.7f)),
                  Vector3.Dot(v,new Vector3(269.5f,183.3f,246.1f)),
                  Vector3.Dot(v,new Vector3(113.5f,271.9f,124.6f)));
   
   	return Mod(Mathv.SinPiecewise(v)*43758.5453123f);
   }

    /*----------------------移位式--------------------------*/
    
//    public static Vector3 Hash33(Vector3 v)
//    {
//        const uint k = 1103515245U;
//        v = ((v>>8)^v.yzx)*k;
//        v = ((v>>8U)^v.yzx)*k;
//        v = ((v>>8U)^v.yzx)*k;
//    
//        return v*(1f/0xffffffffU); 
//        
//    }
    
    /*-------------------------其他--------------------------*/
    public static Vector3 Hash13( uint n ) 
    {
        // integer hash copied from Hugo Elias
        //ref to https://www.shadertoy.com/view/llGSzw 
        n = (n << 13) ^ n;
        n = n * (n * n * 15731U + 789221U) + 1376312589U;
        n *= n;
        uint x = n;
        uint y = n*16807U;
        uint z = n*48271U;
        Vector3 v=Vector3.zero;
        v.x = (x & 0x7fffffffU) / (float) (0x7fffffff);
        v.y = (y & 0x7fffffffU) / (float) (0x7fffffff);
        v.y = (z & 0x7fffffffU) / (float) (0x7fffffff);
        return v;
    }
    
    
    // UE4's RandFast function
    // https://github.com/EpicGames/UnrealEngine/blob/release/Engine/Shaders/Private/Random.ush
    public static float FastHash21(Vector2 v,float seed=0)
    {
        seed = Mathf.Max(seed, 1);//customized
        
        v = (1f/4320f) * v + new Vector2(0.25f,0f);
        float state = Mod( Vector2.Dot( v * v, Vector2.one*3571));
        return Mod( state * state * (3571f * (2f+seed)));
    }
    
}
