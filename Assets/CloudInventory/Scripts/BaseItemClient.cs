using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloudInventory
{
    [DisallowMultipleComponent]
    public abstract class BaseItemClient : MonoBehaviour
    {
        public delegate void ClientJsonCallback(string json);

        public abstract void GetItem(int itemIID, ClientJsonCallback callback);
        public abstract void GetItems(string playerIID, ClientJsonCallback callback);
        public abstract void GetItemsByType(string playerIID, int type, ClientJsonCallback callback);
        public abstract void CreateItem(string itemJson, ClientJsonCallback callback);
        public abstract void UpdateItem(int itemIID, string itemJson, ClientJsonCallback callback);
        public abstract void DeleteItem(int itemIID, ClientJsonCallback callback);
        public abstract void TradeItem(int itemIID, string playerIID, ClientJsonCallback callback);
    }
}
