using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CloudInventory.Examples.ShopExample
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public static readonly int[] PLAYERS = new int[4] { -1, 0, 1, 2 };

        private int player = 0;
        public static int Player { get => instance.player; }

        private const int SHOP_IID = -1;
        public static int Shop { get => SHOP_IID; }

        private UnityEvent<int> onPlayerChanged = new UnityEvent<int>();
        public static UnityEvent<int> OnPlayerChanged { get => instance.onPlayerChanged; }

        private UnityEvent onPlayerLoaded = new UnityEvent();
        public static UnityEvent OnPlayerLoaded { get => instance.onPlayerLoaded; }

        public static InventorySystem InventorySystem { get; private set; }
        public static ShopSystem ShopSystem { get; private set; }
        public static GoldSystem GoldSystem { get; private set; }

        private ItemPlayFabClient playFabClient;

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

        private void Start()
        {
            // Initialize playfab
            if (ItemManager.ClientType == ItemClientType.PlayFabClient)
            {
                playFabClient = ItemManager.GetClient<ItemPlayFabClient>();
                playFabClient.ConnectPlayer("Player" + player, () => OnPlayerLoaded.Invoke());
            }
        }

        public void SetPlayer(int p)
        {
            player = p;
            if (playFabClient != null)
            {
                playFabClient.ConnectPlayer("Player" + player, () => OnPlayerChanged.Invoke(player));
            }
            else
            {
                OnPlayerChanged.Invoke(player);
            }
        }
    }
}
