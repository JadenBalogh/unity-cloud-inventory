using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloudInventory
{
    public class ItemAWSClient : ItemWebClient
    {
        public override void GetItem(string itemIID, ClientJsonCallback callback)
        {

        }

        public override void GetItems(string playerIID, ClientJsonCallback callback)
        {
            Get($"{serverURL}/GetItems?playerId={playerIID}", (json) => callback(json));
        }

        public override void GetItemsByType(string playerIID, int type, ClientJsonCallback callback)
        {

        }

        public override void CreateItem(string itemJson, ClientJsonCallback callback)
        {
            Post(serverURL, itemJson, (json) => callback(json));
        }

        public override void UpdateItem(string itemIID, string itemJson, ClientJsonCallback callback)
        {

        }

        public override void DeleteItem(string itemIID, ClientJsonCallback callback)
        {

        }

        public override void TradeItem(string itemIID, string playerIID, ClientJsonCallback callback)
        {

        }
    }
}
