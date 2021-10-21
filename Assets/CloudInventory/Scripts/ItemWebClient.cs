using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace CloudInventory
{
    public class ItemWebClient : BaseItemClient
    {
        [SerializeField] private string serverURL = "https://unity-cloud-inventory-server.herokuapp.com";

        private delegate void WebRequestCallback(string json);

        public override void GetItem(int itemIID, ClientJsonCallback callback)
        {
            Get($"{serverURL}/get-item?itemId={itemIID}", (json) => callback(json));
        }

        public override void GetItems(int playerIID, ClientJsonCallback callback)
        {
            Get($"{serverURL}/get-items?playerId={playerIID}", (json) => callback(json));
        }

        public override void GetItemsByType(int playerIID, int type, ClientJsonCallback callback)
        {
            Get($"{serverURL}/get-items-by-type?playerId={playerIID}&type={type}", (json) => callback(json));
        }

        public override void CreateItem(string itemJson, ClientJsonCallback callback)
        {
            Post($"{serverURL}/add-item", itemJson, (json) => callback(json));
        }

        public override void UpdateItem(int itemIID, string itemJson, ClientJsonCallback callback)
        {
            Post($"{serverURL}/update-item?itemId={itemIID}", itemJson, (json) => callback(json));
        }

        public override void DeleteItem(int itemIID, ClientJsonCallback callback)
        {
            Get($"{serverURL}/remove-item?itemId={itemIID}", (json) => callback(json));
        }

        public override void TradeItem(int itemIID, int playerIID, ClientJsonCallback callback)
        {
            Get($"{serverURL}/trade-item?itemId={itemIID}&playerId={playerIID}", (json) => callback(json));
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
}
