using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloudInventory
{
    [DisallowMultipleComponent]
    public abstract class BaseItemClient : MonoBehaviour
    {
        public delegate void ClientJsonCallback(string json);

        public abstract void LoadItems(int playerIID, ClientJsonCallback callback);
        public abstract void SaveItem(int playerIID, string itemJson, ClientJsonCallback callback);
    }
}
