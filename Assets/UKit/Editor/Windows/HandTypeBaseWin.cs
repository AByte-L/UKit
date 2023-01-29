#if UNITY_EDITOR

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace AByte.UKit.Editor
{

    /*
     * 修改场景中选中对象的对象名称
     * 需求，打开面板的时，选中车型资源文件，绘制数据
     * 
     * 
     */
    public class HandTypeBaseWin : OdinEditorWindow
    {

        [LabelText("选择处理位置"), LabelWidth(120)]
        public HandleType handleType;

        /**** 前缀 *****/

        [Space]
        [LabelText("原前缀："), LabelWidth(60)]
        [Multiline(1)]
        [SuffixLabel("如果为空，表示直接添加前缀", true)]
        [ShowIf("handleType", HandleType.Prefix)]
        public string oldPrefix;
        [Space]
        [LabelText("新前缀："), LabelWidth(60)]


        /**** 后缀 *****/

        [Multiline(1)]
        [SuffixLabel("", true)]
        [ShowIf("handleType", HandleType.Prefix)]
        public string newPrefix;
        [Space]
        [LabelText("原后缀："), LabelWidth(60)]
        [Multiline(1)]
        [SuffixLabel("如果为空，表示直接添加后缀", true)]
        [ShowIf("handleType", HandleType.Suffix)]
        public string oldSuffix;

        [Space]
        [LabelText("新后缀："), LabelWidth(60)]
        [Multiline(1)]
        [SuffixLabel("", true)]
        [ShowIf("handleType", HandleType.Suffix)]
        public string newSuffix;


        /**** 替换 *****/

        [Space]
        [LabelText("原字符："), LabelWidth(60)]
        [Multiline(1)]
        [ShowIf("handleType", HandleType.Replace)]
        [SuffixLabel("不能为空", true)]
        public string replace_oldString;

        [Space]
        [LabelText("新字符："), LabelWidth(60)]
        [Multiline(1)]
        [SuffixLabel("可以为空", true)]
        [ShowIf("handleType", HandleType.Replace)]
        public string replace_newString;


        /**** 方法 *****/

        [LabelText("更新前缀"), LabelWidth(60)]
        [ShowInInspector]
        [ShowIf("handleType", HandleType.Prefix)]
        private void ModifyPrefixHandler()
        {
            if (HasSelectedObjects() == false) return;
            ModifyPrefixCmd(oldPrefix, newPrefix);
        }

        [LabelText("更新后缀"), LabelWidth(60)]
        [ShowInInspector]
        [ShowIf("handleType", HandleType.Suffix)]
        private void ModifySuffixHandler()
        {
            if (HasSelectedObjects() == false) return;
            ModifySuffixCmd(oldSuffix, newSuffix);
        }

        [ShowIf("handleType", HandleType.Replace)]
        [LabelText("替换"), LabelWidth(60)]
        [ShowInInspector]
        private void ReplaceHandler()
        {
            if (HasSelectedObjects() == false) return;
            ReplaceCmd(replace_oldString, replace_newString);
        }



        protected bool HasSelectedObjects()
        {
            if (IsSelectedObject) return true;
            ShowMsgWarning("未选中对象!");
            return false;

        }


        /**** 重写 *****/

        protected virtual bool IsSelectedObject => false;


        protected virtual void ModifyPrefixCmd(string ori, string value)
        {
        }
        protected virtual void ModifySuffixCmd(string ori, string value)
        {
        }
        protected virtual void ReplaceCmd(string ori, string value)
        {
        }

        /**** 打印 *****/

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="info"></param>
        protected void ShowMsg(string info)
        {
            Debug.Log(info);
            this.ShowNotification(new GUIContent { text = info });

        }
        protected void ShowMsgWarning(string info)
        {
            Debug.LogWarning(info);
            this.ShowNotification(new GUIContent { text = info });

        }

    }
}

#endif