using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CloudInventory.Examples.ItemListExample
{
    public class ItemDisplay : MonoBehaviour
    {
        private const float USAGE_COOLDOWN = 1.5f;

        [SerializeField] private Text itemsTextbox;
        [SerializeField] private InputField itemNameInput;

        private bool usageCooldownActive = false;

        private void Start()
        {
            RefreshItems();
        }

        public void RefreshItems()
        {
            if (usageCooldownActive) return;
            ItemManager.LoadItems<Item>(0, UpdateDisplay);
            StartCoroutine(UsageCooldown());
        }

        public void AddItem()
        {
            string itemName = itemNameInput.text;
            if (itemName == "") return;
            itemNameInput.text = "";

            Item item = new Item();
            item.IID = 0;
            item.Name = itemName;
            item.Price = Random.Range(1, 9);
            ItemManager.SaveItem(0, item, () => RefreshItems());
        }

        private void UpdateDisplay(Item[] items)
        {
            string output = "";
            foreach (Item item in items)
            {
                output += $"{item.Name} (id = {item.IID}) - ${item.Price}\n";
            }
            itemsTextbox.text = output;
        }

        private IEnumerator UsageCooldown()
        {
            usageCooldownActive = true;
            yield return new WaitForSeconds(USAGE_COOLDOWN);
            usageCooldownActive = false;
        }
    }
}
