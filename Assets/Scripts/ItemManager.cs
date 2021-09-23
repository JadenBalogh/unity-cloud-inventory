using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public delegate void GetItemsCallback(Item[] items);
    public delegate void AddItemCallback();
    private delegate void WebRequestCallback(string json);

    private const string SERVER_URL = "https://unity-cloud-inventory-server.herokuapp.com";

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
        instance.Get($"{SERVER_URL}/get-items", (json) =>
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

    public static void AddItem(string itemName, AddItemCallback callback)
    {
        RawItemData itemData = new RawItemData();
        itemData.name = itemName;
        string body = JsonUtility.ToJson(itemData);
        Debug.Log(body);
        instance.Post($"{SERVER_URL}/add-item", body, (json) => callback());
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

    [System.Serializable]
    private class RawItemData
    {
        public string name;
    }
}
