using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Gallery
{
    public class ContentLoader : MonoBehaviour
    {
        private static string BaseURL = "http://data.ikppbb.com/test-task-unity-data/pics/";
        private Task<Sprite> _task;
        private CancellationTokenSource _cancellation = new ();

        public event Action<Texture> LoadedImage;

        public void StartLoad(int index)
        {
            _cancellation.Cancel();
            _cancellation.Dispose();
            _cancellation = new CancellationTokenSource();
            LoadAsync(index, _cancellation.Token);
        }

        private async Task LoadAsync(int index, CancellationToken token)
        {
            string fullURL = BaseURL + $"{index}.jpg";
            var request = UnityWebRequestTexture.GetTexture(fullURL);
            request.SendWebRequest();
            while (!(request.isDone || _cancellation.IsCancellationRequested))
                await Task.Yield();

            if (token.IsCancellationRequested)
            {
                request.Abort();
                return;
            }
            
            if (request.result == UnityWebRequest.Result.Success)
                LoadedImage?.Invoke(DownloadHandlerTexture.GetContent(request));
            else
                Debug.LogErrorFormat($"Web request [{fullURL}] returned error: \"{request.error}]\"");
        }
    }
}