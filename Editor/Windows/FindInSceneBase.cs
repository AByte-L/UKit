
using System.Reflection;
using UnityEngine;

namespace AByte.UKit.Editor
{
    public abstract class FindInSceneBase<T> where T : MonoBehaviour
    {

        public virtual bool Comp(T t, object value)
        {
            return false;
        }

        // public void Find()
        // {
        //     var arr = UnityEngine.Object.FindObjectsOfType<T>();
        //     var prop = typeof(T).GetProperty(fielName);
        //     foreach (var item in arr)
        //     {
        //         var v = prop.GetValue(item, null) as string;
        //         if (v == id)
        //         {
        //             Selection.activeObject = item;
        //             this.ShowNotification(new GUIContent { text = $"找到ID为“{id}”的对象：“{item.name}”" });
        //             return;
        //         }
        //     }
        //     this.ShowNotification(new GUIContent { text = "什么都没有！" });
        // }
    }

}