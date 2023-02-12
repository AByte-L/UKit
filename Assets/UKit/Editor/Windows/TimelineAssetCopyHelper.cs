using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using System.IO;
using YamlDotNet.RepresentationModel;
using System.Collections.Generic;

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

        private IDictionary<YamlNode, YamlNode> _yn;

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




        }

        [ShowInInspector]
        private void ReadYaml()
        {
            scene = SceneManager.GetActiveScene();
            string scenePath = scene.path;
            string originGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(originTimeline));
            string newGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(newTimeline));


            string yaml = File.ReadAllText(scenePath);
            StringReader sr = new StringReader(yaml);
            YamlStream ys = new YamlStream();
            ys.Load(sr);





            // 读取root节点

            foreach (var doc in ys.Documents)
            {
                YamlMappingNode yamlMappingNode = (YamlMappingNode)doc.RootNode;
                _yn = yamlMappingNode.Children;
                foreach (var item in _yn)
                {
                    if (item.Key.ToString() == "PlayableDirector")
                    {
                        YamlMappingNode subMapp = (YamlMappingNode)item.Value;
                        var playableDirectorMappChilds = subMapp.Children;
                        if (playableDirectorMappChilds["m_PlayableAsset"].ToString().Contains(newGuid))
                        {

                            var sceneBindings = (YamlSequenceNode)playableDirectorMappChilds[new YamlScalarNode("m_SceneBindings")];
                            foreach (YamlMappingNode bindings in sceneBindings)
                            {
                                YamlMappingNode bindv = (YamlMappingNode)bindings.Children["key"];

                                string guid = bindv.Children["guid"].ToString();
                                if (guid == newGuid)
                                {
                                    bindv.Children["guid"] = originGuid;
                                }

                                else if (guid == originGuid)
                                {

                                    bindv.Children["guid"] = newGuid;

                                }

                            }

                            break;
                        }

                    }
                }

            }
            using (TextWriter writer = File.CreateText(scenePath))
            {
                ys.Save(writer);
            }



            // Debug.Log($"{_yn["NickName"]} --- {_yn["Age"]} --- {_yn["Height"]} --- {_yn["Hobbies"]}");
        }

    }
}

#endif




