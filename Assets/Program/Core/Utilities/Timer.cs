using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ueels.Core
{ 
    public sealed class Timer 
    {
        private float timer;
        private float timingLength;//计时长度
        private Action timingFinishCallBack;
        
        public static IEnumerator DelayDo(float delaytime,Action todo)
        {
            yield return new WaitForSeconds(delaytime);
            todo();
        }
        
        public Timer(float timingLength,Action timingFinishCallBack)
        {
            ResetTiming();
            this.timingLength = timingLength;
            this.timingFinishCallBack = timingFinishCallBack;
        }
        public Timer(float timingLength)
        {
            ResetTiming();
            this.timingLength = timingLength;
        }
        public void BindCallBack(Action timingFinishCallBack)
        {
            this.timingFinishCallBack = timingFinishCallBack;
        }
        /// <summary>
        /// 重新计时
        /// </summary>
        public void ResetTiming()
        {
            timer = 0;
        }

        /// <summary>
        /// 计时器更新
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            if (timer > timingLength)
                return;
            timer += deltaTime;

            if (timer > timingLength)
                timingFinishCallBack.Invoke();//不绑定回调函数会报错
        }


    }
}
