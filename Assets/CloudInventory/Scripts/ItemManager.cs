using System;
using UnityEngine;
using Newtonsoft.Json;

namespace CloudInventory
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager instance;

        public delegate void GetItemCallback<T>(T item);
        public delegate void GetItemsCallback<T>(T[] items);
        public delegate void CreateItemCallback();

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
        /// Standard method to load an item from the database by ID.
        /// BaseItem is sent to the callback upon retrieval.
        ///</summary>
        public static void GetItem(int playerIID, GetItemCallback<BaseItem> callback) => GetItem<BaseItem>(playerIID, callback);

        ///<summary>
        /// Generic method to load an item from the database by ID.
        /// BaseItem of specified type is sent to the callback upon retrieval.
        ///</summary>
        public static void GetItem<T>(int playerIID, GetItemCallback<T> callback) where T : BaseItem
        {
            instance.Client.GetItem(playerIID, (json) => callback(instance.DeserializeItem<T>(json)));
        }

        ///<summary>
        /// Standard method to load all items from the database for a given player.
        /// Array of BaseItems is sent to the callback upon retrieval.
        ///</summary>
        public static void GetItems(int playerIID, GetItemsCallback<BaseItem> callback) => GetItems<BaseItem>(playerIID, callback);

        ///<summary>
        /// Generic method to load all items from the database for a given player.
        /// Array of given BaseItem type is sent to the callback upon retrieval.
        ///</summary>
        public static void GetItems<T>(int playerIID, GetItemsCallback<T> callback) where T : BaseItem
        {
            instance.Client.GetItems(playerIID, (json) => callback(instance.DeserializeItems<T>(json)));
        }

        ///<summary>
        /// Standard method to load all items from the database for a given player with a given type.
        /// Array of BaseItems is sent to the callback upon retrieval.
        ///</summary>
        public static void GetItems(int playerIID, int type, GetItemsCallback<BaseItem> callback) => GetItems<BaseItem>(playerIID, type, callback);

        ///<summary>
        /// Generic method to load all items from the database for a given player with a given type.
        /// Array of given BaseItem type is sent to the callback upon retrieval.
        ///</summary>
        public static void GetItems<T>(int playerIID, int type, GetItemsCallback<T> callback) where T : BaseItem
        {
            instance.Client.GetItemsByType(playerIID, type, (json) => callback(instance.DeserializeItems<T>(json)));
        }

        ///<summary>
        /// Standard method to create an item in the database for a given player.
        /// Upon successful save, callback is invoked.
        ///</summary>
        public static void CreateItem(int playerIID, BaseItem item, CreateItemCallback callback)
        {
            RawItemData rawData = new RawItemData();
            rawData.name = item.Name;
            rawData.type = item.Type;

            // Serialize custom item data
            ItemData customData = new ItemData();
            item.Serialize(customData);
            rawData.data = JsonConvert.SerializeObject(customData);

            string itemJson = JsonUtility.ToJson(rawData);
            instance.Client.CreateItem(playerIID, itemJson, (json) => callback());
        }

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

        private T DeserializeItem<T>(string json) where T : BaseItem
        {
            RawItem rawItem = JsonUtility.FromJson<RawItem>(json);
            return CreateItemInstance<T>(rawItem);
        }

        private T[] DeserializeItems<T>(string json) where T : BaseItem
        {
            RawItem[] rawItems = JsonUtility.FromJson<RawItems>(json).items;
            T[] items = new T[rawItems.Length];
            for (int i = 0; i < rawItems.Length; i++)
            {
                RawItem rawItem = rawItems[i];
                T item = CreateItemInstance<T>(rawItem);
                items[i] = item;
            }
            return items;
        }

        private T CreateItemInstance<T>(RawItem raw) where T : BaseItem
        {
            // Create instance of given item type
            T item = Activator.CreateInstance<T>();
            item.IID = raw.id;
            item.Name = raw.item_name;
            item.Type = raw.item_type;

            // Deserialize custom item data
            ItemData customData = JsonConvert.DeserializeObject<ItemData>(raw.item_data, new ItemDataConverter());
            item.Deserialize(customData);

            // Return loaded item
            return item;
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
            public int item_type;
            public string item_data;
        }

        [System.Serializable]
        private class RawItemData
        {
            public string name;
            public int type;
            public string data;
        }
    }
}
