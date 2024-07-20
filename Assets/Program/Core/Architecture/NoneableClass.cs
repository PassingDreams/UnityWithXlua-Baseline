using System;
using Ueels.Core.Debug;

namespace Ueels
{
    
    //使用要求：
    //枚举类型E中必须有Default=0, None的value类型则随意指定


    /// <summary>
    /// 可空类，构造一个具有唯一实例的None对象来代替null，在进行类型判断时可以更方便
    /// </summary>
    /// <typeparam name="T">继承者实例类</typeparam>
    /// <typeparam name="E">该类的类型枚举</typeparam>
    public abstract class NoneableClass<T,E> where E: Enum where T : NoneableClass<T,E>, new()
    {
        public E Type
        {
            get { return _type; }
        }

        private E _type;//禁止外部直接设置类型
        
        
        //空对象唯一实例
        public static T None 
        {
            get
            {
                if(_none==null)
                    BuildSingleNone();

                return _none;
            }
        }
        private static T _none = default;

        
        /// <summary>
        /// 构建唯一空实例,这个函数最多只会被调用一次
        /// </summary>
        /// <returns>返回空实例</returns>
        private static void BuildSingleNone()
        {
            _none=new T();
            _none._type = _none.NoneType();
        }
        
        /// <summary>
        /// 用来告知基类空类None class对应的枚举类型
        /// </summary>
        /// <returns></returns>
        protected abstract E NoneType();
        
        
        //可以开放设置的类型,此处负责封闭None的构建
        public void SetType(E type)
        {
            if (type.Equals(NoneType()))
            {
                ThrowHelper.Throw("禁止设置空类型，空类型应在全局具有唯一的实例");
            }
            _type = type;
        }
    }


    /*-------------------演示用例------------------
    public class NoneableDemo: NoneableClass<NoneableDemo,NoneableDemo.ENoneableType>
    {
        public enum ENoneableType
        {
            Default=0,//type的默认值总是会被初始化为0，所以确保默认类型=0而不是其他类型=0,这样可以保证内部初始化结果类型为Default
            None,//空类型,对应null
            Other,
            //...
        }

        public NoneableDemo(ENoneableType type)
        {
            SetType(type);
        }

        public NoneableDemo()
        {
            
        }

        protected override ENoneableType NoneType()
        {
            return ENoneableType.None;
        }



        public static void test()
        {

            NoneableDemo  t=new NoneableDemo();
            Logger.PrintError(t.Type);
            t=new NoneableDemo(ENoneableType.Other);
            Logger.PrintError(t.Type);
            
            
            Logger.PrintHint(None.Type);//可以访问None
            
            t=new NoneableDemo(ENoneableType.None);//但不能创建更多None实例
            Logger.PrintError(t.Type);
            
        }


    }
    -------------------------------------------*/

}
