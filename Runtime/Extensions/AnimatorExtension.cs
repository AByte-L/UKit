using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace AByte.UKit.Extensions
{
    /// <summary>
    /// Animator 扩展
    /// </summary>
    public static class AnimatorExtension
    {
        /// <summary>
        /// 播放动画并返回结束
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="trigger"></param>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        public static IObservable<AsyncUnit> SetTriggerWithCompleted (this Animator animator,string trigger, int layerIndex = 0)
        {
            animator.SetTrigger(trigger);
            var t = UniTask.WaitUntil(() =>
              animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime >= 1.0f //注意，这里每次都要重新获取
            );
            return t.ToObservable();    
        }

        public static async UniTask SetTriggerWithCompletedTask(this Animator animator, string trigger, int layerIndex = 0)
        {
            animator.SetTrigger(trigger);
            await UniTask.WaitUntil(() =>
              animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime >= 1.0f //注意，这里每次都要重新获取
            );
            
        }
    }
}
