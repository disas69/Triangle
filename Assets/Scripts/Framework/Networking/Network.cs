using System;
using System.Collections;
using Framework.Tasking;
using UnityEngine;
using UnityEngine.Networking;

namespace Framework.Networking
{
    public static class Network
    {
        public static void Request(string url, Action<string> response)
        {
            Task.Create(LoadText(url, response)).Start();
        }

        public static void Request(string url, Action<Texture2D> response)
        {
            Task.Create(LoadTexture(url, response)).Start();
        }

        public static void Request(string url, Action<AssetBundle> response)
        {
            Task.Create(LoadAssetBundle(url, response)).Start();
        }

        private static IEnumerator LoadText(string url, Action<string> response)
        {
            var request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            if (!request.isHttpError && !request.isNetworkError)
            {
                response(request.downloadHandler.text);
            }
            else
            {
                Debug.LogErrorFormat("Request error [{0}, {1}]", url, request.error);
                response(null);
            }

            request.Dispose();
        }

        private static IEnumerator LoadTexture(string url, Action<Texture2D> response)
        {
            var request = UnityWebRequestTexture.GetTexture(url);

            yield return request.SendWebRequest();

            if (!request.isHttpError && !request.isNetworkError)
            {
                response(DownloadHandlerTexture.GetContent(request));
            }
            else
            {
                Debug.LogErrorFormat("Request error [{0}, {1}]", url, request.error);
                response(null);
            }

            request.Dispose();
        }

        private static IEnumerator LoadAssetBundle(string url, Action<AssetBundle> response)
        {
            var request = UnityWebRequestAssetBundle.GetAssetBundle(url);

            yield return request.SendWebRequest();

            if (!request.isHttpError && !request.isNetworkError)
            {
                response(DownloadHandlerAssetBundle.GetContent(request));
            }
            else
            {
                Debug.LogErrorFormat("Request error [{0}, {1}]", url, request.error);
                response(null);
            }

            request.Dispose();
        }
    }
}