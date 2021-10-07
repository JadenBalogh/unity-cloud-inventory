using System;
using UnityEngine;
using Newtonsoft.Json;

namespace CloudInventory
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager instance;

        public delegate void LoadItemsCallback<T>(T[] items);
        public delegate void SaveItemsCallback();

        public BaseItemClient Client { get; private set; }

        private void Awake()
        {
            // Enforce singleton behaviour
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

            // Requires an BaseItemClient component
            InitializeClient();
        }

        ///<summary>
        /// Standard method to load all items from the database for a given player.
        /// Array of BaseItems is sent to the callback upon retrieval.
        ///</summary>
        public static void LoadItems(int playerIID, LoadItemsCallback<BaseItem> callback) => LoadItems<BaseItem>(playerIID, callback);

        ///<summary>
        /// Generic method to load all items from the database for a given player.
        /// Array of given BaseItem type is sent to the callback upon retrieval.
        ///</summary>
        public static void LoadItems<T>(int playerIID, LoadItemsCallback<T> callback) where T : BaseItem
        {
            instance.Client.LoadItems(playerIID, (json) =>
            {
                RawItem[] rawItems = JsonUtility.FromJson<RawItems>(json).items;
                T[] items = new T[rawItems.Length];
                for (int i = 0; i < rawItems.Length; i++)
                {
                    RawItem raw = rawItems[i];

                    // Create instance of given item type
                    T item = Activator.CreateInstance<T>();
                    item.IID = raw.id;
                    item.Name = raw.item_name;

                    // Deserialize custom item data
                    ItemData customData = JsonConvert.DeserializeObject<ItemData>(raw.item_data, new ItemDataConverter());
                    item.Deserialize(customData);

                    // Add loaded item to list
                    items[i] = item;
                }
                callback(items);
            });
        }

        ///<summary>
        /// Standard method to save an item to the database for a given player.
        /// Upon successful save, callback is invoked.
        ///</summary>
        public static void SaveItem(int playerIID, BaseItem item, SaveItemsCallback callback)
        {
            RawItemData rawData = new RawItemData();
            rawData.name = item.Name;

            // Serialize custom item data
            ItemData customData = new ItemData();
            item.Serialize(customData);
            rawData.data = JsonConvert.SerializeObject(customData);

            string itemJson = JsonUtility.ToJson(rawData);
            instance.Client.SaveItem(playerIID, itemJson, (json) => callback());
        }

        // TODO: SaveItems function

        private void InitializeClient()
        {
            if (TryGetComponent<BaseItemClient>(out BaseItemClient client))
            {
                Client = client;
            }
            else
            {
                throw new MissingComponentException("ItemManager is missing a Client-type component!");
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
            public string item_data;
        }

        [System.Serializable]
        private class RawItemData
        {
            public string name;
            public string data;
        }
    }
}
