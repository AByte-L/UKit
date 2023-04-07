using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AByte.UKit
{
    public static class VectorHelper
    {

        /// <summary>
        /// 设置自身看向目标，且自身永远在水平面
        /// 适用场景：
        /// 1 怪物看向玩家
        /// 2 world 中的 ui 看向玩家
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        public static void LookRotation(Transform self, Transform target)
        {
            Vector3 lookDir = self.position - target.position;
            lookDir.y = 0;
            self.rotation = Quaternion.Slerp(self.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime);
        }
    }
}
