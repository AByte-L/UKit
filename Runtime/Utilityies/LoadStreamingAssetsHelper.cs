using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace AByte.UKit.Utilities
{

    /*
     * 文件夹工具 
     */
    public static class LoadStreamingAssetsHelper
    {
        /// <summary>
        /// 读取 StramingAssets 中的文本 
        /// </summary>
        /// <param name="fileName">文件名称，包含扩展名</param>
        /// <returns></returns>
        public static string LoadStramingAssetsText(string fileName)
        {
            UnityWebRequest request;

#if UNITY_ANDROID && !UNITY_EDITOR
            // 安卓平台下的路径 安卓平台下读取StreamingAssets中的资源只能通过 UnityWebRequest 来读取， 以前是 www ，目前以弃用
            // 安卓平台下 不能使用 System.IO 中的 方法
            request = new UnityWebRequest("jar:file://" + Application.dataPath + "!/assets" + "/" + fileName);

#else // UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_IPHONE 这些的路径一样
            request = new UnityWebRequest(Application.streamingAssetsPath + "/" + fileName);
#endif
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
            UnityWebRequest request;

#if UNITY_ANDROID && !UNITY_EDITOR
            // 安卓平台下的路径 安卓平台下读取StreamingAssets中的资源只能通过 UnityWebRequest 来读取， 以前是 www ，目前以弃用
            // 安卓平台下 不能使用 System.IO 中的 方法
            request = new UnityWebRequest("jar:file://" + Application.dataPath + "!/assets" + "/" + fileName);

#else // UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_IPHONE 这些的路径一样
            request = new UnityWebRequest(Application.streamingAssetsPath + "/" + fileName);
#endif
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