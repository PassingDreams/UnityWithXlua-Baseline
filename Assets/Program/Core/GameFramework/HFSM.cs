using System;
using System.Collections.Generic;
using Ueels.Core.Debug;

namespace Ueels.Core.GameFramework
{
    
    public interface IFSM
    {
        int GetCurrentState();
        bool SwitchState(int stateCode,bool forceSwitch=false);
        bool Triger(int eventCode,bool isSwitchToSelfable=true);
        void Update();
    }
    /// <summary>
    /// Hierarchical FSM
    /// </summary>
    public class HFSM<T>: IFSM where T: Enum  
    {
        public T CurrentState  { get; private set; }
        public T LastState  { get; private set; }
        private IFSM CurrentStateMachine => subFsmHashTable[CurrentState];
        private Dictionary<T,IFSM> subFsmHashTable=new Dictionary<T, IFSM>();


        /// <summary>
        /// 递归得到叶子结点状态
        /// </summary>
        /// <returns></returns>
        public int GetCurrentState()
        {
            return CurrentStateMachine.GetCurrentState();
        }
        
        /// <summary>
        /// 得到本层状态
        /// </summary>
        /// <returns></returns>
        
        public T GetCurrentSuperState()
        {
            return CurrentState;
        }

        public bool SwitchState(int stateCode, bool forceSwitch = false)
        {
            var cur = CurrentStateMachine;
            return cur.SwitchState(stateCode, forceSwitch);
        }



        /// <summary>
        /// switch super state
        /// </summary>
        /// <param name="state"></param>
        /// <param name="forceSwitch"></param>
        /// <returns></returns>
        public bool Play(T state)
        {

            var statesHashTable = subFsmHashTable;
            if (!statesHashTable.ContainsKey(state)) return false;//不含键

            if (state.Equals(CurrentState)) return false;//无效转换

            LastState = CurrentState;
            CurrentState = state;

            return true;
        }

        public bool Triger(int eventCode,bool isSwitchToSelfable=true)
        {
            return CurrentStateMachine.Triger(eventCode);
        }

        public void Update()
        {
            CurrentStateMachine.Update();
        }

        public bool AddState(T state,IFSM subStateMachine)
        {
            
            if (subFsmHashTable.ContainsKey(state))
            {
                Logger.PrintError("状态重复添加");
                return false;
            }
            subFsmHashTable.Add(state,subStateMachine);
            return true;
        }
    }


}
