using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

namespace AByte.UKit.Utilities
{

    /*
     * 文件夹工具 
     */
    public static class LoadStreamingAssetsHelper
    {

        /// <summary>
        /// 使用UnityWebRequest请求时的地址（根据各个平台处理）
        /// 注：这个路径不能使用File来使用
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetRequestURL(string fileName)
        {

            string path = Path.Combine(Application.streamingAssetsPath, fileName);

#if UNITY_ANDROID && !UNITY_EDITOR
            path ="jar:file://" + ptah;
#elif UNITY_EIDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IPHONE //苹果下是这样
            path ="file://"+ ptah;

// #else // UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_IPHONE 这些的路径一样
//             request = new UnityWebRequest(Application.streamingAssetsPath + "/" + fileName);
#endif
            return path;
        }

        /// <summary>
        /// 读取 StramingAssets 中的文本 
        /// </summary>
        /// <param name="fileName">文件名称，包含扩展名</param>
        /// <returns></returns>
        public static string LoadStramingAssetsText(string fileName)
        {
            UnityWebRequest request = new UnityWebRequest(GetRequestURL(fileName));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SendWebRequest();
            while (!request.isDone) { }
            return request.downloadHandler.text;
        }

        /// <summary>
        /// 读取 StramingAssets 中的文本 
        /// </summary>
        /// <param name="fileName">文件名称，包含扩展名</param>
        /// <param name="callback">读完回调</param>
        /// <returns></returns>
        public static IEnumerator LoadStramingAssetsTextAsync(string fileName, Action<string> callback)
        {
            UnityWebRequest request = new UnityWebRequest(GetRequestURL(fileName));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SendWebRequest();
            while (!request.isDone)
            {
                yield return null;
            }
            callback?.Invoke(request.downloadHandler.text);
        }
    }
}