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
    public class ModifAssetNameWin : HandTypeBaseWin
    {
        [MenuItem("UKit/修改资源对象名")]
        private static void Open()
        {
            var EditorWin = GetWindow<ModifAssetNameWin>(title: "修改资源对象名");
            EditorWin.position = GUIHelper.GetEditorWindowRect().AlignCenter(500, 200);
        }
        protected override bool IsSelectedObject => Selection.assetGUIDs.Length > 0;


        protected override void ModifyPrefixCmd(string oldStart, string newStart)
        {
            foreach (var item in Selection.assetGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(item);
                var asset = AssetDatabase.LoadMainAssetAtPath(path);
                if (asset == null) continue;
                if (newStart != null) newStart.Trim();

                if (oldStart == null)
                {
                    AssetDatabase.RenameAsset(path, newStart + asset.name);
                }
                else
                {
                    if (asset.name.StartsWith(oldStart))
                    {
                        AssetDatabase.RenameAsset(path, newStart + asset.name.Remove(0, oldStart.Length));
                    }
                }

            }
            this.ShowNotification(new GUIContent { text = "更新完成！" });
        }


        protected override void ModifySuffixCmd(string oldEnd, string newEnd)
        {
            int count = 0;
            foreach (var item in Selection.assetGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(item);
                var asset = AssetDatabase.LoadMainAssetAtPath(path);
                if (asset == null) continue;

                if (newEnd != null) newEnd.Trim();
                if (oldEnd == null)
                {
                    AssetDatabase.RenameAsset(path, asset.name + newEnd);
                    count++;
                }
                else
                {
                    if (asset.name.EndsWith(oldEnd))
                    {
                        AssetDatabase.RenameAsset(path, asset.name.Remove(asset.name.Length - oldEnd.Length, oldEnd.Length) + newEnd);
                        count++;
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
            foreach (var item in Selection.assetGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(item);
                var asset = AssetDatabase.LoadMainAssetAtPath(path);
                if (asset == null) continue;

                if (asset.name.Contains(replace_oldString))
                {
                    string newName = asset.name.Replace(replace_oldString, replace_newString);
                    AssetDatabase.RenameAsset(path, newName);
                    count++;
                }

            }
            string info = $"共 {count} 个对象的名称被替换完成！";
            ShowMsg(info);
        }

    }
}

#endif