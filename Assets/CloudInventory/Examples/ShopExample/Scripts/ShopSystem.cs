using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CloudInventory.Examples.ShopExample
{
    public class ShopSystem : MonoBehaviour
    {
        [SerializeField] private string[] randomItemPrefixes;
        [SerializeField] private string[] randomItemNames;
        [SerializeField] private int minRandomItemPrice;
        [SerializeField] private int maxRandomItemPrice;

        public void StockRandomItem()
        {
            // Generate random attributes
            string prefix = randomItemPrefixes[Random.Range(0, randomItemPrefixes.Length)];
            string name = randomItemNames[Random.Range(0, randomItemNames.Length)];
            int price = Random.Range(minRandomItemPrice, maxRandomItemPrice + 1);

            // Create new item
            Item item = new Item();
            item.Name = prefix + " " + name;
            item.Type = (int)ItemType.Item;
            item.Price = price;

            // Add item to database
            ItemManager.SaveItem(-1, item, () => GameManager.InventorySystem.UpdateInventory(-1));
        }
    }
}
