using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AByte.UKit.Extensions
{
    public static class VectorExtension
    {
        public static float DistanceToTargetOnHorPlane(this Vector3 self, Vector3 target)
        {
            // Vector3 self2TargetVec = target - self;
            // Vector3 self2TargetVecOnplane = new Vector3(target.x, self.y, target.z) - self;
            return Vector3.Distance(self, new Vector3(target.x, self.y, target.z));
        }

        /// <summary>
        /// 玩家到目标的水平距离
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float DistanceToTargetOnHorPlane(this Transform self, Transform target)
        {
            return Vector3.Distance(self.position, new Vector3(target.position.x, self.position.y, target.position.z));
        }

        /// <summary>
        /// 返回玩家到目标的夹角(度，不是弧度)  
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float Angle(this Transform self, Transform target)
        {
            var cosValue = Vector3.Dot(self.forward.normalized, (target.position - self.position).normalized);
            float rad = Mathf.Acos(cosValue);//反余玄求夹角的弧[-1,1]
            var deg = rad * Mathf.Rad2Deg;//弧度转度
            return deg;
        }

    }
}
