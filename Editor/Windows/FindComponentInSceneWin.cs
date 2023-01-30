#if UNITY_EDITOR
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace AByte.UKit.Editor
{

    /*
     * 查找场景中的组件
     * 根据组件配置的值
     * 
     */
    public class FindComponentInSceneWin : OdinEditorWindow
    {
        [MenuItem("UKit/查找场景中组件")]
        private static void Open()
        {
            var EditorWin = GetWindow<FindComponentInSceneWin>(title: "查找场景中组件");
            EditorWin.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 200);
            EditorWin.findColl = Resources.Load<FindComponentColl>("FindComponentColl");
        }
        [Space, LabelText("配置文件"),InfoBox("需要自行创建并命名为“FindComponentColl”,且放置在Rerources下")] public FindComponentColl findColl;


        /************************************************************/

        [Space, LabelText("选择查找组件"), ValueDropdown("GetFilteredTypeList")]
        public string SelectedClass; //选择的类
        [Space, CustomValueDrawer("strValueDraw")] public string strValue;
        [CustomValueDrawer("intValueDraw")] public int intVlaue = 0;

        /// <summary>
        /// typeName类型过滤
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetFilteredTypeList()
        {
            return findColl.items.Select(x => x.CombineID);
        }

        public string strValueDraw(string value, GUIContent labe)
        {
            //绘制属性
            if (string.IsNullOrEmpty(SelectedClass)) return null;
            if (findColl == null) return null;
            if (findColl.items == null) return null;
            var item = GetSelectedItem();
            if (item == null) return null;
            if (item.valueType == "String")
            {
                return EditorGUILayout.TextField(label: item.ValueName, text: strValue);
            }
            return null;
        }


        public int intValueDraw(int value, GUIContent labe)
        {
            //绘制属性
            if (string.IsNullOrEmpty(SelectedClass)) return 0;
            if (findColl == null) return 0;
            if (findColl.items == null) return 0;
            var item = GetSelectedItem();
            if (item == null) return 0;
            if (item.valueType == "Int32")
            {
                return EditorGUILayout.IntField(label: item.ValueName, value: intVlaue);
            }
            return 0;

        }

        private FindItem GetSelectedItem()
        {
            if (SelectedClass == null) return null;
            for (int i = 0; i < findColl.items.Count(); i++)
            {
                if (findColl.items[i] != null && findColl.items[i].CombineID == SelectedClass)
                    return findColl.items[i];

            }
            return null;
        }

        [PropertySpace, ShowInInspector, Button(ButtonSizes.Medium)]
        private void Find()
        {
            var item = GetSelectedItem();
            var assembly = Assembly.Load(item.AssemblyName);
            var type = assembly.GetType(item.className);
            var arr = UnityEngine.Object.FindObjectsOfType(type);
            if (arr == null)
            {
                Debug.LogWarning("arr is null!");
                return;
            }

            foreach (var com in arr)
            {

                object v = null;
                bool finded = false;
                switch (item.memberType)
                {
                    case MemberType.Field:
                        var fieldInfo = type.GetField(item.ValueName);
                        v = fieldInfo.GetValue(com);

                        break;
                    case MemberType.Property:
                        var propInfo = type.GetProperty(item.ValueName);
                        v = propInfo.GetValue(com);
                        break;

                }
                if (v is Int32)
                {
                    finded = (int)v == intVlaue;
                }
                else if (v is String)
                {
                    finded = (string)v == strValue;
                }

                if (finded)
                {
                    Selection.activeObject = com;
                    this.ShowNotification(new GUIContent { text = $"找到 {item.ValueName} 为“{v}”的对象：“{com.name}”" });
                    return;
                }
            }
            this.ShowNotification(new GUIContent { text = "什么都没有！" });
        }

    }
}

#endif