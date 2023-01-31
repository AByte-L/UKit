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
    public class FindComponentInSceneWin : BaseWin
    {
        [MenuItem("UKit/查找场景中组件")]
        private static void Open()
        {
            var EditorWin = GetWindow<FindComponentInSceneWin>(title: "查找场景中组件");
            EditorWin.position = GUIHelper.GetEditorWindowRect().AlignCenter(550, 320);
            EditorWin.findColl = Resources.Load<FindComponentColl>("FindComponentColl");
        }
        [Space, LabelText("配置文件"), InfoBox("“配置文件”需要自行创建并命名为“FindComponentColl”,且放置在Rerources下")] public FindComponentColl findColl;


        /************************************************************/
        [Space, LabelText("包含未显示对象")]
        public bool m_includeInactive;
        [Space, LabelText("选择查找组件"), OnValueChanged("OnValueChanged"), ValueDropdown("GetFilteredTypeList")]
        public string m_selectedComponentInfo; //选择的组件信息（类名和成名）
        private string m_className;//
        [Title("$m_className")]
        [Space, CustomValueDrawer("strValueDraw")] public string m_strValue;
        [PropertySpace(SpaceBefore = 0, SpaceAfter = 12), CustomValueDrawer("intValueDraw")] public int m_intVlaue = 0;


        [PropertyOrder(10), HideLabel, PropertySpace(SpaceBefore = 10, SpaceAfter = 10), Multiline, GUIColor("$GetMsgColor"),ShowIf("@this.m_selectedComponentInfo!=null")] 
        public string m_msg;

        List<UnityEngine.Object> findList = new List<UnityEngine.Object>();//找到组件的所有列表
        int showIndex = -1; //显示索引
        MsgType msgType;


        private void OnValueChanged()
        {
            if (string.IsNullOrEmpty(m_selectedComponentInfo)) m_className = "";
            if (findColl == null) m_className = "";
            if (findColl.items == null) m_className = "";
            var item = GetSelectedItem();
            if (item == null) m_className = "";
            m_className = item.className;
        }

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
            if (string.IsNullOrEmpty(m_selectedComponentInfo)) return null;
            if (findColl == null) return null;
            if (findColl.items == null) return null;
            var item = GetSelectedItem();
            if (item == null) return null;
            if (item.valueType == "String")
            {
                return EditorGUILayout.TextField(label: item.ValueName, text: m_strValue);
            }
            return null;
        }


        public int intValueDraw(int value, GUIContent labe)
        {
            //绘制属性
            if (string.IsNullOrEmpty(m_selectedComponentInfo)) return 0;
            if (findColl == null) return 0;
            if (findColl.items == null) return 0;
            var item = GetSelectedItem();
            if (item == null) return 0;
            if (item.valueType == "Int32")
            {
                return EditorGUILayout.IntField(label: item.ValueName, value: m_intVlaue);
            }
            return 0;

        }

        private FindItem GetSelectedItem()
        {
            if (m_selectedComponentInfo == null) return null;
            for (int i = 0; i < findColl.items.Count(); i++)
            {
                if (findColl.items[i] != null && findColl.items[i].CombineID == m_selectedComponentInfo)
                    return findColl.items[i];

            }
            return null;
        }


        [ShowInInspector, ButtonGroup, Button(ButtonSizes.Medium),ShowIf("@this.m_selectedComponentInfo != null")]
        private void Find()
        {
            showIndex = -1;
            findList.Clear();
            var item = GetSelectedItem();
            if(item == null)return;
            var assembly = Assembly.Load(item.AssemblyName);
            var type = assembly.GetType(item.className);
            var arr = UnityEngine.GameObject.FindObjectsOfType(type, includeInactive: m_includeInactive);
            if (arr == null || arr.Length == 0)
            {
                ShowMsgInfo(MsgType.Warning, $"UKit: 场景中未挂载该组件 ！");
                return;
            }
            m_className = item.className;

            foreach (var com in arr)
            {

                object v = null;
                bool isFind = false;
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
                    isFind = (int)v == m_intVlaue;
                }
                else if (v is String)
                {
                    isFind = (string)v == m_strValue;
                }

                if (isFind)
                {
                    findList.Add(com);
                }
            }
            if (findList.Count > 0)
            {
                showIndex = 0;
                Selection.activeObject = findList[showIndex];
                ShowMsgInfo(MsgType.Info, $"UKit:选中({showIndex + 1}/{findList.Count})");
            }
            else
            {
                ShowMsgInfo(MsgType.Warning, $"UKit: 未找到满足条件的该组件 ！");
            }
        }

        [ShowInInspector, ButtonGroup, EnableIf("@this.findList.Count > 1"), LabelText("下一个"),ShowIf("@this.m_selectedComponentInfo != null")]
        public void Next()
        {
            if (findList.Count < 2) return; //小于2就没得必要
            showIndex = (++showIndex) % findList.Count;
            Selection.activeObject = findList[showIndex];
            ShowMsgInfo(MsgType.Info, $"UKit:选中({showIndex + 1}/{findList.Count})");
        }

        private void ShowMsgInfo(MsgType msgType, string msg)
        {
            this.msgType = msgType;
            m_msg = msg;
        }

        private Color GetMsgColor()
        {
            Sirenix.Utilities.Editor.GUIHelper.RequestRepaint();
            switch (msgType)
            {
                case MsgType.Info:
                    // return Color.HSVToRGB(1, 1, 1);
                    return Color.white;
                case MsgType.Warning:
                    return Color.yellow;
                //return Color.HSVToRGB(1, 1, 0);
                case MsgType.Error:
                    return Color.red;
                    //return Color.HSVToRGB(1, 0, 0);

            }
            return Color.HSVToRGB(1, 1, 1);
            //return Color.HSVToRGB(Mathf.Cos((float)UnityEditor.EditorApplication.timeSinceStartup + 1f) * 0.225f + 0.325f, 1, 1);
        }
    }
}

#endif