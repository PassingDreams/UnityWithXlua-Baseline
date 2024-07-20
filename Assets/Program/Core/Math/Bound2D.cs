using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 public struct Bound2D
    {
        public Vector2 leftDown;//左下角位置
        public Vector2 rightUp;
        public Vector2 Center => (leftDown + rightUp) / 2f;
        public Vector2 Size => (rightUp - leftDown) ;

        public bool IsCanContain(Vector2 p)
        {
            return !(p.x < leftDown.x || p.x > rightUp.x || p.y < leftDown.y || p.y > rightUp.y);
        }
        
        /// <summary>
        /// 可以完全包含这个区域，而不是相交或相离
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public bool IsCanContain(Bound2D area)
        {
            return leftDown.x < area.leftDown.x &&
                   leftDown.y < area.leftDown.y &&
                   rightUp.x > area.rightUp.x &&
                   rightUp.y > area.rightUp.y;
        }
        
        
        /// <summary>
        /// 即判断两区域没有重合部分
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public bool IsOutOf(Bound2D area)
        {
            return rightUp.x<area.leftDown.x||
                    leftDown.x>area.rightUp.x||
                    rightUp.y<area.leftDown.y||
                    leftDown.y>area.rightUp.y;
        }


        public Bound2D(Vector2 leftDown, Vector2 rightUp)
        {
            this.leftDown = leftDown;
            this.rightUp = rightUp;
        }

        public float SignedDistance(Vector2 p)
        {
            return new SDF.Square(Center, Size).SignedDistance(p);
        }
        
    }
    
