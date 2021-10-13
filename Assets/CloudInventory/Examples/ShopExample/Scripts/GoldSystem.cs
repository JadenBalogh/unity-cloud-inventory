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

        private Dictionary<int, int> goldAmounts = new Dictionary<int, int>();
        private Dictionary<int, UnityEvent<int>> goldChangedEvents = new Dictionary<int, UnityEvent<int>>();

        private void Awake()
        {
            // Initialize events
            int[] players = GameManager.PLAYERS;
            for (int i = 0; i < players.Length; i++)
            {
                goldChangedEvents.Add(players[i], new UnityEvent<int>());
                int startingGold = players[i] == -1 ? shopStartingGold : playerStartingGold;
                goldAmounts[players[i]] = startingGold;
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

        public bool CanAfford(int player, int price)
        {
            return goldAmounts[player] >= price;
        }

        public void SpendGold(int player, int amount)
        {
            goldAmounts[player] -= amount;
            goldChangedEvents[player].Invoke(goldAmounts[player]);
        }

        public void AddGold(int player, int amount)
        {
            goldAmounts[player] += amount;
            goldChangedEvents[player].Invoke(goldAmounts[player]);
        }

        public int GetGold(int player)
        {
            return goldAmounts[player];
        }
    }
}
