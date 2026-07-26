using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;

public static class ServerRequest
{
    public static readonly string SERVER_URL = "http://localhost:80/CustomerPortal";

    public static async Task<string> SendPostRequest(string url, Dictionary<string, string> data)
    {
        using (UnityWebRequest req = UnityWebRequest.Post(url, data))
        {
            UnityWebRequestAsyncOperation RequestTask = req.SendWebRequest();

            while (!RequestTask.isDone)
            {
                await Task.Yield();
            }

            if (req.result != UnityWebRequest.Result.Success)
            {
                return "{\"success\":false,\"message\":\"" + req.error + "\"}";
            }

            return req.downloadHandler.text;
        }
    }

    public static async Task<string> SendGetRequest(string url)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            UnityWebRequestAsyncOperation RequestTask = req.SendWebRequest();

            while (!RequestTask.isDone)
            {
                await Task.Yield();
            }

            if (req.result != UnityWebRequest.Result.Success)
            {
                return "{\"success\":false,\"message\":\"" + req.error + "\"}";
            }

            return req.downloadHandler.text;
        }
    }

    public static async Task<Sprite> GetSprite(string url)
    {
        if (string.IsNullOrEmpty(url)) return null;

        // If absolute URL, ensure localhost has the :80 port if needed
        if (url.StartsWith("http://") || url.StartsWith("https://"))
        {
            if (url.StartsWith("http://localhost/") && !url.StartsWith("http://localhost:80/"))
            {
                url = url.Replace("http://localhost/", "http://localhost:80/");
            }
        }
        else
        {
            // Relative URL: Prepend server base URL (http://localhost:80/CustomerPortal)
            url = SERVER_URL + "/" + url.TrimStart('/');
        }

        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
        {
            UnityWebRequestAsyncOperation op = req.SendWebRequest();
            while (!op.isDone)
            {
                await Task.Yield();
            }

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("Failed to load image from: " + url + " - Error: " + req.error);
                return null;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(req);
            if (texture != null)
            {
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
        return null;
    }
}
