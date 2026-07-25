using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Networking;

public static class ServerRequest
{
    public static readonly string SERVER_URL = "http://localhost/CustomerPortal";

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
}
