using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkTest : MonoBehaviour
{
    public delegate void WebRequestCallback(byte[] result);

    private void Start()
    {
        Get("http://localhost:3000/test");
    }

    public void Get(string uri, WebRequestCallback callback = null) => StartCoroutine(GetAsync(uri, callback));

    private IEnumerator GetAsync(string uri, WebRequestCallback callback = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Perform action based on the result
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
                    if (callback != null) callback(webRequest.downloadHandler.data);
                    break;
            }
        }
    }
}
