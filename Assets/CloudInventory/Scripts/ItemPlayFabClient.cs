using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

namespace CloudInventory
{
    public class ItemPlayFabClient : BaseItemClient
    {
        [SerializeField] private string titleId = "83DA5";

        private delegate void PlayFabCallback();

        private bool isConnecting = false;
        private bool isConnected = false;

        public override void GetItem(string itemIID, ClientJsonCallback callback)
        {
            ValidateConnection(() =>
            {
                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "getItems",
                    FunctionParameter = new { id = itemIID }
                }, (result) =>
                {
                    JsonObject res = (JsonObject)result.FunctionResult;
                    string ret = (string)res["ret"];
                    callback(ret);
                }, (err) =>
                {
                    Debug.Log(err.ErrorMessage);
                });
            });
        }

        public override void GetItems(string playerIID, ClientJsonCallback callback)
        {
            ValidateConnection(() =>
            {
                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "getItems",
                    FunctionParameter = new { playerId = playerIID }
                }, (result) =>
                {
                    JsonObject res = (JsonObject)result.FunctionResult;
                    string ret = (string)res["ret"];
                    callback(ret);
                }, (err) =>
                {
                    Debug.Log(err.ErrorMessage);
                });
            });
        }

        public override void GetItemsByType(string playerIID, int type, ClientJsonCallback callback)
        {
            ValidateConnection(() =>
            {
                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "getItems",
                    FunctionParameter = new { playerId = playerIID, type = type }
                }, (result) =>
                {
                    JsonObject res = (JsonObject)result.FunctionResult;
                    string ret = (string)res["ret"];
                    callback(ret);
                }, (err) =>
                {
                    Debug.Log(err.ErrorMessage);
                });
            });
        }

        public override void CreateItem(string itemJson, ClientJsonCallback callback)
        {
            ValidateConnection(() =>
            {
                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "addItem",
                    FunctionParameter = new { data = itemJson }
                }, (result) =>
                {
                    JsonObject res = (JsonObject)result.FunctionResult;
                    string ret = (string)res["ret"];
                    callback(ret);
                }, (err) =>
                {
                    Debug.Log(err.ErrorMessage);
                });
            });
        }

        public override void UpdateItem(string itemIID, string itemJson, ClientJsonCallback callback)
        {
            ValidateConnection(() =>
            {
                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "updateItem",
                    FunctionParameter = new { id = itemIID, data = itemJson }
                }, (result) =>
                {
                    JsonObject res = (JsonObject)result.FunctionResult;
                    string ret = (string)res["ret"];
                    callback(ret);
                }, (err) =>
                {
                    Debug.Log(err.ErrorMessage);
                });
            });
        }

        public override void DeleteItem(string itemIID, ClientJsonCallback callback)
        {
            ValidateConnection(() =>
            {
                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "deleteItem",
                    FunctionParameter = new { id = itemIID }
                }, (result) =>
                {
                    callback("");
                }, (err) =>
                {
                    Debug.Log(err.ErrorMessage);
                });
            });
        }

        public override void TradeItem(string itemIID, string playerIID, ClientJsonCallback callback)
        {
            ValidateConnection(() =>
            {
                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "tradeItem",
                    FunctionParameter = new { id = itemIID, playerId = playerIID }
                }, (result) =>
                {
                    JsonObject res = (JsonObject)result.FunctionResult;
                    string ret = (string)res["ret"];
                    callback(ret);
                }, (err) =>
                {
                    Debug.Log(err.ErrorMessage);
                });
            });
        }

        private void ValidateConnection(PlayFabCallback callback)
        {
            if (!isConnected && !isConnecting)
            {
                isConnecting = true;
                LoginWithCustomIDRequest request = new LoginWithCustomIDRequest();
                request.TitleId = titleId;
                request.CustomId = "ItemPlayer";
                request.CreateAccount = true;
                PlayFabClientAPI.LoginWithCustomID(request, (result) =>
                {
                    Debug.Log("Logged in player " + result.PlayFabId);
                    isConnected = true;
                    callback();
                }, (err) =>
                {
                    Debug.Log(err.ErrorMessage);
                });
            }
            else if (isConnecting)
            {
                StartCoroutine(WaitConnected(callback));
            }
            else if (isConnected)
            {
                callback();
            }
        }

        private IEnumerator WaitConnected(PlayFabCallback callback)
        {
            yield return new WaitUntil(() => isConnected);
            callback();
        }
    }
}
