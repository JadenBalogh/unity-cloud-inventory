using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace CloudInventory.Examples.ShopExample
{
    public class BuyButton : MonoBehaviour
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        public void UpdateInteractable(Item item)
        {
            button.interactable = item != null && GameManager.GoldSystem.CanAfford(GameManager.Player, item.Price);
        }
    }
}
