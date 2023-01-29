#if UNITY_EDITOR
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
    public class ModifyGameObjectNameWin : HandTypeBaseWin
    {
        [MenuItem("UKit/修改场景对象名")]
        private static void Open()
        {
            var EditorWin = GetWindow<ModifyGameObjectNameWin>(title: "修改场景对象名");
            EditorWin.position = GUIHelper.GetEditorWindowRect().AlignCenter(500, 200);
        }

        protected override bool IsSelectedObject => Selection.gameObjects.Length > 0;

        protected override void ModifyPrefixCmd(string oldPrefix, string newPrefix)
        {
            foreach (var item in Selection.gameObjects)
            {
                if (newPrefix != null) newPrefix.Trim();
                if (oldPrefix == null)
                {
                    item.name = newPrefix + item.name;
                }
                else
                {
                    if (item.name.StartsWith(oldPrefix))
                    {
                        item.name = newPrefix + item.name.Remove(0, oldPrefix.Length);
                    }
                }

            }
            this.ShowNotification(new GUIContent { text = "更新完成！" });
        }


        protected override void ModifySuffixCmd(string oldSuffix, string newSuffix)
        {
            foreach (var item in Selection.gameObjects)
            {

                if (newSuffix != null) newSuffix.Trim();
                if (oldSuffix == null)
                {
                    item.name = item.name + newSuffix;
                }
                else
                {
                    if (item.name.EndsWith(oldSuffix))
                    {
                        item.name = item.name.Remove(item.name.Length - oldSuffix.Length, oldSuffix.Length) + newSuffix;
                    }
                }
            }
            this.ShowNotification(new GUIContent { text = "更新完成！" });
        }



        protected override void ReplaceCmd(string replace_oldString, string replace_newString)
        {
            if (string.IsNullOrEmpty(replace_oldString))
            {
                this.ShowNotification(new GUIContent { text = "被替换的字符不能为空！" });
                return;
            }
            int count = 0;
            foreach (var item in Selection.gameObjects)
            {
                if (item.name.Contains(replace_oldString))
                {
                    item.name = item.name.Replace(replace_oldString, replace_newString);
                    count++;
                }
            }
            string info = $"共 {count} 个对象的名称被替换完成！";
            Debug.Log(info);
            this.ShowNotification(new GUIContent { text = info });
        }

    }
}

#endif