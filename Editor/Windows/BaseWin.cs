#if UNITY_EDITOR

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace AByte.UKit.Editor
{

    public class BaseWin : OdinEditorWindow
    {

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