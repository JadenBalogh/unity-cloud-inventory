using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloudInventory
{
    [DisallowMultipleComponent]
    public abstract class BaseItemClient : MonoBehaviour
    {
        public delegate void ClientJsonCallback(string json);

        public abstract void GetItem(int itemID, ClientJsonCallback callback);
        public abstract void GetItems(int playerIID, ClientJsonCallback callback);
        public abstract void GetItemsByType(int playerIID, int type, ClientJsonCallback callback);
        public abstract void CreateItem(int playerIID, string itemJson, ClientJsonCallback callback);
        public abstract void SaveItem(int playerIID, string itemJson, ClientJsonCallback callback);
    }
}
