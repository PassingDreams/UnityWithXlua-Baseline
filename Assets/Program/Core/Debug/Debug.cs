using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace
Ueels.Core.Debug
{
        public static class Symbol
        {
            public const string DEBUG_LOG = "DEBUG_LOG";

        }

        #region 调试字符输出

        public static class Logger
        {



            [Conditional(Symbol.DEBUG_LOG)]
            private static void LogMethod(string s)
            {
                UnityEngine.Debug.Log(s);
            }
            
            
            
            public static void PrintParams(params object[] infos)
            {
                LogMethod(StringMaster.Params(infos));
            }

            public static void PrintCollection<T>(IEnumerable<T> collection)
            {
                LogMethod(StringMaster.Collection(collection));
            }

            public static void PrintError(object info)
            {
                LogMethod(StringMaster.Colorfy("Error: "+ info,"red"));
            }

            public static void PrintHint(object info)
            {
                LogMethod(StringMaster.Colorfy("Hint: "+ info,"#66ccff"));
            }

            public static void PrintWarning(object info)
            {
                LogMethod(StringMaster.Colorfy("Warning: "+ info,"yellow"));
            }
            
            public static void PrintDataClass<T>(T dataObj) where T : new()
            {
                UnityEngine.Debug.Log(Reflection.DataClassToString(dataObj));
                
            }


        }
        
        #endregion



    #region 图像绘制
    


        /// <summary>
        /// XZ平面调试绘制
        /// </summary>
        public class Drawer2D4XZ
        {
            public static Color color = Color.red;

            private static void DrawLine(Vector3 from,Vector3 to,float duration=0)
            {
                UnityEngine.Debug.DrawLine(from,to,color,duration);
            }

            public static void DrawRectangle(Vector3 center, float rightWidth = 1f, float upHeight = 1f,
                float leftWidth = 0, float downHeight = 0, float duration = 0)
            {
                Vector3 vfrom, vto;
                vfrom = new Vector3(-leftWidth, 0, -downHeight) + center;
                vto = vfrom;
                vto.z += upHeight + downHeight;
                DrawLine(vfrom,vto,duration);
                vfrom = new Vector3(rightWidth, 0, -downHeight) + center;
                vto = vfrom;
                vto.z += upHeight + downHeight;
                DrawLine(vfrom,vto,duration);

                vfrom = new Vector3(-leftWidth, 0, -downHeight) + center;
                vto = vfrom;
                vto.x += leftWidth + rightWidth;
                DrawLine(vfrom,vto,duration);
                vfrom = new Vector3(-leftWidth, 0, upHeight) + center;
                vto = vfrom;
                vto.x += leftWidth + rightWidth;
                DrawLine(vfrom,vto,duration);
            }

            public static void DrawRectangle(Vector3 center, Vector2 leftDown, Vector2 rightUp,float duration)
            {
                
                Vector2 c=new Vector2(center.x,center.z);
                leftDown = c-leftDown;
                rightUp -= c;
                DrawRectangle(center,rightUp.x,rightUp.y,leftDown.x,leftDown.y,duration);
                
            }
            
            public static void DrawSquare(Vector3 center,float width,float duration=0)
            {

                width /= 2;
                DrawRectangle(center,width,width,width,width,duration);
            }
            
            public static void DrawSquare(Vector2 center,float worldY,float width=0.1f,float duration=0)
            {

                width /= 2;
                Vector3 c=new Vector3(center.x,worldY,center.y);
                DrawRectangle(c,width,width,width,width,duration);
            }

            public static void DrawCellSpace(Vector3 original, Vector2 cellSize, Vector2 spaceSize,
                bool flipDrawByX = false, float duration = 0)
            {
                Vector2Int cellNum = Vector2Int.zero;
                cellNum.y = (int) (spaceSize.y / cellSize.y);
                cellNum.x = (int) (spaceSize.x / cellSize.x);
                DrawCellSpace(original, cellSize, cellNum, flipDrawByX, duration);
            }

            public static void DrawCellSpace(Vector3 original, Vector2 cellSize, Vector2Int cellNum,
                bool flipDrawByX = false, float duration = 0)
            {
                float spaceHeight = cellNum.y * cellSize.y;
                float spaceWidth = cellNum.x * cellSize.x;
                Vector3 vfrom, vto;
                int xFlip = flipDrawByX ? 1 : -1;
                for (int i = 0; i <= cellNum.x; i++)
                {
                    vfrom = new Vector3(i * cellSize.x, 0, 0) + original;
                    vto = vfrom;
                    vto.z += spaceHeight * xFlip;
                    DrawLine(vfrom,vto,duration);
                }

                for (int j = 0; j <= cellNum.y; j++)
                {
                    vfrom = new Vector3(0, 0, j * cellSize.y * xFlip) + original;
                    vto = vfrom;
                    vto.x += spaceWidth;
                    DrawLine(vfrom,vto,duration);
                }
            }
            
            

            
        }//-------------class end



            

        public class Drawer3D
        {
            public static Color color = Color.red;
            
            /// <summary>
            /// 
            /// </summary>
            /// <param name="original"></param>
            /// <param name="localPoints">局部坐标，当original取0时可理解为世界坐标</param>
            /// <param name="duration"></param>
            public static void DrawPath(Vector3 original, Vector3[] points, float duration = 0)
            {
                Vector3 vfrom, vto;
                for (int i = 0; i < points.Length - 1; i++)
                {
                    vfrom = points[i] + original;
                    vto = points[i + 1] + original;
                    UnityEngine.Debug.DrawLine(vfrom, vto, color, duration);
                }
            }

            public static void DrawLine(Vector3 start ,Vector3 end)
            {
                UnityEngine.Debug.DrawLine(start,end,color);
            }

            public static void DrawCube(Bounds bounds, float duration = 0)
            {
                float x = bounds.extents.x;
                float y = bounds.extents.y;
                float z = bounds.extents.z;
                Drawer2D4XZ.color = color;
                Drawer2D4XZ.DrawRectangle(bounds.center-Vector3.up*y,x,z,x,z,duration);
                Drawer2D4XZ.DrawRectangle(bounds.center+Vector3.up*y,x,z,x,z,duration);

                Vector3 up = 2 * y * Vector3.up;
                Vector3 p = bounds.min;
                DrawLine(p,p+up);
                p = bounds.min + 2 * x * Vector3.right;
                DrawLine(p,p+up);
                p = bounds.min + 2 * z * Vector3.forward;
                DrawLine(p,p+up);
                p = bounds.min + new Vector3(2 * x, 0, 2 * z);
                DrawLine(p,p+up);

            }
            
        }

            #endregion



            #region 异常管理

            

        //----------------------------------------异常管理------------------------------------//

        public class ThrowHelper
        {
            public enum EExceptionType
            {
                ArgumentOutOfRangeException,//越界
                EndlessLoopException,//死循环
                MethodNotImplementException,
                
            }

            public static void Throw(EExceptionType exception)
            {
                switch (exception)
                {
                    case EExceptionType.ArgumentOutOfRangeException:
                        Throw("ArgumentOutOfRangeException");
                        break;
                    case EExceptionType.EndlessLoopException:
                        Throw("EndlessLoopException");
                        break;
                    case EExceptionType.MethodNotImplementException:
                        Throw("MethodNotImplementException");
                        break;
                }
                
            }
            public static void Throw(string info)
            {
                throw new Exception(info+"Exception");
            }
            
        }
        
        
            #endregion





            #region 树结构分析

            

        //----------------------------------------树分析------------------------------------//

        public interface ITreeStruct<T>
        {
            /// <summary>
            /// 从孩子编号从零开始
            /// </summary>
            /// <param name="idx"></param>
            /// <returns></returns>
            T GetChild(int idx);
            int GetChildrenNum();
        }
        /// <summary>
        /// 啄木鸟，分析树形数据结构的工具
        /// </summary>
        public class TreePecker<T> where T: ITreeStruct<T>
        {
            private T bindRoot ;
            private StringBuilder stringBuilder;

            
            /// <summary>
            /// 
            /// </summary>
            /// <param name="rootNode"></param>
            /// <param name="branch">树的最大分支数量</param>
            public TreePecker(T rootNode)
            {
                bindRoot = rootNode;
                stringBuilder=new StringBuilder();
            }

            private  void DefaultPrintMethod(string s)
            {
                stringBuilder.Append(s);
            }

            public delegate void StringOutputer(string s);
            private void PrintRecursively(T node,int indent,StringOutputer printMethod)
            {
                for(int k=0;k<indent;k++)
                   printMethod("____________|");//占位符
                if (node != null)
                {
                   printMethod(node.ToString());
                   printMethod("\n");
                }
                else
                {
                   printMethod("null");
                   printMethod("\n");
                    return;
                }
                
                for (int i = 0; i < node.GetChildrenNum(); i++)//这里假定所有孩子都是从零起紧密排列的
                {
                    PrintRecursively(node.GetChild(i),indent+1,printMethod);
                }
                
            }
            public void PrintTree()
            {
                stringBuilder.Clear();
                DefaultPrintMethod("\n");
                PrintRecursively(bindRoot,0,DefaultPrintMethod);
                Logger.PrintHint(stringBuilder.ToString());
            }
            public void PrintTree(StringOutputer printMethod)
            {
                stringBuilder.Clear();
                printMethod("\n");
                PrintRecursively(bindRoot,0,printMethod);
                Logger.PrintHint(stringBuilder.ToString());
            }

        }
        
            #endregion
        
}
