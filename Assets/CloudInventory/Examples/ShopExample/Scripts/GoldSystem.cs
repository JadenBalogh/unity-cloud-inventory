using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CloudInventory.Examples.ShopExample
{
    public class GoldSystem : MonoBehaviour
    {
        [SerializeField] private int playerStartingGold = 25;
        [SerializeField] private int shopStartingGold = 25;

        private Dictionary<int, bool> isGoldInitialized = new Dictionary<int, bool>();
        private Dictionary<int, Gold> goldAmounts = new Dictionary<int, Gold>();
        private Dictionary<int, UnityEvent<int>> goldChangedEvents = new Dictionary<int, UnityEvent<int>>();

        private void Awake()
        {
            int[] players = GameManager.PLAYERS;
            for (int i = 0; i < players.Length; i++)
            {
                int player = players[i];

                // Initialize events
                isGoldInitialized.Add(player, false);
                goldChangedEvents.Add(player, new UnityEvent<int>());
                int startingGold = player == GameManager.Shop ? shopStartingGold : playerStartingGold;

                // Fetch gold from DB or create starting gold
                ItemManager.GetItems<Gold>(player, (int)ItemType.Currency, (items) =>
                {
                    if (items.Length > 0)
                    {
                        // Fetch existing gold
                        goldAmounts[player] = items[0];
                        isGoldInitialized[player] = true;
                        goldChangedEvents[player].Invoke(goldAmounts[player].Amount);
                    }
                    else
                    {
                        // Create starting gold
                        Gold goldItem = new Gold(player, startingGold);
                        ItemManager.CreateItem<Gold>(goldItem, (createdItem) =>
                        {
                            goldAmounts[player] = createdItem;
                            isGoldInitialized[player] = true;
                            goldChangedEvents[player].Invoke(goldAmounts[player].Amount);
                        });
                    }
                });
            }
        }

        public void RemoveGoldListener(int player, UnityAction<int> listener)
        {
            goldChangedEvents[player].RemoveListener(listener);
        }

        public void AddGoldListener(int player, UnityAction<int> listener)
        {
            goldChangedEvents[player].AddListener(listener);
        }

        public bool IsGoldInitialized(int player)
        {
            return isGoldInitialized[player];
        }

        public bool CanAfford(int player, int price)
        {
            return goldAmounts[player].Amount >= price;
        }

        public void SpendGold(int player, int amount)
        {
            goldAmounts[player].Amount -= amount;
            goldChangedEvents[player].Invoke(goldAmounts[player].Amount);
            ItemManager.UpdateItem(goldAmounts[player]);
        }

        public void AddGold(int player, int amount)
        {
            goldAmounts[player].Amount += amount;
            goldChangedEvents[player].Invoke(goldAmounts[player].Amount);
            ItemManager.UpdateItem(goldAmounts[player]);
        }

        public int GetGold(int player)
        {
            return goldAmounts[player].Amount;
        }
    }
}
