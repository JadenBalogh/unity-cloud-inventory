using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CloudInventory.Examples.ShopExample
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public static readonly string[] PLAYERS = new string[4] { "Shop", "Player1", "Player2", "Player3" };

        private string player = "Player1";
        public static string Player { get => instance.player; }

        private const string SHOP_IID = "Shop";
        public static string Shop { get => SHOP_IID; }

        private UnityEvent<string> onPlayerChanged = new UnityEvent<string>();
        public static UnityEvent<string> OnPlayerChanged { get => instance.onPlayerChanged; }

        public static InventorySystem InventorySystem { get; private set; }
        public static ShopSystem ShopSystem { get; private set; }
        public static GoldSystem GoldSystem { get; private set; }

        private void Awake()
        {
            // Enforce singleton
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;

            // Initialize systems
            InventorySystem = GetComponent<InventorySystem>();
            ShopSystem = GetComponent<ShopSystem>();
            GoldSystem = GetComponent<GoldSystem>();
        }

        public void SetPlayer(int p)
        {
            player = PLAYERS[p + 1];
            OnPlayerChanged.Invoke(player);
        }
    }
}
