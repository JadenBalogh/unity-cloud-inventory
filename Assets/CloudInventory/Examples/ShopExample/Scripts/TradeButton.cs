using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace CloudInventory.Examples.ShopExample
{
    public class TradeButton : MonoBehaviour
    {
        [SerializeField] private bool requireGold;
        [SerializeField] private bool isTargetShop;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        public void UpdateInteractable(Item item)
        {
            button.interactable = item != null && CanAfford(item);
        }

        private bool CanAfford(Item item)
        {
            string player = isTargetShop ? GameManager.Shop : GameManager.Player;
            return !requireGold || GameManager.GoldSystem.CanAfford(player, item.Price);
        }
    }
}
