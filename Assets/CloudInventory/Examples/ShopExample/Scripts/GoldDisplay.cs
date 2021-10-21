using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CloudInventory.Examples.ShopExample
{
    public class GoldDisplay : MonoBehaviour
    {
        [SerializeField] private bool isShop;
        [SerializeField] private Text goldText;

        private int player;

        private void Start()
        {
            // Hook up event listeners
            if (!isShop)
            {
                GameManager.OnPlayerChanged.AddListener(UpdatePlayer);
            }
            UpdatePlayer(isShop ? -1 : GameManager.Player);
        }

        public void UpdateDisplay(int gold)
        {
            goldText.text = gold.ToString();
        }

        private void UpdatePlayer(int player)
        {
            GameManager.GoldSystem.RemoveGoldListener(this.player, UpdateDisplay);
            GameManager.GoldSystem.AddGoldListener(player, UpdateDisplay);
            if (GameManager.GoldSystem.IsGoldInitialized(player))
            {
                UpdateDisplay(GameManager.GoldSystem.GetGold(player));
            }
            this.player = player;
        }
    }
}
