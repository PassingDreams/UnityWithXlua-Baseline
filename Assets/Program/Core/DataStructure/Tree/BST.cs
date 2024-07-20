using System;
using System.Text;
using UnityEngine;

public class BST<K,T>  where K: IComparable
{
    public class Node
    {
        public K key;//for comparing
        public T record;//data
        public Node lchild=null, rchild=null;

        public Node(K key)
        {
            this.key = key;
        }

        public Node()
        {
            
        }
    }

    private Node root=null;
    public void Insert(Node node)
    {
        if(root==null)
        {
            root=node;
        }
        else
        {
            var r = root;
            while (true)
            {
                int cmp = node.key.CompareTo(r.key);//a compare to b just like return sign(a-b)
                if (cmp > 0)
                {
                    if (r.rchild != null)
                    {
                        r = r.rchild;
                    }
                    else
                    {
                        r.rchild = node;
                        return;
                    }
                }
                else if (cmp == 0)
                {
                    Debug.Log("Error BST Node existd when <insert>");
                    return;
                }
                else if (cmp < 0)
                {
                    if (r.lchild != null)
                    {
                        r = r.lchild;
                    }
                    else
                    {
                        r.lchild = node;
                        return;
                    }
                    
                }
                
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns>找不到时返回null</returns>
    public Node Find(K key)
    {
        var r = root;
        while (r!=null)
        {
            int cmp = key.CompareTo(r.key);//a compare to b just like return sign(a-b)
            if (cmp > 0)
            {
                r = r.rchild;
            }
            else if (cmp == 0)
            {
                return r;
            }
            else if (cmp < 0)
            {
                r = r.lchild;
            }
        }
        return null;
    }

    public bool Contains(K key)
    {
        return Find(key) != null;
    }

    public void Remove()
    {
        throw new Exception("TODO");
    }
    
    
    //--------------------------------------DebugTool------------------------------------------//
    private void _MidorderPrint_Recursive_Core(Node root)
    {
        if(root==null)
            return;
        _MidorderPrint_Recursive_Core(root.lchild);
        Debug.Log(root.key);
        _MidorderPrint_Recursive_Core(root.rchild);
    }

    public void DebugMidorderPrint()
    {
        _MidorderPrint_Recursive_Core(root);
    }

    private void _DrawTree_Recursive_Core(Node root,int layer,Stack<char> heredityChain,StringBuilder stringBuilder)
    {
        if(root.rchild!=null)
        {
            heredityChain.Push('r');
             _DrawTree_Recursive_Core(root.rchild,layer+1,heredityChain,stringBuilder);
            heredityChain.Pop();
        }
        heredityChain.Push('m');//me
        for(int i=0;i<layer;i++)
        {
            if (i == layer - 1)
                stringBuilder.Append((heredityChain.Peep(i) == 'r' ? '/' : '\\') + "-----");
            else
            {
                if (heredityChain.Peep(i) != heredityChain.Peep(i + 1))
                    stringBuilder.Append("|#####");//unity editor对于.或者空格这类字符的宽度处理后变窄了，所以我只能使用#作为空占位符了
                else
                    stringBuilder.Append("######");
            }
        }

        stringBuilder.Append(root.key+"\n");
        
        heredityChain.Pop();
        if(root.lchild!=null)
        {
            heredityChain.Push('l');
             _DrawTree_Recursive_Core(root.lchild,layer+1,heredityChain,stringBuilder);
            heredityChain.Pop();
        }
    }

    public void DebugDrawTree()
    {
        StringBuilder stringBuilder=new StringBuilder();
        stringBuilder.Clear();
        Stack<char> heredityChain=new Stack<char>(10);
        _DrawTree_Recursive_Core(root,0,heredityChain,stringBuilder);
        Debug.Log(stringBuilder.ToString());
    }

    public static void _TestDemo()
    {
       var bst=new BST<int, object>();
       //test insert
       for(int i=0;i<20;i++) 
           bst.Insert(new BST<int, object>.Node(UnityEngine.Random.Range(1,100)));
       
       //test debug
       bst.DebugMidorderPrint();
       bst.DebugDrawTree();
        
    }
    

}



