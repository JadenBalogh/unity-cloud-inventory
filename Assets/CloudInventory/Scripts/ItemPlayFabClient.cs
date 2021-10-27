using System.Text;
using System.Collections;
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

        public delegate void PlayFabCallback();

        public void ConnectPlayer(string player, PlayFabCallback callback)
        {
            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest();
            request.TitleId = titleId;
            request.CustomId = player;
            request.CreateAccount = true;
            PlayFabClientAPI.LoginWithCustomID(request, (result) =>
            {
                Debug.Log("Logged in player " + result.PlayFabId);
                callback();
            }, (err) =>
            {
                Debug.Log(err.ErrorMessage);
            });
        }

        public override void GetItem(int itemIID, ClientJsonCallback callback)
        {

        }

        public override void GetItems(int playerIID, ClientJsonCallback callback)
        {

        }

        public override void GetItemsByType(int playerIID, int type, ClientJsonCallback callback)
        {

        }

        public override void CreateItem(string itemJson, ClientJsonCallback callback)
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "addItem",
                FunctionParameter = new { data = itemJson }
            }, (result) =>
            {
                Debug.Log("Created item! Finished calling: " + result.FunctionName);
                JsonObject res = (JsonObject)result.FunctionResult;
                string id = (string)res["id"];
                string data = (string)res["data"];
                Debug.Log("Created item id: " + id);
                Debug.Log("Created item data: " + data);
                callback("");
            }, (err) =>
            {
                Debug.Log(err.ErrorMessage);
            });
        }

        public override void UpdateItem(int itemIID, string itemJson, ClientJsonCallback callback)
        {

        }

        public override void DeleteItem(int itemIID, ClientJsonCallback callback)
        {

        }

        public override void TradeItem(int itemIID, int playerIID, ClientJsonCallback callback)
        {

        }
    }
}
