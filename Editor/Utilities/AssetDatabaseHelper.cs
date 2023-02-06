using UnityEditor;

namespace AByte.UKit.Editor.Utilities
{

    /*
     * 文件夹工具 
     */
    public static class AssetDatabaseHelper
    {

        /// <summary>
        /// 在 Assets 文件夹下创建文件夹
        /// 直接传你要创建的路径，目录不存在会自动创建
        /// </summary>
        /// <param name="folderPath">文件夹路径，如 "Assets/Folder/SubFodler"</param>
        public static void CreateFolder(string folderPath)
        {
            //逐步检查路径并创建
            string[] pathArr = folderPath.Split('/');
            if (pathArr.Length < 2) return;

            string parentPath = "Assets";
            for (int i = 1; i < pathArr.Length; i++)
            {
                string path = @$"{parentPath}/{pathArr[i]}";
                bool valid = AssetDatabase.IsValidFolder(path);
                if (!valid)
                    AssetDatabase.CreateFolder(parentPath, pathArr[i]);
                parentPath = path;

            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 创建资源
        /// </summary>
        /// <param name="asset">资源对象</param>
        /// <param name="folderPath">资源所在的文件夹</param>
        /// <param name="assetName">资源名称，包含扩展名，如demo.asset</param>
        public static void CreateAsset(UnityEngine.Object asset, string folderPath, string assetName)
        {
            //创建路径
            CreateFolder(folderPath);
            AssetDatabase.CreateAsset(asset, @$"{folderPath}/{assetName}");
            AssetDatabase.SaveAssets(); //存储资源
            AssetDatabase.Refresh();

        }

    }
}