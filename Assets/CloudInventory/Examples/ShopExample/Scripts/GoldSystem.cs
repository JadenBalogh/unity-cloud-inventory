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
        [SerializeField] private int minQuestGold = 5;
        [SerializeField] private int maxQuestGold = 15;

        private Dictionary<string, bool> isGoldInitialized = new Dictionary<string, bool>();
        private Dictionary<string, Gold> goldAmounts = new Dictionary<string, Gold>();
        private Dictionary<string, UnityEvent<int>> goldChangedEvents = new Dictionary<string, UnityEvent<int>>();

        private void Awake()
        {
            string[] players = GameManager.PLAYERS;
            for (int i = 0; i < players.Length; i++)
            {
                string player = players[i];

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

        public void RemoveGoldListener(string player, UnityAction<int> listener)
        {
            goldChangedEvents[player].RemoveListener(listener);
        }

        public void AddGoldListener(string player, UnityAction<int> listener)
        {
            goldChangedEvents[player].AddListener(listener);
        }

        public bool IsGoldInitialized(string player)
        {
            return isGoldInitialized[player];
        }

        public bool CanAfford(string player, int price)
        {
            return goldAmounts[player].Amount >= price;
        }

        public void SpendGold(string player, int amount)
        {
            goldAmounts[player].Amount -= amount;
            goldChangedEvents[player].Invoke(goldAmounts[player].Amount);
            ItemManager.UpdateItem(goldAmounts[player]);
        }

        public void AddGold(string player, int amount)
        {
            goldAmounts[player].Amount += amount;
            goldChangedEvents[player].Invoke(goldAmounts[player].Amount);
            ItemManager.UpdateItem(goldAmounts[player]);
        }

        public void PlayQuest()
        {
            // a.k.a. - give the current player some gold
            AddGold(GameManager.Player, Random.Range(minQuestGold, maxQuestGold));
        }

        public int GetGold(string player)
        {
            return goldAmounts[player].Amount;
        }
    }
}
