using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public delegate void LoadItemsCallback(BaseItem[] items);
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

    public static void LoadItems(int playerIID, LoadItemsCallback callback)
    {
        instance.Client.LoadItems(playerIID, (json) =>
        {
            RawItem[] rawItems = JsonUtility.FromJson<RawItems>(json).items;
            BaseItem[] items = new BaseItem[rawItems.Length];
            for (int i = 0; i < rawItems.Length; i++)
            {
                RawItem raw = rawItems[i];
                items[i] = new BaseItem(raw.id, raw.item_name);
            }
            callback(items);
        });
    }

    public static void SaveItem(int playerIID, BaseItem item, SaveItemsCallback callback)
    {
        RawItemData itemData = new RawItemData();
        itemData.name = item.Name;
        string body = JsonUtility.ToJson(itemData);
        Debug.Log(body);
        instance.Client.SaveItem(playerIID, body, (json) => callback());
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
    }

    [System.Serializable]
    private class RawItemData
    {
        public string name;
    }
}
