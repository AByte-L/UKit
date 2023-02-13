#if UNITY_EDITOR
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using AByte.UKit.Editor.Utilities;

namespace AByte.UKit.Editor
{

    /*
     * 查找场景中的组件
     * 根据组件配置的值
     * 
     */
    public class FindComponentInSceneWin : BaseWin
    {

        const string CONF_FILE_NAME = "组件收集器";
        [MenuItem("UKit/查找场景中组件")]
        private static void Open()
        {
            var EditorWin = GetWindow<FindComponentInSceneWin>(title: "查找场景中组件");
            EditorWin.position = GUIHelper.GetEditorWindowRect().AlignCenter(550, 320);
            EditorWin.Init();
        }

        //[InlineButton("CreateCollectConf", "创建") ] 自动创建
        [InfoBox("请自行向“配置文件”中添加需要查找的组件")]
        [Space, LabelText("配置文件"), ReadOnly] public FindComponentColl findColl;


        /************************************************************/
        [Space, LabelText("包含未显示对象")]
        public bool m_includeInactive;
        [Space, LabelText("选择查找组件"), OnValueChanged("OnValueChanged"), ValueDropdown("GetFilteredTypeList")]
        public string m_selectedComponentInfo; //选择的组件信息（类名和成名）
        private string m_className;//
        [Title("$m_className")]
        [Space, CustomValueDrawer("strValueDraw")] public string m_strValue;
        [PropertySpace(SpaceBefore = 0, SpaceAfter = 12), CustomValueDrawer("intValueDraw")] public int m_intVlaue = 0;


        [PropertyOrder(10), HideLabel, PropertySpace(SpaceBefore = 10, SpaceAfter = 10), Multiline, GUIColor("$GetMsgColor"), ShowIf("@this.m_selectedComponentInfo!=null")]
        public string m_msg;

        List<UnityEngine.Object> findList = new List<UnityEngine.Object>();//找到组件的所有列表
        int showIndex = -1; //显示索引

        protected override string Msg
        {
            get { return m_msg; }
            set { m_msg = value; }

        }


        //[EnableIf("@this.findColl!=null")]
        public void Init()
        {
            findColl = (FindComponentColl)EditorGUIUtility.Load($"UKit/{CONF_FILE_NAME}.asset");
            if (findColl == null)
            {
                var conf = ScriptableObject.CreateInstance<FindComponentColl>();
                AssetDatabaseHelper.CreateAsset(conf, @$"Assets/Editor Default Resources/UKit", $"{CONF_FILE_NAME}.asset");//在传入的路径中创建资源
                findColl = conf;
            }

        }


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
            if (findColl == null) return null;
            if (findColl.items == null) return null;
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


        [ShowInInspector, ButtonGroup, Button(ButtonSizes.Medium), ShowIf("@this.m_selectedComponentInfo != null")]
        private void Find()
        {
            showIndex = -1;
            findList.Clear();
            var item = GetSelectedItem();
            if (item == null) return;
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

        [ShowInInspector, ButtonGroup, EnableIf("@this.findList.Count > 1"), LabelText("下一个"), ShowIf("@this.m_selectedComponentInfo != null")]
        public void Next()
        {
            if (findList.Count < 2) return; //小于2就没得必要
            showIndex = (++showIndex) % findList.Count;
            Selection.activeObject = findList[showIndex];
            ShowMsgInfo(MsgType.Info, $"UKit:选中({showIndex + 1}/{findList.Count})");
        }

    }
}

#endif