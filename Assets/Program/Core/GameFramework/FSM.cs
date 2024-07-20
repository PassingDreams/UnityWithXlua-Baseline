using System;
using System.Collections.Generic;
using UnityEngine;
using Logger = Ueels.Core.Debug.Logger;



namespace Ueels.Core.GameFramework
{

    public static class StateInfoExtentions
    {
        public static FSM.StateInfo OnUpdate(this FSM.StateInfo state,Action onUpdate)
        {
            state.onUpdate = onUpdate;
            return state;
        }
        public static FSM.StateInfo OnEnter(this FSM.StateInfo state,Action onEnter)
        {
            state.onEnter = onEnter;
            return state;
        }
        public static FSM.StateInfo OnExit(this FSM.StateInfo state,Action onExit)
        {
            state.onExit = onExit;
            return state;
        }
    }
    public class FSM: IFSM
    {
        public class StateInfo
        {
            public Action onEnter;
            public Action onUpdate;
            public Action onExit;
            public List<(int toState, int eventCode)> transitions;

            public StateInfo()
            {

            }
        }
        public int CurrentStateCode  { get; private set; }//使用int是为了与具体枚举类型解耦
        public int LastStateCode  { get; private set; }

        
        private Dictionary<int,StateInfo> statesHashTable=new Dictionary<int, StateInfo>();
        
        
        private Action beforeUpdateCallBack;//回调函数
        private Action afterUpdateCallBack;
        public FSM(int entranceState=-1,Action beforeUpdateCallBack=null,Action afterUpdateCallBack=null)
        {
            CurrentStateCode = entranceState;
            this.beforeUpdateCallBack = beforeUpdateCallBack;
            this.afterUpdateCallBack = afterUpdateCallBack;
        }

        public static StateInfo NewAnonymousState()
        {
            return new StateInfo();
        }

        public int GetCurrentState()
        {
            return CurrentStateCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="forceSwitch">是否允许自状态切换</param>
        /// <returns></returns>
        public  bool SwitchState(int state,bool forceSwitch=false)
        {
            if (!statesHashTable.ContainsKey(state)) return false;

            if (state == CurrentStateCode&&!forceSwitch) return false;

            LastStateCode = CurrentStateCode;
            CurrentStateCode = state;
            
            if(statesHashTable.ContainsKey(LastStateCode))
                statesHashTable[LastStateCode].onExit?.Invoke();
            statesHashTable[CurrentStateCode].onEnter?.Invoke();
            return true;
        }

        /// <summary>
        /// 状态机启动，进入第一个状态
        /// </summary>
        public bool Enter(int state)
        {
            if (!statesHashTable.ContainsKey(state)) return false;
            LastStateCode = CurrentStateCode;
            CurrentStateCode = state;
            statesHashTable[CurrentStateCode].onEnter?.Invoke();
            return true;
        }

        /// <summary>
        /// 当当前状态存在符合eventCode的transition时，执行transition到新状态
        /// </summary>
        /// <param name="eventCode"></param>
        /// <returns></returns>
        public bool Triger(int eventCode,bool isSwitchToSelfable=true)
        {
            var transitions = statesHashTable[CurrentStateCode].transitions;
            if (transitions == null)
            {
                Logger.PrintError("找不到跳转,eventCode=" + eventCode);
                return false;
            }

            foreach (var transition in transitions)
            {
                if (transition.eventCode == eventCode)
                {
                    return SwitchState(transition.toState,isSwitchToSelfable);
                }
            }
            return false;
        }


        public bool AddState(int state, Action onEnter=null, Action onExit=null, Action onUpdate=null)
        {
            StateInfo s=new StateInfo();
            s.onEnter = onEnter;
            s.onExit = onExit;
            s.onUpdate = onUpdate;
            return AddState(state, s);
        }
        public bool AddState(int state,StateInfo stateInfo)
        {
            if (statesHashTable.ContainsKey(state))
            {
                Logger.PrintError("状态重复添加");
                return false;
            }
            statesHashTable.Add(state,stateInfo);
            //Debug.LogError("状态添加"+state);
            return true;
        }

        public bool AddTransition(int from, int to, int eventCode)
        {
                if(statesHashTable[from].transitions==null)
                    statesHashTable[from].transitions=new List<(int toState,int eventCode)>();
            statesHashTable[from].transitions.Add( (to, eventCode) );
            return true;
        }
        public void ClearGraph()
        {
            ClearAllTransitions();
            ClearAllStates();
        }
        public void ClearAllStates()
        {
            statesHashTable.Clear();
        }

        public void ClearAllTransitions()
        {
            foreach(var state in statesHashTable.Values)
            {
                var transitions=state.transitions;
                if (transitions == null)
                    continue;
                transitions.Clear();
            }
        }

        public void Update()
        {
            if (statesHashTable.TryGetValue(CurrentStateCode, out var stateInfo))
            {
               beforeUpdateCallBack?.Invoke(); 
               stateInfo.onUpdate?.Invoke();
               afterUpdateCallBack?.Invoke();
            }
            
        }
        
    }
        
        
}
