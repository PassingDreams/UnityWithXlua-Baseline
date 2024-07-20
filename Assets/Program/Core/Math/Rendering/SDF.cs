using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ueels;

public class SDF 
{
    public struct Circle
    {
        public Vector2 center;
        public float r;
        public Circle(Vector2 center, float r)
        {
            this.center = center;
            this.r = r;
        }

        public float SignedDistance(Vector2 p)
        {
            return (p - center).magnitude - r;
        }
    }

    public struct Square
    {
        private Vector2 center;
        private Vector2 size;

        public Square(Vector2 center, Vector2 size)
        {
            this.center = center;
            this.size = size;
        }

        public float SignedDistance(Vector2 p)
        {
            p = p - center;
            Vector2 np=Mathv.Abs(p)-size*.5f;
            float o=(Mathv.MaxPiecewise(np,Vector2.zero)).magnitude;
            float i=Mathf.Min(0f,Mathf.Max(np.x,np.y));
            return o+i;
        }
        
    }
    
}
