using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CloudInventory.Examples.ShopExample
{
    public class InventorySystem : MonoBehaviour
    {
        private Dictionary<int, UnityEvent<Item[]>> itemsChangedEvents = new Dictionary<int, UnityEvent<Item[]>>();

        private void Awake()
        {
            // Initialize events
            int[] players = GameManager.PLAYERS;
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
            UpdateInventory(-1);

            // Initialize current player
            UpdateInventory(GameManager.Player);
        }

        public void RemoveItemsListener(int player, UnityAction<Item[]> listener)
        {
            itemsChangedEvents[player].RemoveListener(listener);
        }

        public void AddItemsListener(int player, UnityAction<Item[]> listener)
        {
            itemsChangedEvents[player].AddListener(listener);
        }

        public void UpdateInventory(int player)
        {
            ItemManager.LoadItems<Item>(player, (items) => itemsChangedEvents[player].Invoke(items));
        }

        public void BuyItem(int player, Item item)
        {
            if (GameManager.GoldSystem.CanAfford(player, item.Price))
            {
                // Update gold amounts
                GameManager.GoldSystem.AddGold(-1, item.Price);
                GameManager.GoldSystem.SpendGold(player, item.Price);

                // TODO: remove items / trade items
                ItemManager.SaveItem(player, item, () => UpdateInventory(player));
            }
        }
    }
}
