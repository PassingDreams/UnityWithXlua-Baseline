using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrustumCuller  
{
    
    private  Camera camera;
    private  Plane[] frustumPlanes=new Plane[6];

    public FrustumCuller(Camera camera)
    {
        this.camera = camera;
    }

    public enum EInsideSituation
    {
        Inner,
        Outter,
        Intersected,
    }

    public void UpdateFrustumPlanes()
    {
        //https://docs.unity3d.com/cn/2017.3/ScriptReference/GeometryUtility.CalculateFrustumPlanes.html
        /*
        此函数采用给定摄像机的视椎体，然后返回形成此视椎体的六个平面。
        排序：[0] = 左、[1] = 右、[2] = 下、[3] = 上、[4] = 近、[5] = 远
        其中平面的法线都是朝向视锥内侧的(已测试)
        而平面的distance是指平面方程 ax+by+cz+d=0中的d，当使用点乘式时应有意使用-distance来代替，即ax+by+cz=-d=-distance
        */
        GeometryUtility.CalculateFrustumPlanes(camera, frustumPlanes);

    }


    /// <summary>
    /// 点剔除
    /// </summary>
    public EInsideSituation InsideCheck(Vector3 point)
    {
        for (int i = 0; i < frustumPlanes.Length; i++)
        {
            var plane = frustumPlanes[i];
            //令n * p= d
            var normal = plane.normal;//朝内侧
            var d = -plane.distance;

            var pd = Vector3.Dot(point, normal);

            if (pd < d) //法线的指向是等值(等d)面增长的方向
                return EInsideSituation.Outter;

        }

        return EInsideSituation.Inner;
    }
    
    /// <summary>
    /// 球状(包围球)剔除
    /// </summary>
    /// 
    public EInsideSituation InsideCheck(Vector3 center,float radius)
    {
        bool isAllInside = true;
        for (int i = 0; i < frustumPlanes.Length; i++)
        {
            var plane = frustumPlanes[i];
            //令n * p= d
            var normal = plane.normal;//朝内侧
            var d = -plane.distance;
            
            var pd = Vector3.Dot(center, normal);
            
            //法线的指向是等值(等d)面增长的方向
            if (pd < d-radius) //只要检测到球在一个面的不相交完全外侧，那么球一定在视锥外侧
                return EInsideSituation.Outter;

            if (!(pd > d + radius)) //则 非 完全内侧，若不存在完全外侧的面，但是存在一个不完全内侧，则证明相交发生
                isAllInside = false;
        }

        return isAllInside ? EInsideSituation.Inner: EInsideSituation.Intersected;
    }

    public EInsideSituation InsideCheck(Bounds bounds)
    {
        return GeometryUtility.TestPlanesAABB(frustumPlanes, bounds)? EInsideSituation.Inner: EInsideSituation.Outter;

    }

}
