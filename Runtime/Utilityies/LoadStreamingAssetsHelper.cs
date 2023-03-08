using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

namespace AByte.UKit.Utilities
{

    /*
     * StreamingAssets文件夹下处理
     * File方式：           只有安卓端不支持，其他平台都支持，且路径都是Application.StreamingAssetsPath,不要在前面加前缀
     * UnityWebRequest方式：所有平台都支持，但是路径不一致
        win：   不加前缀
        安卓：   前缀："jar:file://"
        mac/ios：前缀："file://"
      使用UnityWebRequest请求时的地址（根据各个平台处理）
      注：这个路径不能使用File来使用
     */
    public static class LoadStreamingAssetsHelper
    {

        /// <summary>
        /// UnityWebRequest 访问地址
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetUWR_URL(string fileName)
        {

            string path = Path.Combine(Application.streamingAssetsPath, fileName);

#if UNITY_ANDROID && !UNITY_EDITOR
            path ="jar:file://" + ptah;
#elif UNITY_EIDITOR_OSX || UNITY_STANDALONE_OSX || (UNITY_IPHONE&&!UNITY_EDITOR_WIN) //苹果下是这样
            path ="file://"+ ptah;
#endif
            return path;
        }

        /// <summary>
        /// 读取 StramingAssets 中的文本 
        /// </summary>
        /// <param name="fileName">文件名称，包含扩展名</param>
        /// <returns></returns>
        public static string ReadTextCommon(string fileName)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return ReadByUWR(fileName);
#else
            ReadByFile(fileName);
#endif
            return null;

        }

        /// <summary>
        /// 通过File的方式读取，【安卓端无效】
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadByFile(string fileName)
        {
            string path = Path.Combine(Application.streamingAssetsPath, fileName);
            return File.ReadAllText(path);

        }

        /// <summary>
        /// 通过 UnityWebRequest 读取
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadByUWR(string fileName)
        {
            UnityWebRequest request = new UnityWebRequest(GetUWR_URL(fileName));
            request.downloadHandler = new DownloadHandlerBuffer();
            var handle = request.SendWebRequest();
            while (!request.isDone) { }
            return request.downloadHandler.text;
        }

        /// <summary>
        /// 协程调用方式
        /// </summary>
        /// <param name="fileName">文件名称，包含扩展名</param>
        /// <param name="callback">读完回调</param>
        /// <returns></returns>
        public static IEnumerator ReadByUWR_Coroutine(string fileName, Action<string> callback)
        {
            UnityWebRequest request = new UnityWebRequest(GetUWR_URL(fileName));
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                callback?.Invoke(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError(request.error);
            }

        }

        /// <summary>
        /// 协程调用方式
        /// </summary>
        /// <param name="fileName">文件名称，包含扩展名</param>
        /// <param name="callback">读完回调</param>
        /// <returns></returns>
        public static void ReadByUWR(string fileName, Action<string> callback)
        {
            UnityWebRequest request = new UnityWebRequest(GetUWR_URL(fileName));
            request.downloadHandler = new DownloadHandlerBuffer();
            var op = request.SendWebRequest();
            op.completed += _ =>
            {
                if (op.isDone)
                    callback?.Invoke(request.downloadHandler.text);
                else
                    Debug.LogError(request.error);
            };
        }
    }
}