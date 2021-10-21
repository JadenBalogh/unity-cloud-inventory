using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace CloudInventory.Examples.ShopExample
{
    public class InventoryDisplay : MonoBehaviour
    {
        [SerializeField] private bool isShop;
        [SerializeField] private RectTransform contentParent;
        [SerializeField] private ItemDisplay itemDisplayPrefab;
        [SerializeField] private UnityEvent<Item> onSelectionChanged;

        private int player;
        private ItemDisplay selectedDisplay;
        private List<ItemDisplay> itemDisplays = new List<ItemDisplay>();

        private void Start()
        {
            // Hook up event listeners
            if (!isShop)
            {
                GameManager.OnPlayerChanged.AddListener(UpdatePlayer);
            }
            UpdatePlayer(isShop ? GameManager.Shop : GameManager.Player);
        }

        public void BuySelected()
        {
            GameManager.InventorySystem.BuyItem(GameManager.Player, selectedDisplay.Item);
            RefreshSelection();
        }

        public void SellSelected()
        {
            GameManager.InventorySystem.SellItem(GameManager.Player, selectedDisplay.Item);
            RefreshSelection();
        }

        public void DeleteSelected()
        {
            GameManager.InventorySystem.DeleteItem(GameManager.Player, selectedDisplay.Item);
            RefreshSelection();
        }

        private void UpdateDisplay(Item[] items)
        {
            ClearDisplay();
            float height = itemDisplayPrefab.GetComponent<RectTransform>().sizeDelta.y;
            for (int i = 0; i < items.Length; i++)
            {
                ItemDisplay display = Instantiate(itemDisplayPrefab, contentParent);
                display.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -height);
                display.UpdateDisplay(items[i]);
                display.OnSelected.AddListener(() => UpdateSelected(display));
                itemDisplays.Add(display);
            }
            contentParent.sizeDelta = new Vector2(contentParent.sizeDelta.x, items.Length * height);
        }

        private void ClearDisplay()
        {
            UpdateSelected(null);
            foreach (ItemDisplay display in itemDisplays)
            {
                Destroy(display.gameObject);
            }
            itemDisplays = new List<ItemDisplay>();
        }

        private void UpdateSelected(ItemDisplay selected)
        {
            // Update selections
            if (selectedDisplay != null)
            {
                selectedDisplay.SetSelected(false);
            }
            selectedDisplay = selected;
            if (selectedDisplay != null)
            {
                selectedDisplay.SetSelected(true);
            }
            RefreshSelection();
        }

        private void RefreshSelection()
        {
            // Send selection changed event
            Item item = selectedDisplay != null ? selectedDisplay.Item : null;
            onSelectionChanged.Invoke(item);
        }

        private void UpdatePlayer(int player)
        {
            GameManager.InventorySystem.RemoveItemsListener(this.player, UpdateDisplay);
            GameManager.InventorySystem.AddItemsListener(player, UpdateDisplay);
            this.player = player;
        }
    }
}
