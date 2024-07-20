using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ants.Utilities
{
    public struct Sphere2D
    {
        public Vector2 center;
        public float radius;

        public Sphere2D(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }
    }

    public struct TangentInfo2D
    {
        public Vector2 worldPos;
        public Vector2 dir;
        public TangentInfo2D(Vector2 position,Vector2 dir)
        {
            this.worldPos = position;
            this.dir = dir;
        }
    }
    

}

