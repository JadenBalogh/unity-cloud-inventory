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

        private string player;

        private void Start()
        {
            // Hook up event listeners
            if (!isShop)
            {
                GameManager.OnPlayerChanged.AddListener(UpdatePlayer);
            }
            UpdatePlayer(isShop ? GameManager.Shop : GameManager.Player);
        }

        public void UpdateDisplay(int gold)
        {
            goldText.text = gold.ToString();
        }

        private void UpdatePlayer(string player)
        {
            if (this.player != null)
            {
                GameManager.GoldSystem.RemoveGoldListener(this.player, UpdateDisplay);
            }
            GameManager.GoldSystem.AddGoldListener(player, UpdateDisplay);
            if (GameManager.GoldSystem.IsGoldInitialized(player))
            {
                UpdateDisplay(GameManager.GoldSystem.GetGold(player));
            }
            this.player = player;
        }
    }
}
