using System;
using UnityEngine;
using Newtonsoft.Json;

namespace CloudInventory
{
    public enum ItemClientType
    {
        WebClient,
        PlayFabClient
    }

    public class ItemManager : MonoBehaviour
    {
        private static ItemManager instance;

        public delegate void GetItemCallback<T>(T item);
        public delegate void GetItemsCallback<T>(T[] items);
        public delegate void ModifyItemCallback();

        private BaseItemClient client;
        public static BaseItemClient Client { get => instance.client; }
        public static ItemClientType ClientType
        {
            get
            {
                switch (Client.GetType().ToString())
                {
                    case "CloudInventory.ItemWebClient": return ItemClientType.WebClient;
                    case "CloudInventory.ItemPlayFabClient": return ItemClientType.PlayFabClient;
                }
                return ItemClientType.WebClient;
            }
        }

        private void Awake()
        {
            // Enforce singleton behaviour
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

            // Require a BaseItemClient component
            InitializeClient();
        }

        ///<summary>
        /// Returns the current item client by the given generic type.
        ///</summary>
        public static T GetClient<T>() where T : BaseItemClient
        {
            return (T)Client;
        }

        ///<summary>
        /// Standard method to load an item from the database by ID.
        /// BaseItem is sent to the callback upon retrieval.
        ///</summary>
        public static void GetItem(int itemIID, GetItemCallback<BaseItem> callback) => GetItem<BaseItem>(itemIID, callback);

        ///<summary>
        /// Generic method to load an item from the database by ID.
        /// BaseItem of specified type is sent to the callback upon retrieval.
        ///</summary>
        public static void GetItem<T>(int itemIID, GetItemCallback<T> callback) where T : BaseItem
        {
            Client.GetItem(itemIID, (json) => callback(instance.DeserializeItem<T>(json)));
        }

        ///<summary>
        /// Standard method to load all items from the database for a given player.
        /// Array of BaseItems is sent to the callback upon retrieval.
        ///</summary>
        public static void GetItems(string playerIID, GetItemsCallback<BaseItem> callback) => GetItems<BaseItem>(playerIID, callback);

        ///<summary>
        /// Generic method to load all items from the database for a given player.
        /// Array of given BaseItem type is sent to the callback upon retrieval.
        ///</summary>
        public static void GetItems<T>(string playerIID, GetItemsCallback<T> callback) where T : BaseItem
        {
            Client.GetItems(playerIID, (json) => callback(instance.DeserializeItems<T>(json)));
        }

        ///<summary>
        /// Standard method to load all items from the database for a given player with a given type.
        /// Array of BaseItems is sent to the callback upon retrieval.
        ///</summary>
        public static void GetItems(string playerIID, int type, GetItemsCallback<BaseItem> callback) => GetItems<BaseItem>(playerIID, type, callback);

        ///<summary>
        /// Generic method to load all items from the database for a given player with a given type.
        /// Array of given BaseItem type is sent to the callback upon retrieval.
        ///</summary>
        public static void GetItems<T>(string playerIID, int type, GetItemsCallback<T> callback) where T : BaseItem
        {
            Client.GetItemsByType(playerIID, type, (json) => callback(instance.DeserializeItems<T>(json)));
        }

        ///<summary>
        /// Standard method to create an item in the database.
        ///</summary>
        public static void CreateItem(BaseItem item) => CreateItem(item, () => { });

        ///<summary>
        /// Standard method to create an item in the database.
        /// BaseItem is sent to the callback upon creation.
        ///</summary>
        public static void CreateItem(BaseItem item, GetItemCallback<BaseItem> callback) => CreateItem<BaseItem>(item, callback);

        ///<summary>
        /// Standard method to create an item in the database.
        /// Callback is invoked upon completion.
        ///</summary>
        public static void CreateItem(BaseItem item, ModifyItemCallback callback)
        {
            Client.CreateItem(instance.SerializeItem(item), (json) => callback());
        }

        ///<summary>
        /// Generic method to create an item in the database.
        /// BaseItem of specified type is sent to the callback upon creation.
        ///</summary>
        public static void CreateItem<T>(BaseItem item, GetItemCallback<T> callback) where T : BaseItem
        {
            Client.CreateItem(instance.SerializeItem(item), (json) => callback(instance.DeserializeItem<T>(json)));
        }

        ///<summary>
        /// Standard method to update an item in the database.
        ///</summary>
        public static void UpdateItem(BaseItem item) => UpdateItem(item, () => { });

        ///<summary>
        /// Standard method to update an item in the database.
        /// Updated BaseItem is sent to the callback once complete.
        ///</summary>
        public static void UpdateItem(BaseItem item, GetItemCallback<BaseItem> callback) => UpdateItem<BaseItem>(item, callback);

        ///<summary>
        /// Standard method to update an item in the database.
        /// Callback is invoked upon completion.
        ///</summary>
        public static void UpdateItem(BaseItem item, ModifyItemCallback callback)
        {
            Client.UpdateItem(item.IID, instance.SerializeItem(item), (json) => callback());
        }

        ///<summary>
        /// Generic method to update an item in the database.
        /// Updated BaseItem of specified type is sent to the callback once complete.
        ///</summary>
        public static void UpdateItem<T>(BaseItem item, GetItemCallback<T> callback) where T : BaseItem
        {
            Client.UpdateItem(item.IID, instance.SerializeItem(item), (json) => callback(instance.DeserializeItem<T>(json)));
        }

        ///<summary>
        /// Standard method to delete an item from the database by id.
        ///</summary>
        public static void DeleteItem(int itemIID) => DeleteItem(itemIID, () => { });

        ///<summary>
        /// Standard method to delete an item from the database by id.
        /// Upon successful removal, callback is invoked.
        ///</summary>
        public static void DeleteItem(int itemIID, ModifyItemCallback callback)
        {
            Client.DeleteItem(itemIID, (json) => callback());
        }

        ///<summary>
        /// Standard method to trade an item in the database by id to a given player.
        ///</summary>
        public static void TradeItem(int itemIID, string playerIID) => TradeItem(itemIID, playerIID, () => { });

        ///<summary>
        /// Standard method to trade an item in the database by id to a given player.
        /// Upon successful trade, callback is invoked.
        ///</summary>
        public static void TradeItem(int itemIID, string playerIID, ModifyItemCallback callback)
        {
            Client.TradeItem(itemIID, playerIID, (json) => callback());
        }

        private void InitializeClient()
        {
            if (TryGetComponent<BaseItemClient>(out BaseItemClient client))
            {
                this.client = client;
            }
            else
            {
                throw new MissingComponentException("ItemManager is missing a Client-type component!");
            }
        }

        private string SerializeItem(BaseItem item)
        {
            // Create raw item container object
            RawItemData rawData = new RawItemData();
            rawData.playerId = item.PlayerIID;
            rawData.name = item.Name;
            rawData.type = item.Type;

            // Serialize custom item data
            ItemData customData = new ItemData();
            item.Serialize(customData);
            rawData.data = JsonConvert.SerializeObject(customData);

            // Return serialized item
            return JsonUtility.ToJson(rawData);
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
            item.PlayerIID = raw.player_id;
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
            public string player_id;
            public string item_name;
            public int item_type;
            public string item_data;
        }

        [System.Serializable]
        private class RawItemData
        {
            public string playerId;
            public string name;
            public int type;
            public string data;
        }
    }
}
