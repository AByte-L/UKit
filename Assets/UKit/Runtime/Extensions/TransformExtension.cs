using System;
using UnityEngine;

namespace AByte.UKit
{

    public static class TransformExtension
    {

        public interface IItem<T> where T : class
        {
            void Init(T t);
        }


        /// <summary>
        /// 清除子对象，除了名称为：excludeChildName
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="excludeChildNames"></param>
        public static void ClearChilds<T>(this Transform parent, params string[] excludeChildNames) where T : MonoBehaviour
        {
            if (parent == null) return;
            for (int i = parent.childCount - 1; i > -1; i--)
            {
                var deItem = parent.GetChild(i);
                if (deItem.GetComponent<T>() == null) continue;

                bool skip = false;
                if (excludeChildNames != null)
                {
                    foreach (var item in excludeChildNames)
                    {
                        if(item == deItem.name)
                        {
                            skip = true;
                            break;
                        }

                    }
                }
                if (skip == false)
                {
                    //删除
                    GameObject.Destroy(deItem.gameObject);
                }
            }
        }


        /// <summary>
        /// 清除子对象，除了 excludeChild
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="excludeChilds"></param>
        public static void ClearChilds(this Transform parent, params Transform[] excludeChilds)
        {
            if (parent == null) return;
            for (int i = parent.childCount - 1; i > -1; i--)
            {
                var deItem = parent.GetChild(i);
                bool skip = false;
                if (excludeChilds != null)
                {
                    foreach (var item in excludeChilds)
                    {
                        if (item == deItem)
                        {
                            skip = true;
                            break;
                        }
                    }
                }
                if (skip == false)
                {
                    //删除
                    GameObject.Destroy(deItem.gameObject);
                }
            }
        }

        public static void AddChild<T, D>(this Transform parent, T defualtitem, D data) where D : class where T : MonoBehaviour, IItem<D>
        {
            if (parent == null) return;
            var newItem = GameObject.Instantiate(defualtitem, parent);
            newItem.gameObject.SetActive(true);
            newItem.Init(data);
        }

        public static void RemoveChild<T>(this Transform parent, Func<T, bool> condition) where T : MonoBehaviour
        {
            if (parent == null) return;
            for (int i = parent.childCount - 1; i > -1; i--)
            {
                var item = parent.GetChild(i);

                var t = item.GetComponent<T>();
                if (t == null) continue;
                if (condition(t))
                {
                    GameObject.Destroy(t.gameObject);
                    return;
                }

            }
        }

    }
}