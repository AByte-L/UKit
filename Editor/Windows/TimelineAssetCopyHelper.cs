using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using System.IO;

#if UNITY_EDITOR
using System;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor.SearchService;
using AByte.UKit.Editor.Utilities;

namespace AByte.UKit.Editor
{

    /*
     * 查找场景中的组件
     * 根据组件配置的值
     * 
     */
    public class TimelineAssetCopyHelper : BaseWin
    {

        const string CONF_FILE_NAME = "组件收集器";
        [MenuItem("UKit/Timeline复制")]
        private static void Open()
        {
            var EditorWin = GetWindow<TimelineAssetCopyHelper>(title: "Timeline复制");
            EditorWin.position = GUIHelper.GetEditorWindowRect().AlignCenter(550, 320);
        }
        public UnityEngine.SceneManagement.Scene scene;
        public PlayableAsset originTimeline;
        public PlayableAsset newTimeline;

        TimelineAsset dd;
        // Start is called before the first frame update
        void Start()
        {

        }
        [ShowInInspector]

        public void Fixed()
        {
            scene = SceneManager.GetActiveScene();
            string scenePath = scene.path;
            string originGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(originTimeline));
            string newGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(newTimeline));

            //读取场景文件并打印
            Debug.Log(scenePath);
            Debug.Log(File.ReadAllText(scenePath));

        }

    }
}

#endif




