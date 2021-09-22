using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public delegate void GetItemsCallback(Item[] items);
    private delegate void WebRequestCallback(string json);

    private const string SERVER_URL = "";

    private void Awake()
    {
        // Enforce singleton behaviour
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public static void GetItems(GetItemsCallback callback)
    {
        instance.Get("http://localhost:3000/get-items", (json) =>
        {
            RawItem[] rawItems = JsonUtility.FromJson<RawItems>(json).items;
            Item[] items = new Item[rawItems.Length];
            for (int i = 0; i < rawItems.Length; i++)
            {
                RawItem raw = rawItems[i];
                items[i] = new Item(raw.id, raw.item_name);
            }
            callback(items);
        });
    }

    private void Get(string uri, WebRequestCallback callback = null) => StartCoroutine(GetAsync(uri, callback));

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
                    if (callback != null)
                    {
                        callback(webRequest.downloadHandler.text);
                    }
                    break;
            }
        }
    }

    [System.Serializable]
    private class RawItems
    {
        public RawItem[] items;
    }

    [System.Serializable]
    private class RawItem
    {
        public int id;
        public string item_name;
    }
}
