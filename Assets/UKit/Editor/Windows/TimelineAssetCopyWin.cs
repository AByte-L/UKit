using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;

namespace AByte.UKit.Editor
{

    /*
     * 查找场景中的组件
     * 根据组件配置的值
     */
    public class TimelineAssetCopyWin : BaseWin
    {

        [MenuItem("UKit/Timeline复制")]
        private static void Open()
        {
            var EditorWin = GetWindow<TimelineAssetCopyWin>(title: "Timeline轨道绑定修复");
            EditorWin.position = GUIHelper.GetEditorWindowRect().AlignCenter(420, 200);
        }

        [Space, LabelText("原始Timeline")] public PlayableAsset originTimeline;
        [Space, LabelText("新建Timeline")] public PlayableAsset newTimeline;

        [PropertyOrder(10), HideLabel, PropertySpace(SpaceBefore = 10, SpaceAfter = 10), Multiline(4), GUIColor("$GetMsgColor")]
        public string m_msg;

        protected override string Msg
        {
            get { return m_msg; }
            set { m_msg = value; }

        }


        [PropertySpace(SpaceBefore = 12, SpaceAfter = 4), ShowInInspector, Button(ButtonSizes.Medium), LabelText("复制")]
        private void ReadYaml()
        {
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
            string scenePath = scene.path;
            string originGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(originTimeline));
            string newGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(newTimeline));
            string yaml = File.ReadAllText(scenePath);
            string[] oldLines = File.ReadAllLines(scenePath);
            List<string> newLines = new List<string>();
            bool onPlayableDirector = false;
            bool onPlayableAsset = false; //标记正在处理
            bool hasCompleted = false;

            bool updateIsValid = true;

            for (int i = 0; i < oldLines.Length; i++)
            {
                var line = oldLines[i];

                if (hasCompleted)
                {
                    newLines.Add(line);
                }
                else if (onPlayableAsset)
                {
                    if (line.Contains(newGuid))
                    {
                        ++i;
                        continue;
                    }
                    else if (line.Contains(originGuid))
                    {
                        //替换
                        line = line.Replace(originGuid, newGuid);
                        newLines.Add(line);
                        if (updateIsValid == false) //标记以下，避免连续点击照常删除了已经修改好的配置
                            updateIsValid = true;
                    }
                    else
                    {
                        newLines.Add(line);
                    }

                    if (line.StartsWith("---"))
                    {
                        hasCompleted = true;
                    }

                }
                else if (onPlayableDirector)
                {
                    newLines.Add(line);
                    if (line.Contains("m_PlayableAsset") && line.Contains(newGuid))
                    {
                        onPlayableAsset = true;
                    }
                    else if (line.Contains("m_PlayableAsset") && !line.Contains(newGuid))
                    {
                        onPlayableDirector = false;
                    }
                }
                else
                {
                    newLines.Add(line);

                    if (line.Contains("PlayableDirector"))
                    {
                        onPlayableDirector = true;
                    }
                }
            }

            if (updateIsValid)
            {
                File.WriteAllLines(scenePath, newLines);
                AssetDatabase.Refresh();
                ShowMsgInfo(MsgType.Info, "复制数据完成！");
            }
            else
            {
                ShowMsgInfo(MsgType.Warning, "没有可修改的");
                //Debug.LogWarning("已经修改过了！！！");
            }

        }

    }
}

#endif




