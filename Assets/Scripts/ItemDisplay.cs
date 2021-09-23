using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        ItemManager.GetItems(UpdateDisplay);
        StartCoroutine(UsageCooldown());
    }

    public void AddItem()
    {
        if (usageCooldownActive) return;
        string itemName = itemNameInput.text;
        if (itemName == "") return;
        itemNameInput.text = "";
        ItemManager.AddItem(itemName, () => RefreshItems());
        StartCoroutine(UsageCooldown());
    }

    private void UpdateDisplay(Item[] items)
    {
        string output = "";
        foreach (Item item in items)
        {
            output += $"{item.name} (id = {item.id})\n";
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
