using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Ueels.Core.Debug;
using Ueels.Core.Test;
using UnityEngine;
using Logger = Ueels.Core.Debug.Logger;


namespace Ueels.Core
{
    public interface IQuadTreePartitionInfoCheck
    {
        Vector2 GetPosition2D();//返回四叉树中的空间位置
    }
    public class QuadTree<T> where T:  IQuadTreePartitionInfoCheck
    {
        #region struct_Bound2D

        public struct Bound2D
        {
            public Vector2 leftDown;//左下角位置
            public Vector2 rightUp;
            public Vector2 Center => (leftDown + rightUp) / 2f;
            public float Width => rightUp.x - leftDown.x;

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

            public static void Doublefy(Bound2D bound)//将边界扩张到原来的二倍
            {
                Vector2 center = bound.Center;
                bound.leftDown = center + (bound.leftDown - center) * 2;
                bound.rightUp = center + (bound.rightUp - center) * 2;
            }
            
            public static Bound2D Quadrant1st(Bound2D bound)//第一象限
            {
                return new Bound2D(Vector2.zero, bound.rightUp);
            }
            
            
            /// <summary>
            /// 返回区域的第三象限
            /// </summary>
            /// <param name="bound"></param>
            /// <returns></returns>
            public static Bound2D Quadrant3rd(QuadTree<_QuadTreeTestDataStruct>.Bound2D bound)//第三象限
            {
                return new Bound2D(bound.leftDown,Vector3.zero);
            }

            public Bound2D(Vector2 leftDown, Vector2 rightUp)
            {
                this.leftDown = leftDown;
                this.rightUp = rightUp;
            }
            public Bound2D(Vector2 center, float cellWidth)
            {
                cellWidth *= .5f;//half it
                Vector2 off=Vector2.one*cellWidth;
                this.leftDown = center - off;
                this.rightUp = center + off;
            }
            /// <summary>
            /// 得到点在bound以中心为原点的坐标系中的象限序号
            /// </summary>
            /// <param name="pos"></param>
            /// <returns>返回这个坐标在Bound中的象限，象限序号采取笛卡尔坐标系的约定</returns>
            public int MapQuadrant(Vector2 pos)
            {
                pos -= Center;
                if (pos.x > 0)
                    return pos.y > 0 ? 1 : 4;
                else
                    return pos.y > 0 ? 2 : 3;//注意（0,0）点被归入第三象限

            }
        }
        

        #endregion

        #region class_Node 

        

        public class Node :ITreeStruct<Node> 
        {
            public Bound2D Bound;
            public T val=default;
            public Node father;
            public List<Node> children=default;//非叶节点存储孩子，叶子结点存储数据,四个节点分别对应四个象限

            
            
            
            public Node(Bound2D bound)
            {
                Bound = bound;
            }
            public Node(T data)
            {
                val = data;
            }
            

            /// <summary>
            /// 判断在给定阈值限定的条件下该节点是否已经是叶子
            /// </summary>
            /// <param name="node"></param>
            /// <param name="cellMinimumWidthThreshold"></param>
            /// <returns></returns>
            public static bool IsLeaf(Node node, float cellMinimumWidthThreshold = 1)
            {
                return node.Bound.Width / 2 < cellMinimumWidthThreshold;//不可再向下细分了,说明已经是叶子结点了
            }
            
            /// <summary>
            /// 
            /// </summary>
            /// <param name="data"></param>
            /// <param name="cellMinimumWidthThreshold">空间划分最小单元格尺寸</param>
            public bool Insert(T data,float cellMinimumWidthThreshold=1)
            {
                var pos = data.GetPosition2D();
                if(!Bound.IsCanContain(pos)) return false;//界外
                
                
                if(IsLeaf(this,cellMinimumWidthThreshold))//如果是叶子结点，数据直接存入children列表,需要注意：叶子的val总是空的
                {
                    if(children==null)
                        children=new List<Node>(3);

                    var newnode = new Node(data);
                    newnode.Bound=new Bound2D(data.GetPosition2D(),1e-6f);//暂时先将叶子结电的挂载数据的边界处理为小方形
                    newnode.father = this;
                    children.Add(newnode);
                    return true;

                }
                else if (val == null)//不是叶子，就先存到val中
                {
                    
                    val = data;
                    return true;
                }
                else//val插不下就插入子节点
                {
                    if (children == null)
                    {
                        children=new List<Node>(4);
                        children.Add(null);
                        children.Add(null);
                        children.Add(null);
                        children.Add(null);

                    }
                    
                    Vector2 off=new Vector2(1,0)*Bound.Width/2;
                    int quadrant = Bound.MapQuadrant(pos);//查询该节点位于Bound的哪一象限
                    if (children[quadrant - 1] == null) //空孩子则构造
                    {
                        
                        switch (quadrant)
                        {
                            case 1:
                                children[quadrant-1] = new Node(new Bound2D(Bound.Center, Bound.rightUp));
                                break;
                            case 2:
                                children[quadrant-1] = new Node(new Bound2D(Bound.Center-off, Bound.rightUp-off));
                                break;
                            case 3:
                                children[quadrant-1]  = new Node(new Bound2D(Bound.leftDown, Bound.Center));
                                break;
                            case 4:
                                children[quadrant-1]  = new Node(new Bound2D(Bound.leftDown+off,Bound.Center+off));
                                break;
                                
                            
                        }
                        children[quadrant - 1].father = this;
                    }
                    children[quadrant - 1].Insert(data, cellMinimumWidthThreshold);
                }

                return true;
            }

            /// <summary>
            /// 找到区域内所有对象
            /// </summary>
            /// <param name="area"></param>
            /// <param name="outputBuffer"></param>
            public void Search(Bound2D area, List<T> outputBuffer)
            {
                if(Bound.IsOutOf(area)) return;//无交集
                
                //区域完全包含区域（算法优化）
                if (area.IsCanContain(Bound))
                {
                    Extract(this,outputBuffer);
                    return;
                }
                
                
                if(val!=null&&area.IsCanContain(val.GetPosition2D())) outputBuffer.Add(val);//点包含测试
                
                if(children!=null)
                    foreach (var node in children)
                        node?.Search(area,outputBuffer);
                        
            }
            
            
            /// <summary>
            /// 带过滤器的搜索方法，过滤掉不匹配的对象
            /// </summary>
            /// <param name="area"></param>
            /// <param name="outputBuffer"></param>
            /// <param name="matchMethod">不符合匹配条件的对象会被忽略掉</param>>
            public void SearchWithFilter(Bound2D area, List<T> outputBuffer,Predicate<T> matchMethod)
            {
                if(Bound.IsOutOf(area)) return;//无交集
                
                //区域完全包含区域（算法优化）
                if (area.IsCanContain(Bound))
                {
                    ExtractWithFilter(this,outputBuffer,matchMethod);
                    return;
                }
                
                
                if(val!=null&&area.IsCanContain(val.GetPosition2D())&& matchMethod(val) ) outputBuffer.Add(val);//点包含测试
                
                if(children!=null)
                    foreach (var node in children)
                        node?.SearchWithFilter(area,outputBuffer,matchMethod);
                        
            }
            

            /// <summary>
            /// 提取某个节点下所有有效数据
            /// </summary>
            /// <param name="outputBuffer"></param>
            private static void Extract(Node node,List<T> outputBuffer)
            {
                if(node==null) return;
                
                if(node.val!=null) 
                {
                    outputBuffer.Add(node.val);
                }
                
                if(node.children!=null)
                    foreach (var n in node.children)
                        Extract(n,outputBuffer);
            }
            
            private static void ExtractWithFilter(Node node,List<T> outputBuffer,Predicate<T> matchMethod)
            {
                if(node==null) return;
                
                if(node.val!=null&&matchMethod(node.val) ) 
                {
                    outputBuffer.Add(node.val);
                }
                
                if(node.children!=null)
                    foreach (var n in node.children)
                        ExtractWithFilter(n,outputBuffer,matchMethod);
            }

            //--------------------------------------匹配-------------------------------------//
            /// <summary>
            /// 以哈希码为判断一致的依据,带剔除地查找是否包含此元素
            /// </summary>
            /// <param name="data">是否包含该元素</param>
            /// <param name="cellMinW">空间划分阈值</param>
            /// <returns></returns>
            public bool Contains(T data,float cellMinW)
            {
                if (val != null && val.GetHashCode() == data.GetHashCode()) return true;


                if (IsLeaf(this, cellMinW))//是叶子,需要特殊处理
                {
                    if(children!=null)
                        foreach (var n in children)
                        {
                            if (n.val.GetHashCode() == data.GetHashCode()) return true;
                        }
                    
                }
                else//不是叶子
                {
                    var child = children[Bound.MapQuadrant(data.GetPosition2D()) - 1];//四叉树叶子剔除

                    return child == null ? false : child.Contains(data, cellMinW);

                }

                return false;
            }
            
            /// <summary>
            /// 以哈希码为判断一致的依据,带空间剔除地查找是否包含此元素
            /// 特别注意，如果物体在移动时未删除四叉树中的节点，会有可能导致该节点丢失，即节点在树中，但由于位置剔除而找不到该节点。
            /// </summary>
            /// <param name="data"></param>
            /// <returns>是否成功删除</returns>
            public bool Remove(T data,float cellMinW)
            {
                
                if (val != null && val.GetHashCode() == data.GetHashCode())//成功找到数据
                {
                    val = default(T);
                    return true;
                }


                if (IsLeaf(this, cellMinW))//是叶子,需要特殊处理
                {
                    Logger.PrintParams("check leaf");
                    if (children != null)
                    {
                    Logger.PrintCollection(children);
                        int idx = children.FindIndex((t) => t.val.GetHashCode() == data.GetHashCode());//findindex这个函数找不到元素时会返回负一
                        if (idx < 0) return false;
                        children.RemoveAt(idx);
                        return true;
                    }
                    
                }
                else//不是叶子,则根据划分递归寻找其中一个叶子
                {
                    var child = children[Bound.MapQuadrant(data.GetPosition2D()) - 1];//四叉树叶子剔除

                    return child == null ? false : child.Remove(data, cellMinW);

                }

                return false;
            }
            
            
            
            
            
            
            
            
            /// <summary>
            /// 以Predicate匹配函数为判断一致的依据,带空间剔除地查找是否包含符合条件的元素,
            /// 只要有一个符合条件的就算包含，就立刻返回true
            /// 但是如果搜索的这片区域实际没有包含符合条件的节点，执行这个方法就相当于完全遍历并tryMatch了一遍所有节点，开销是不小得，这是需要注意的
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public bool Contains(Bound2D area,Predicate<T> matchMethod,float cellMinW)
            {
                if(Bound.IsOutOf(area)) return false;//无交集
                
                
                //区域完全包含区域（算法优化）,此时就不用进行边界检查了，直接看所有孩子是否满足条件就行了
                if (area.IsCanContain(Bound))
                {
                    Stack<Node> stack=new Stack<Node>();//模拟递归
                    stack.Push(this);
                    while (!stack.IsEmpty())
                    {
                        var cur = stack.Pop();
                        if(cur.val!=null&& matchMethod(cur.val)) return true;//只要发现有一个匹配的，直接返回
                        
                        if(children!=null)
                            foreach (var ch in children)
                            {
                                if(ch!=null)
                                    stack.Push(ch);
                            }

                    }
                    return false;//栈空但是仍然没找到，返回false
                }
                
                if (val != null &&//先查看自身挂载值
                     area.IsCanContain(val.GetPosition2D())&&//包含测试
                     matchMethod(val)) return true;//额外条件符合测试

                if (IsLeaf(this, cellMinW))//是叶子,需要特殊处理
                {
                    if(children!=null)
                        foreach (var n in children)
                        {
                            if (area.IsCanContain(n.val.GetPosition2D())&&//区域可以包含这个孩子
                                matchMethod(n.val)) return true;//这个孩子还符合挑选方法
                        }
                    
                }
                else//不是叶子
                {
                    if(children!=null)//查看孩子
                        foreach (var node in children)
                            if (node!=null&&node.Contains(area, matchMethod, cellMinW))//只要有一个孩子包含就算本节点包含
                                return true;
                }

                return false;
                
                
                
                
                        
            }


            /*
            /// <summary>
            /// 查找最近邻元素，这个方法在查找近邻方法的基础上进行最值挑选即可
            /// </summary>
            /// <param name="pos"></param>
            /// <returns></returns>
            public T GetNearestNeighbor(Vector2 pos)
            {

            }
            */
            
            
            
            /// <summary>
            /// 查找带有额外条件的近邻（不一定是最近）
            /// 使用二分查找方法,近邻查找
            /// 这个方法仅当树中存储数据特别多时效果好，当数据较少时不推荐使用(还不如直接把所有节点存在List里面遍历快)
            /// </summary>
            /// <param name="pos"></param>
            /// <param name="searchStopThereshold">二分搜索的停止阈值，同时也是最近邻允许的距离误差(就是说结果可能不是最近，但是相比最近邻距离不会超过这个值),该值必须大于0，越大则停止迭代越快</param>
            /// <returns>返回终止时刻收敛的搜索区域边长</returns>
            /// 配置项：
            const int MAX_NEAR_NEIGHBOR_SEARCH_TIMES = 20;//防止过度搜索
            public float FindNearNeighbor(Vector2 pos,Predicate<T> matchMethod,List<T> outputBuffer,float cellMinW,float searchStopThereshold)
            {
                
                //初始化，以pos为中心的单元格
                float l = cellMinW;//下限,下限之内无目标对象
                float h = float.MaxValue;//上限,上限之外无目标对象
                float c = l;//当前
                Bound2D bound=new Bound2D(pos,c);

                for(int i=0;i<MAX_NEAR_NEIGHBOR_SEARCH_TIMES;i++)
                {
                    if (Contains(bound, matchMethod, cellMinW))//如果包含符合这个条件的节点，尝试压缩边界
                    {
                        h = c;//压缩上限

                        if (Mathf.Abs(h - l) <= searchStopThereshold)//卡准误差，可以提交对象
                        {
                            SearchWithFilter(bound,outputBuffer,matchMethod);
                            Logger.PrintParams("find num:"+outputBuffer.Count);
                            return bound.Width;
                        }

                        c = (h + l)/2f;//重新定位

                    }
                    else if(bound.IsCanContain(Bound))//如果已经包围了整个根区域，还不能找到符合条件的对象 ，那说明根本不存在这样的对象
                    {
                        return c;
                    }
                    else//否则扩大这个区域继续搜
                    {
                        l = c;//提升下限
                        if (Math.Abs(h-float.MaxValue) <= .001f) //说明还没找到过数据
                        {
                            c *= 2;//暴力扩张边界
                        }
                        else
                        {
                            c = (h + l) / 2f;//重新二分

                        }

                    }
                    
                    //更新边界
                    bound=new Bound2D(bound.Center,c+searchStopThereshold);//加一个误差值是为了避免物体刚好压住h边界的情况使得二分无法停止
                }//loop end
                
                Logger.PrintError("过渡搜索");
    //            ThrowHelper.Throw(ThrowHelper.EExceptionType.EndlessLoopException);
                return c;
            }
            
            
            
            
            
            
            /// <summary>
            /// 遍历全树，弹出匹配元素
            /// </summary>
            /// <param name="matchMethod"></param>
            public void PopMatched(List<T> outputBuffer,Predicate<T> matchMethod)
            {
                if (val != null && matchMethod(val))
                {
                    outputBuffer.Add(val);
                    val = default;//null
                }

                if (children != null)
                {
                    foreach (var n in children)
                    {
                        n.PopMatched(outputBuffer,matchMethod);
                    }
                    
                }
            }


            /*-------------------接口方法----------------------*/
            public Node GetChild(int idx)
            {
                if (children == null) return null;
                else if(idx >= children.Count)
                {
                    ThrowHelper.Throw(ThrowHelper.EExceptionType.ArgumentOutOfRangeException);
                    return null;
                }
                
                return children[idx];

            }

            public int GetChildrenNum()
            {
                if (children == null) return 0;
                return children.Count;
            }

            public override string ToString()
            {
                if (val == null) return "Empty-val Node";
                return val.ToString();
            }
        }//--------------------------------class 四叉树节点 End-----------------------

        #endregion
        
        private Node root;
        public Node Root => root;
        public Vector2 Center => Root.Bound.Center;

        private int count = 0;
        public int Count => count;

        //配置
        private float cellMinimumWidth;
        
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftDown">init bound</param>
        /// <param name="rightUp">init bound</param>
        /// <param name="cellMinimumWidth">划分粒度不会小于这个边长宽度的格子</param>
        /// <param name="GetPositionMethod">映射泛型T到位置的方法</param>
        public QuadTree(Vector2 leftDown,Vector2 rightUp,float cellMinimumWidth)
        {
            root=new Node(new Bound2D(leftDown,rightUp));
            treePecker=new TreePecker<QuadTree<T>.Node>(root);//绑定啄木鸟、调试用
            this.cellMinimumWidth = cellMinimumWidth-EPS;//加一个阈值，可以容错小数点精度问题，从而保证恰好取到阈值
            
            _PerformanceAnalyze();
        }

        private const float EPS = 1e-6f;


        public void Insert(T data)
        {
            if(root.Insert(data,cellMinimumWidth))
                count++;
        }
        
        public void Search(Bound2D area,List<T> outputBuffer)
        {
            root.Search(area,outputBuffer);
        }

        public bool Contains(T data)
        {
            return root.Contains(data, cellMinimumWidth);
        }
        
        public bool Contains(Bound2D area,Predicate<T> matchMethod)
        {
            return root.Contains(area, matchMethod ,cellMinimumWidth);
        }

        public bool Remove(T obj)
        {
            bool suc = root.Remove(obj, cellMinimumWidth);
            if (suc) count--;
            return suc;
        }

        
        
        /// <summary>
        /// 遍历全树，弹出符合条件的对象（注意：此方法不具备四叉树过滤优势，消耗较大，应谨慎使用）
        /// </summary>
        /// <param name="outputBuffer"></param>
        /// <param name="matchMethod"></param>
        /// <returns></returns>
        public bool PopTraverse(List<T> outputBuffer,Predicate<T> matchMethod)
        {
            int a = outputBuffer.Count;
            root.PopMatched(outputBuffer,matchMethod);//弹出符合条件的所有对象
            a = outputBuffer.Count - a;
            count -= a;
            return a > 0;
        }


        /// <summary>
        /// 查找带有额外条件的近邻（不一定是最近）
        /// 使用二分查找方法,近邻查找
        /// 这个方法仅当树中存储数据特别多时效果好，当数据较少时不推荐使用
        /// </summary>
        /// <param name="matchMethod">额外的匹配要求</param>
        /// <param name="allowedDeviation">允许误差区间,这个值越大迭代次数越少、收敛越快</param>
        /// <returns>返回搜索方形区域边长</returns>>
        public float FindNearNeighbor(Vector2 searchCenter,List<T> outputBuffer,Predicate<T> matchMethod,float allowedDeviation=1)
        {
            lastFindNearNearNeighborResult = (searchCenter,
                root.FindNearNeighbor(searchCenter, matchMethod, outputBuffer, cellMinimumWidth, allowedDeviation));
            return lastFindNearNearNeighborResult.radius;
        }
        
        public float FindNearNeighbor(Vector2 searchCenter,List<T> outputBuffer,float allowedDeviation=1)
        {
            lastFindNearNearNeighborResult=(searchCenter,root.FindNearNeighbor(searchCenter, t => { return true;},outputBuffer,cellMinimumWidth,allowedDeviation));
            return lastFindNearNearNeighborResult.radius;
        }

        private (Vector2 center, float radius) lastFindNearNearNeighborResult;//for debug

        
        /*---------------------------------------Debug tools------------------------------------*/
        
        #region Debug_Analyze
        private TreePecker<QuadTree<T>.Node> treePecker;
        public void _LogTree()
        {
            treePecker.PrintTree();
        }
        
        /// <summary>
        /// 四叉树网络绘制工具
        /// </summary>
        /// <param name="node"></param>
        /// <param name="duration"></param>
        private void _DrawPartitionMesh(Node node,float duration=10,float worldY=0)
        {
            if(node==null) return;

            Vector2 center = node.Bound.Center;
            Drawer2D4XZ.DrawRectangle(new Vector3(center.x,worldY,center.y),node.Bound.leftDown,node.Bound.rightUp,duration);
            
            if(node.val!=null)//不是叶子
                Drawer2D4XZ.DrawSquare(node.val.GetPosition2D(),worldY,0.1f,duration);
            
            if(node.children!=null)
                foreach (var n in node.children)
                {
                    _DrawPartitionMesh(n,duration);
                }
        }
        public void _DrawPartitionMesh(float duration=100,float worldY=0)
        {
            Drawer2D4XZ.color=Color.red;
            _DrawPartitionMesh(root,duration,worldY);
        }

        public void _DrawLastNearNeighborSearchBound(float drawDuration = 10, float worldY = 0.1f)
        {
            Drawer2D4XZ.color=Color.blue;
            Drawer2D4XZ.DrawSquare(lastFindNearNearNeighborResult.center,worldY,lastFindNearNearNeighborResult.radius,drawDuration);
        }
        
        

        /// <summary>
        /// 分析用
        /// </summary>
        [Conditional("DEBUG")]
        public void _PerformanceAnalyze()
        {
            //假设初始树区域宽度为L，阈值为1，则树深度为D=log_2(L)+1
            //则节点数最多为(4^D-1)/3
            //其中叶子数最多为4^{D-1}
            float areaWidth = root.Bound.Width;
            float readCellMinimumWidth = cellMinimumWidth + EPS;//读数时补偿阈值
            
            //log_2(k)=log_e(k)/log_e(2)
            float maxdepth = Mathf.Floor(Mathf.Log(areaWidth/readCellMinimumWidth)/Mathf.Log(2)+1);

            float realMinimumCellWidth=areaWidth/Mathf.Pow(2,maxdepth-1);//实际最小格子尺寸

            float adviceAreaWidth=readCellMinimumWidth*Mathf.Pow(2,maxdepth-1);//建议区域宽度
            
            int maxNodes = (int)((Mathf.Pow(4f, maxdepth) - 1f) / 3f);
            
            StringBuilder stringBuilder=new StringBuilder();
            stringBuilder.Append("\n");
            stringBuilder.Append("------四叉树性能分析-----");
            stringBuilder.Append("\n");
            stringBuilder.Append("PART 1:");
            stringBuilder.Append("\n");
            stringBuilder.Append("总区域宽度： "+areaWidth);
            stringBuilder.Append("\n");
            stringBuilder.Append("区域拆分阈值： "+readCellMinimumWidth);
            stringBuilder.Append("\n");
            stringBuilder.Append("实际最小单元尺寸： "+realMinimumCellWidth+ " (>"+readCellMinimumWidth+")");
            stringBuilder.Append("\n");
            stringBuilder.Append("建议设置区域宽度(可保证实际最小单元恰为阈值)： "+adviceAreaWidth);
            stringBuilder.Append("\n");
            stringBuilder.Append("四叉树最大深度： "+(int)maxdepth);
            stringBuilder.Append("\n");
            stringBuilder.Append("完全填充时节点数量： "+maxNodes);
            stringBuilder.Append("\n");
            stringBuilder.Append("PART 2:");
            stringBuilder.Append("当前节点数量 "+Count);
            stringBuilder.Append("\n");
            stringBuilder.Append("----------------------------");
            Logger.PrintHint(stringBuilder);
        }
        
        #endregion 
        
        
        /*---------------------------------------test case------------------------------------*/
        /// <summary>
        /// 功能测试
        /// </summary>
        public static void _UseCase_TestDemo()
        {
            const int drawDuration = 100;
            QuadTree<_QuadTreeTestDataStruct>  quadTree=new QuadTree<_QuadTreeTestDataStruct>(Vector2.one*-5f,Vector2.one*5f,1  );
            //quadTree._PerformanceAnalyze();
            
            
            Logger.PrintWarning("插入测试");
            quadTree.Insert(new _QuadTreeTestDataStruct(Vector2.one*2));
            quadTree.Insert(new _QuadTreeTestDataStruct(Vector2.one*-4));
            var tnode = new _QuadTreeTestDataStruct(new Vector2(-3, -4));
            quadTree.Insert(tnode);
            quadTree.Insert(new _QuadTreeTestDataStruct(new Vector2(-4,-3)));
            quadTree.Insert(new _QuadTreeTestDataStruct(new Vector2(-3,4)));
            Logger.PrintHint("数据数目："+quadTree.Count);

            //树形分析
            Logger.PrintWarning("树形分析");
            var treePecker=new TreePecker<QuadTree<_QuadTreeTestDataStruct>.Node>(quadTree.Root);
            treePecker.PrintTree();
            
            //空间分割绘制
            Logger.PrintWarning("空间绘制");//基于XZ平面
            quadTree._DrawPartitionMesh(drawDuration);
            
            
            
            
            
            
            
            Logger.PrintWarning("搜索测试");
            
            //区域搜搜
            List<_QuadTreeTestDataStruct> outputBuffer=new List<_QuadTreeTestDataStruct>();
            QuadTree<_QuadTreeTestDataStruct>.Bound2D searchArea = 
                QuadTree<_QuadTreeTestDataStruct>.Bound2D.Quadrant3rd(quadTree.Root.Bound);
            quadTree.Search(searchArea,outputBuffer);
            Logger.PrintCollection(outputBuffer);
            
            
            Logger.PrintWarning("包含测试");
            Logger.PrintHint( quadTree.Contains(new _QuadTreeTestDataStruct(new Vector2(-3,-4))));
            Logger.PrintHint(quadTree.Contains(tnode));//必须是同一个对象,这个应该和c#的get hash code实现机制有关

            
            Logger.PrintWarning("区域包含");
            Logger.PrintHint( quadTree.Contains(searchArea,(t)=>Math.Abs(t.val.y - (-4)) < 0.01));
            
            
            
            
            
            //删除测试
            Logger.PrintWarning("删除测试");
            Logger.PrintHint(quadTree.Remove(tnode));
            Logger.PrintHint(quadTree.Remove(new _QuadTreeTestDataStruct(new Vector2(999f,999f))));//尝试删除一个不存在的节点
            //树形打印
            treePecker.PrintTree();
            
            
            
            //再次插入
            Logger.PrintWarning("再次插入");
            quadTree.Insert(tnode);
            treePecker.PrintTree();
            
            
            //TODO:遍历弹出
            
            
            
            Logger.PrintWarning("近邻搜索");
            outputBuffer.Clear();
            Vector2 searchCenter = new Vector2(-3, -3);
            //Vector2 searchCenter = new Vector2(3, -3);
            float c=quadTree.FindNearNeighbor(searchCenter,outputBuffer );
            Logger.PrintCollection(outputBuffer);
            Logger.PrintHint(c);
            quadTree._DrawLastNearNeighborSearchBound();
            
        }

        /// <summary>
        ///压力测试
        /// </summary>
        public static void _Pressure_TestDemo()
        {
            const float drawDuration = 100;
            
            QuadTree<_QuadTreeTestDataStruct>  quadTree=new QuadTree<_QuadTreeTestDataStruct>(Vector2.one*-5f,Vector2.one*5f,1  );
            quadTree._PerformanceAnalyze();

            Vector2 range=new Vector2(-5,5);
            foreach (var v in FakeDataGenerator.Vector2List(85,range,range ))
            {
                quadTree.Insert(new _QuadTreeTestDataStruct(v));
            }
            
            
            //树形分析
            Logger.PrintWarning("树形分析");
            TreePecker<QuadTree<_QuadTreeTestDataStruct>.Node> treePecker=new TreePecker<QuadTree<_QuadTreeTestDataStruct>.Node>(quadTree.Root);
            treePecker.PrintTree();
            
            
            quadTree._DrawPartitionMesh(drawDuration);
            
            
            
            Logger.PrintWarning("近邻搜索");
            List<_QuadTreeTestDataStruct> outputBuffer=new List<_QuadTreeTestDataStruct>(30);
            Vector2 searchCenter = new Vector2(-3, -3);
            //Vector2 searchCenter = new Vector2(3, -3);
            float c=quadTree.FindNearNeighbor(searchCenter,outputBuffer );
            Logger.PrintCollection(outputBuffer);
            
            
        }
    }


    /// <summary>
    /// 测试用类
    /// </summary>
    public class _QuadTreeTestDataStruct : IQuadTreePartitionInfoCheck
    {
        public Vector2 val;
        public Vector2 GetPosition2D()//接口方法,这里为了方便测试直接简单的把数据直接映射成位置了
        {
            return val;
        }

        public _QuadTreeTestDataStruct(Vector2 val)
        {
            this.val = val;
        }

        public override string ToString()
        {
            return val.ToString();
        }
    }

}