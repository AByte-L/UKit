#if UNITY_EDITOR

using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace AByte.UKit.Editor
{

    public class BaseWin : OdinEditorWindow
    {
        protected MsgType MsgType;
        protected virtual string Msg { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="info"></param>
        protected void ShowNotification(string info)
        {
            Debug.Log(info);
            this.ShowNotification(new GUIContent { text = info });

        }
        protected void ShowNotification_Warning(string info)
        {
            Debug.LogWarning(info);
            this.ShowNotification(new GUIContent { text = info });

        }


        protected void ShowMsgInfo(MsgType msgType, string msg)
        {
            this.MsgType = msgType;
            Msg = msg;
        }

        protected Color GetMsgColor()
        {
            Sirenix.Utilities.Editor.GUIHelper.RequestRepaint();
            switch (MsgType)
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