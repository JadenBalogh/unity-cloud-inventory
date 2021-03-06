using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CloudInventory.Examples.ShopExample
{
    public class InventorySystem : MonoBehaviour
    {
        private Dictionary<string, UnityEvent<Item[]>> itemsChangedEvents = new Dictionary<string, UnityEvent<Item[]>>();

        private void Awake()
        {
            // Initialize events
            string[] players = GameManager.PLAYERS;
            for (int i = 0; i < players.Length; i++)
            {
                itemsChangedEvents.Add(players[i], new UnityEvent<Item[]>());
            }
        }

        private void Start()
        {
            // Hook up event listeners
            GameManager.OnPlayerChanged.AddListener(UpdateInventory);

            // Initialize shop
            UpdateInventory(GameManager.Shop);

            // Initialize current player
            UpdateInventory(GameManager.Player);
        }

        public void RemoveItemsListener(string player, UnityAction<Item[]> listener)
        {
            itemsChangedEvents[player].RemoveListener(listener);
        }

        public void AddItemsListener(string player, UnityAction<Item[]> listener)
        {
            itemsChangedEvents[player].AddListener(listener);
        }

        public void UpdateInventory(string player)
        {
            ItemManager.GetItems<Item>(player, (int)ItemType.Item, (items) => itemsChangedEvents[player].Invoke(items));
        }

        public void BuyItem(string player, Item item)
        {
            if (GameManager.GoldSystem.CanAfford(player, item.Price))
            {
                // Update gold amounts
                GameManager.GoldSystem.AddGold(GameManager.Shop, item.Price);
                GameManager.GoldSystem.SpendGold(player, item.Price);

                // Trade the item to the current player
                ItemManager.TradeItem(item.IID, player, () =>
                {
                    // Update shop and player
                    UpdateInventory(GameManager.Shop);
                    UpdateInventory(player);
                });
            }
        }

        public void SellItem(string player, Item item)
        {
            if (GameManager.GoldSystem.CanAfford(GameManager.Shop, item.Price))
            {
                // Update gold amounts
                GameManager.GoldSystem.AddGold(player, item.Price);
                GameManager.GoldSystem.SpendGold(GameManager.Shop, item.Price);

                // Trade the item to the shop
                ItemManager.TradeItem(item.IID, GameManager.Shop, () =>
                {
                    // Update shop and player
                    UpdateInventory(GameManager.Shop);
                    UpdateInventory(player);
                });
            }
        }

        public void DeleteItem(string player, Item item)
        {
            ItemManager.DeleteItem(item.IID, () =>
            {
                UpdateInventory(GameManager.Shop);
                UpdateInventory(player);
            });
        }
    }
}
