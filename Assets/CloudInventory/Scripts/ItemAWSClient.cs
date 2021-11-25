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

        }

        public override void GetItemsByType(string playerIID, int type, ClientJsonCallback callback)
        {

        }

        public override void CreateItem(string itemJson, ClientJsonCallback callback)
        {
            HelloWorldData data = new HelloWorldData();
            data.firstName = "Jimmy";
            data.lastName = "Jims";
            Post(serverURL, JsonUtility.ToJson(data), (json) => Debug.Log(json));
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

        private class HelloWorldData
        {
            public string firstName;
            public string lastName;
        }
    }
}