using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemWebClient : BaseItemClient
{
    [SerializeField] private string serverURL = "https://unity-cloud-inventory-server.herokuapp.com";

    private delegate void WebRequestCallback(string json);

    public override void LoadItems(int playerIID, ClientJsonCallback callback)
    {
        Get($"{serverURL}/get-items?playerId={playerIID}", (json) => callback(json));
    }

    public override void SaveItem(int playerIID, string itemJson, ClientJsonCallback callback)
    {
        Post($"{serverURL}/add-item?playerId={playerIID}", itemJson, (json) => callback(json));
    }

    private void Get(string uri, WebRequestCallback callback = null) => StartCoroutine(GetAsync(uri, callback));

    private IEnumerator GetAsync(string uri, WebRequestCallback callback = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received: " + webRequest.downloadHandler.text);
                    if (callback != null)
                    {
                        callback(webRequest.downloadHandler.text);
                    }
                    break;
            }
        }
    }

    private void Post(string uri, string body, WebRequestCallback callback = null) => StartCoroutine(PostAsync(uri, body, callback));

    private IEnumerator PostAsync(string uri, string body, WebRequestCallback callback = null)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(uri, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received: " + webRequest.downloadHandler.text);
                    if (callback != null)
                    {
                        callback(webRequest.downloadHandler.text);
                    }
                    break;
            }
        }
    }
}
